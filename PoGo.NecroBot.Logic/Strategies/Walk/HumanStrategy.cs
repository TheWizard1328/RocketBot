﻿using System;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Model;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using GeoCoordinatePortable;

namespace PoGo.NecroBot.Logic.Strategies.Walk
{
    class HumanStrategy : BaseWalkStrategy
    {
        private double CurrentWalkingSpeed = 0;

        public HumanStrategy(Client client) : base(client)
        {
        }

        public override string RouteName => "NecroBot Walk";

        public override async Task Walk(IGeoLocation targetLocation,
            Func<Task> functionExecutedWhileWalking, ISession session, CancellationToken cancellationToken,
            double walkSpeed = 0.0)
        {
            base.OnStartWalking(session, targetLocation);

            var destinaionCoordinate = new GeoCoordinate(targetLocation.Latitude, targetLocation.Longitude);

            if (CurrentWalkingSpeed <= 0)
                CurrentWalkingSpeed = session.LogicSettings.WalkingSpeedInKilometerPerHour;
            if (session.LogicSettings.UseWalkingSpeedVariant && walkSpeed == 0)
                CurrentWalkingSpeed = session.Navigation.VariantRandom(session, CurrentWalkingSpeed);

            var rw = new Random();
            var speedInMetersPerSecond = (walkSpeed > 0 ? walkSpeed : CurrentWalkingSpeed) / 3.6;

            var sourceLocation = new GeoCoordinate(_client.ClientSession.Player.Latitude, _client.ClientSession.Player.Longitude);

            var nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, destinaionCoordinate);


            var nextWaypointDistance = speedInMetersPerSecond;
            var waypoint = await LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing).ConfigureAwait(false);
            var requestSendDateTime = DateTime.Now;
            var requestVariantDateTime = DateTime.Now;

            await LocationUtils.UpdatePlayerLocationWithAltitude(session, waypoint, (float) speedInMetersPerSecond).ConfigureAwait(false);

            double SpeedVariantSec = rw.Next(1000, 10000);
            base.DoUpdatePositionEvent(session, waypoint.Latitude, waypoint.Longitude, CurrentWalkingSpeed);

            do
            {
                cancellationToken.ThrowIfCancellationRequested();
                TinyIoC.TinyIoCContainer.Current.Resolve<MultiAccountManager>().ThrowIfSwitchAccountRequested();
                var millisecondsUntilGetUpdatePlayerLocationResponse =
                    (DateTime.Now - requestSendDateTime).TotalMilliseconds;
                var millisecondsUntilVariant =
                    (DateTime.Now - requestVariantDateTime).TotalMilliseconds;

                sourceLocation = new GeoCoordinate(_client.ClientSession.Player.Latitude, _client.ClientSession.Player.Longitude);
                var currentDistanceToTarget = LocationUtils
                    .CalculateDistanceInMeters(sourceLocation, destinaionCoordinate);

                if (currentDistanceToTarget < 40)
                    if (speedInMetersPerSecond > SpeedDownTo)
                        speedInMetersPerSecond = SpeedDownTo;

                if (session.LogicSettings.UseWalkingSpeedVariant && walkSpeed == 0)
                {
                    CurrentWalkingSpeed = session.Navigation.VariantRandom(session, CurrentWalkingSpeed);
                }

                speedInMetersPerSecond = (walkSpeed > 0 ? walkSpeed : CurrentWalkingSpeed) / 3.6;

                nextWaypointDistance = Math.Min(currentDistanceToTarget,
                    millisecondsUntilGetUpdatePlayerLocationResponse / 1000 * speedInMetersPerSecond);
                nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, destinaionCoordinate);
                var testeBear = LocationUtils.DegreeBearing(sourceLocation, new GeoCoordinate(40.780396, -73.974844));
                waypoint = await LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing).ConfigureAwait(false);

                requestSendDateTime = DateTime.Now;
                await LocationUtils.UpdatePlayerLocationWithAltitude(session, waypoint, (float) speedInMetersPerSecond).ConfigureAwait(false);

                base.DoUpdatePositionEvent(session, waypoint.Latitude, waypoint.Longitude, CurrentWalkingSpeed);

                if (functionExecutedWhileWalking != null)
                    await functionExecutedWhileWalking().ConfigureAwait(false); // look for pokemon
            } while (LocationUtils.CalculateDistanceInMeters(sourceLocation, destinaionCoordinate) >= (new Random()).Next(1, 10));
        }

        public override double CalculateDistance(double sourceLat, double sourceLng, double destinationLat,
            double destinationLng, ISession session = null)
        {
            return LocationUtils.CalculateDistanceInMeters(sourceLat, sourceLng, destinationLat, destinationLng);
        }
    }
}