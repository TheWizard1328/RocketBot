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
           return new InfoState();
        }
    }
}