#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using Google.Protobuf;
using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Model;
using PoGo.NecroBot.Logic.Tasks;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Enums;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class CatchState : IState
    {
        public class CatchablePokemon
        {
            public ulong EncounteredId { get; set; }
            public bool Checked { get; set; }
            public DateTime ExpiredTime { get; set; }
            public double Latitude { get; internal set; }
            public double Longitude { get; internal set; }
            public string SpawnId { get; internal set; }

            public PokemonId PokemonId { get; internal set; }

            public MapPokemon ToMapPokemon()
            {
                return new MapPokemon()
                {
                    EncounterId = EncounteredId,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    SpawnPointId = SpawnId,
                    PokemonId = PokemonId
                };
            }
        }

        public async Task<IState> Execute(ISession session, CancellationToken cancellationToken)
        {
            //Console.WriteLine($"waiting data....., {data.Count}");
            CatchablePokemon pkm = null;
            var currentLatitude = session.Client.ClientSession.Player.Latitude;
            var currentLongitude = session.Client.ClientSession.Player.Longitude;

            do
            {
                pkm = data.FirstOrDefault(p => !p.Checked &&
                                               p.ExpiredTime > DateTime.Now &&
                                               LocationUtils.CalculateDistanceInMeters(
                                                   currentLatitude,
                                                   currentLongitude,
                                                   p.Latitude,
                                                   p.Longitude
                                               ) < 100);
                if (pkm == null) break;

                EncounterResponse encounter;
                try
                {
                    //await
                    //    LocationUtils.UpdatePlayerLocationWithAltitude(session,
                    //        new GeoCoordinate(pkm.Latitude, pkm.Longitude, session.Client.CurrentAltitude), 0).ConfigureAwait(false); // Set speed to 0 for random speed.

                    var CurrentAltitude = new GeoCoordinate(session.Client.ClientSession.Player.Latitude,session.Client.ClientSession.Player.Longitude).Altitude;

                    await session.Navigation.Move(
                        new MapLocation(pkm.Latitude, pkm.Longitude, CurrentAltitude),
                        null, session, cancellationToken).ConfigureAwait(false);

                    var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
                    {
                        RequestType = RequestType.Encounter,
                        RequestMessage = new EncounterMessage
                        {
                            EncounterId = pkm.EncounteredId,
                            PlayerLatitude = session.Client.ClientSession.Player.Latitude,
                            PlayerLongitude = session.Client.ClientSession.Player.Longitude,
                            SpawnPointId = pkm.SpawnId
                        }.ToByteString()
                    }, true);

                    encounter = EncounterResponse.Parser.ParseFrom(response);
                }
                finally
                {
                    //await
                    //    var CurrentAltitude = new GeoCoordinate(session.Client.ClientSession.Player.Latitude,session.Client.ClientSession.Player.Longitude).Altitude;
                    //    LocationUtils.UpdatePlayerLocationWithAltitude(session,
                    //        new GeoCoordinate(currentLatitude, currentLongitude, CurrentAltitude), 0).ConfigureAwait(false); // Set speed to 0 for random speed.
                }
                switch (encounter.Status)
                {
                    case EncounterResponse.Types.Status.EncounterSuccess:
                        await CatchPokemonTask.Execute(session, cancellationToken, encounter, pkm.ToMapPokemon(),
                            currentFortData: null, sessionAllowTransfer: true).ConfigureAwait(false);
                        break;
                    case EncounterResponse.Types.Status.PokemonInventoryFull:
                        if (session.LogicSettings.TransferDuplicatePokemon)
                        {
                            await TransferDuplicatePokemonTask.Execute(session, cancellationToken).ConfigureAwait(false);
                        }
                        else
                        {
                            session.EventDispatcher.Send(new WarnEvent
                            {
                                Message = session.Translation.GetTranslation(TranslationString.InvFullTransferManually)
                            });
                        }

                        if (EvolvePokemonTask.IsActivated(session))
                        {
                            await EvolvePokemonTask.Execute(session, cancellationToken).ConfigureAwait(false);
                        }
                        break;

                    default:
                        session.EventDispatcher.Send(new WarnEvent
                        {
                            Message =
                                session.Translation.GetTranslation(
                                    TranslationString.EncounterProblem, encounter.Status)
                        });
                        break;
                }
                pkm.Checked = true;
                await Task.Delay(1000).ConfigureAwait(false);
            } while (pkm != null);
            await Task.Delay(3000).ConfigureAwait(false);
            return this;
        }

        private static List<CatchablePokemon> data = new List<CatchablePokemon>();

        public static PokemonId GetId(string name)
        {
            var t = name[0];
            var realName = new StringBuilder(name.ToLower());
            realName[0] = t;
            try
            {
                var p = (PokemonId) Enum.Parse(typeof(PokemonId), realName.ToString());
                return p;
            }
            catch (Exception)
            {
            }
            return PokemonId.Missingno;
        }

        private static CatchablePokemon Map(HumanWalkSnipeTask.FastPokemapItem result)
        {
            return new CatchablePokemon
            {
                Latitude = result.lnglat.coordinates[1],
                Longitude = result.lnglat.coordinates[0],
                PokemonId = GetId(result.pokemon_id),
                ExpiredTime = result.expireAt.ToLocalTime(),
                SpawnId = result.spawn_id,
                //Source = "Fastpokemap"
                EncounteredId = Convert.ToUInt64(result.encounter_id)
            };
        }

        public static void AddFastPokemapItem(dynamic jsonData)
        {
            var list = JsonConvert.DeserializeObject<List<HumanWalkSnipeTask.FastPokemapItem>>(jsonData.ToString());

            //List<CatchablePokemon> result = new List<CatchablePokemon>();
            foreach (var item in list)
            {
                var snipeItem = Map(item);
                if (!data.Exists(p => p.EncounteredId == snipeItem.EncounteredId))
                {
                    data.Add(snipeItem);
                }
            }
        }
    }
}