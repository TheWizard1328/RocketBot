using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Tasks;
using POGOProtos.Enums;
using System;
using GeoCoordinatePortable;

namespace PoGo.NecroBot.Logic.State
{
    public class BotSwitcherState : IState
    {
        private EncounteredEvent encounterData;
        private PokemonId pokemonToCatch;

        public BotSwitcherState(PokemonId pokemon)
        {
            pokemonToCatch = pokemon;
        }

        public BotSwitcherState(PokemonId pokemon, EncounteredEvent encounterData) : this(pokemon)
        {
            this.encounterData = encounterData;
        }

        public async Task<IState> Execute(ISession session, CancellationToken cancellationToken)
        {
            if (encounterData == null)
            {
                var CurrentAltitude = new GeoCoordinate(session.Client.ClientSession.Player.Latitude,
                    session.Client.ClientSession.Player.Longitude).Altitude;
                session.Client.ClientSession.Player.SetCoordinates(session.Client.ClientSession.Player.Latitude,
                    session.Client.ClientSession.Player.Longitude, CurrentAltitude);
                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
                await CatchNearbyPokemonsTask.Execute(session, cancellationToken, pokemonToCatch).ConfigureAwait(false);
                await CatchLurePokemonsTask.Execute(session, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                //snipe pokemon 
                await MSniperServiceTask.CatchWithSnipe(session, new MSniperServiceTask.MSniperInfo2()
                {
                    AddedTime = DateTime.Now,
                    Latitude = encounterData.Latitude, 
                    Longitude = encounterData.Longitude,
                    Iv = encounterData.IV, 
                    PokemonId =(short)encounterData.PokemonId,
                    SpawnPointId = encounterData.SpawnPointId,
                    EncounterId = Convert.ToUInt64(encounterData.EncounterId)
                }, session.CancellationTokenSource.Token).ConfigureAwait(false);
            }
            return new InfoState();
        }
    }
}