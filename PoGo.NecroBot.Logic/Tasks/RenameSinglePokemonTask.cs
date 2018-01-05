#region using directives

using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Networking.Responses;
using System.Linq;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using Google.Protobuf;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class RenameSinglePokemonTask
    {
        public static async Task Execute(ISession session, ulong pokemonId, string newNickname, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            TinyIoC.TinyIoCContainer.Current.Resolve<MultiAccountManager>().ThrowIfSwitchAccountRequested();
            var pokemon = (await session.Inventory.GetPokemons().ConfigureAwait(false)).Where(x => x.Id == pokemonId).FirstOrDefault();

            if (pokemon == null || pokemon.Nickname == newNickname)
                return;

            if (newNickname.Length > 12)
                newNickname = newNickname.Substring(0, 12);

            var oldNickname = string.IsNullOrEmpty(pokemon.Nickname) ? pokemon.PokemonId.ToString() : pokemon.Nickname;

            var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
            {
                RequestType = RequestType.NicknamePokemon,
                RequestMessage = new NicknamePokemonMessage
                {
                    Nickname = newNickname,
                    PokemonId = pokemon.Id
                }.ToByteString()
            }, true);

            NicknamePokemonResponse nicknamePokemonResponse = null;

            nicknamePokemonResponse = NicknamePokemonResponse.Parser.ParseFrom(response);

            if (nicknamePokemonResponse.Result == NicknamePokemonResponse.Types.Result.Success)
            {
                pokemon.Nickname = newNickname;

                session.EventDispatcher.Send(new RenamePokemonEvent
                {
                    Id = pokemon.Id,
                    PokemonId = pokemon.PokemonId,
                    OldNickname = oldNickname,
                    NewNickname = newNickname
                });
            }
        }
    }
}