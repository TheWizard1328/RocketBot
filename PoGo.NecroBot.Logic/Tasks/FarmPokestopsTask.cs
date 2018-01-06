#region using directives

using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.Model;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using GeoCoordinatePortable;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class FarmPokestopsTask
    {
        private static bool checkForMoveBackToDefault = true;

        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            TinyIoC.TinyIoCContainer.Current.Resolve<MultiAccountManager>().ThrowIfSwitchAccountRequested();
            var distanceFromStart = LocationUtils.CalculateDistanceInMeters(
                session.Settings.AccountLatitude, session.Settings.AccountLongitude,
                session.Client.ClientSession.Player.Latitude, session.Client.ClientSession.Player.Longitude);
            var CurrentAltitude = new GeoCoordinate(session.Client.ClientSession.Player.Latitude, session.Client.ClientSession.Player.Longitude).Altitude;

            await LocationUtils.UpdatePlayerLocationWithAltitude(session, new GeoCoordinate(session.Client.ClientSession.Player.Latitude, session.Client.ClientSession.Player.Longitude, CurrentAltitude), session.Client.CurrentSpeed).ConfigureAwait(false);
            // Edge case for when the client somehow ends up outside the defined radius
            if (session.LogicSettings.MaxTravelDistanceInMeters != 0 && checkForMoveBackToDefault &&
                distanceFromStart > session.LogicSettings.MaxTravelDistanceInMeters)
            {
                checkForMoveBackToDefault = false;
                Logger.Write(
                    session.Translation.GetTranslation(TranslationString.FarmPokestopsOutsideRadius, distanceFromStart),
                    LogLevel.Warning);

                var eggWalker = new EggWalker(1000, session);

                var defaultLocation = new MapLocation(session.Settings.AccountLatitude,
                    session.Settings.AccountLongitude,
                    await LocationUtils.GetElevation(session.ElevationService, session.Settings.AccountLatitude,
                        session.Settings.AccountLongitude).ConfigureAwait(false)
                );

                // we have moved this distance, so apply it immediately to the egg walker.
                await eggWalker.ApplyDistance(distanceFromStart, cancellationToken).ConfigureAwait(false);
            }
            checkForMoveBackToDefault = false;

            await CatchNearbyPokemonsTask.Execute(session, cancellationToken).ConfigureAwait(false);

            // initialize the variables in UseNearbyPokestopsTask here, as this is a fresh start.
            UseNearbyPokestopsTask.Initialize();
            await UseNearbyPokestopsTask.Execute(session, cancellationToken).ConfigureAwait(false);
        }
    }
}
