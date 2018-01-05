using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event.Player;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.Logging;
using POGOProtos.Data;
using POGOProtos.Enums;
using System;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using Google.Protobuf;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class SelectBuddyPokemonTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken, ulong pokemonId = 0)
        {
            PokemonData newBuddy = null;
            if (pokemonId == 0)
            {
                if (string.IsNullOrEmpty(session.LogicSettings.DefaultBuddyPokemon))
                    return;

                PokemonId buddyPokemonId;
                bool success = Enum.TryParse(session.LogicSettings.DefaultBuddyPokemon, out buddyPokemonId);
                if (!success)
                {
                    // Invalid buddy pokemon type
                    Logger.Write($"The DefaultBuddyPokemon ({session.LogicSettings.DefaultBuddyPokemon}) is not a valid pokemon.", LogLevel.Error);
                    return;
                }

                if (session.Profile.PlayerData.BuddyPokemon?.Id > 0)
                {
                    var currentBuddy = (await session.Inventory.GetPokemons().ConfigureAwait(false)).FirstOrDefault(x => x.Id == session.Profile.PlayerData.BuddyPokemon.Id);
                    if (currentBuddy.PokemonId == buddyPokemonId)
                    {
                        //dont change same buddy
                        return;
                    }
                }

                var buddy = (await session.Inventory.GetPokemons().ConfigureAwait(false)).Where(x => x.PokemonId == buddyPokemonId)
                .OrderByDescending(x => PokemonInfo.CalculateCp(x));

                if (session.LogicSettings.PrioritizeIvOverCp)
                {
                    buddy = buddy.OrderByDescending(x => PokemonInfo.CalculatePokemonPerfection(x));
                }
                newBuddy = buddy.FirstOrDefault();

                if (newBuddy == null)
                {
                    Logger.Write($"You don't have the pokemon {session.LogicSettings.DefaultBuddyPokemon} to set as your buddy");
                    return;
                }
            }
            if (newBuddy == null)
            {
                newBuddy = (await session.Inventory.GetPokemons().ConfigureAwait(false)).FirstOrDefault(x => x.Id == pokemonId);
            }
            if (newBuddy == null) return;

            var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
            {
                RequestType = RequestType.SetBuddyPokemon,
                RequestMessage = new SetBuddyPokemonMessage
                {
                    PokemonId = newBuddy.Id
                }.ToByteString()
            }, true);

            SetBuddyPokemonResponse setBuddyPokemonResponse = null;

            setBuddyPokemonResponse = SetBuddyPokemonResponse.Parser.ParseFrom(response);

            if (setBuddyPokemonResponse.Result == SetBuddyPokemonResponse.Types.Result.Success)
            {
                session.EventDispatcher.Send(new BuddyUpdateEvent(setBuddyPokemonResponse.UpdatedBuddy, newBuddy));
            }
        }
    }
}