#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using Google.Protobuf;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Map.Fort;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    //add delegate
    public delegate void PokemonsEncounterLureDelegate(List<MapPokemon> pokemons);
    public static class CatchLurePokemonsTask
    {
        public static async Task Execute(ISession session, FortData currentFortData,
            CancellationToken cancellationToken)
        {
            TinyIoC.TinyIoCContainer.Current.Resolve<MultiAccountManager>().ThrowIfSwitchAccountRequested();
            cancellationToken.ThrowIfCancellationRequested();
            if (!session.LogicSettings.CatchPokemon ||
                session.CatchBlockTime > DateTime.Now) return;

            Logger.Write(session.Translation.GetTranslation(TranslationString.LookingForLurePokemon), LogLevel.Debug);

            var pokemonId = currentFortData.LureInfo.ActivePokemonId;

            if ((session.LogicSettings.UsePokemonToCatchLocallyListOnly &&
                 !session.LogicSettings.PokemonToCatchLocally.Pokemon.Contains(pokemonId)) ||
                (session.LogicSettings.UsePokemonToNotCatchFilter &&
                 session.LogicSettings.PokemonsNotToCatch.Contains(pokemonId)))
            {
                session.EventDispatcher.Send(new NoticeEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.PokemonSkipped, pokemonId)
                });
            }
            else
            {
                var encounterId = currentFortData.LureInfo.EncounterId;
                if (session.Cache.Get(CatchPokemonTask.GetEncounterCacheKey(currentFortData.LureInfo.EncounterId)) != null)
                    return; //pokemon been ignore before

                var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
                {
                    RequestType = RequestType.DiskEncounter,
                    RequestMessage = new DiskEncounterMessage
                    {
                        EncounterId = encounterId,
                        FortId = currentFortData.Id,
                        GymLatDegrees = currentFortData.Latitude,
                        GymLngDegrees = currentFortData.Longitude,
                        PlayerLatitude = session.Client.ClientSession.Player.Latitude,
                        PlayerLongitude = session.Client.ClientSession.Player.Longitude
                    }.ToByteString()
                }, true);

                DiskEncounterResponse diskEncounterResponse = null;

                diskEncounterResponse = DiskEncounterResponse.Parser.ParseFrom(response);

                if (diskEncounterResponse.Result == DiskEncounterResponse.Types.Result.Success &&
                    session.LogicSettings.CatchPokemon)
                {
                    var pokemon = new MapPokemon
                    {
                        EncounterId = encounterId,
                        ExpirationTimestampMs = currentFortData.LureInfo.LureExpiresTimestampMs,
                        Latitude = currentFortData.Latitude,
                        Longitude = currentFortData.Longitude,
                        PokemonId = currentFortData.LureInfo.ActivePokemonId,
                        SpawnPointId = currentFortData.Id
                    };

                    //add delegate function
                    OnPokemonEncounterEvent(new List<MapPokemon> { pokemon });

                    // Catch the Pokemon
                    await CatchPokemonTask.Execute(session, cancellationToken, diskEncounterResponse, pokemon,
                        currentFortData, sessionAllowTransfer: true).ConfigureAwait(false);
                }
                else if (diskEncounterResponse.Result == DiskEncounterResponse.Types.Result.PokemonInventoryFull)
                {
                    if (session.LogicSettings.TransferDuplicatePokemon || session.LogicSettings.TransferWeakPokemon)
                    {
                        session.EventDispatcher.Send(new WarnEvent
                        {
                            Message = session.Translation.GetTranslation(TranslationString.InvFullTransferring)
                        });
                        if (session.LogicSettings.TransferDuplicatePokemon)
                            await TransferDuplicatePokemonTask.Execute(session, cancellationToken).ConfigureAwait(false);
                        if (session.LogicSettings.TransferWeakPokemon)
                            await TransferWeakPokemonTask.Execute(session, cancellationToken).ConfigureAwait(false);
                        if (EvolvePokemonTask.IsActivated(session))
                            await EvolvePokemonTask.Execute(session, cancellationToken).ConfigureAwait(false);
                    }
                    else
                        session.EventDispatcher.Send(new WarnEvent
                        {
                            Message = session.Translation.GetTranslation(TranslationString.InvFullTransferManually)
                        });
                }
                else
                {
                    if (diskEncounterResponse.Result.ToString().Contains("NotAvailable")) return;
                    session.EventDispatcher.Send(new WarnEvent
                    {
                        Message =
                            session.Translation.GetTranslation(TranslationString.EncounterProblemLurePokemon,
                                diskEncounterResponse.Result)
                    });
                }
            }
        }

        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            // Looking for any lure pokestop neaby

            var mapObjects = session.Client.ClientSession.Map.Cells;
            var pokeStops = mapObjects.SelectMany(i => i.Forts)
                .Where(
                    i =>
                        (i.Type == FortType.Checkpoint) &&
                        i.CooldownCompleteTimestampMs < DateTime.UtcNow.ToUnixTime()
                );

            session.AddForts(pokeStops.ToList());

            var forts = session.Forts.Where(p => p.Type == FortType.Checkpoint);
            List<FortData> luredNearBy = new List<FortData>();

            foreach (FortData fort in forts)
            {
                var distance = LocationUtils.CalculateDistanceInMeters(session.Client.ClientSession.Player.Latitude,
                    session.Client.ClientSession.Player.Longitude, fort.Latitude, fort.Longitude);
                if (distance < 40 && fort.LureInfo != null)
                {
                    luredNearBy.Add(fort);
                    await Execute(session, fort, cancellationToken).ConfigureAwait(false);
                }
            }
            ;
        }
        //add delegate event
        public static event PokemonsEncounterLureDelegate PokemonEncounterEvent;

        private static void OnPokemonEncounterEvent(List<MapPokemon> pokemons)
        {
            PokemonEncounterEvent?.Invoke(pokemons);
        }
    }
}