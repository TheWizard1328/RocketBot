using Google.Protobuf;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Data;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class UseRareCandyTask
    {
        public static async Task Execute(ISession session, ItemData item, PokemonData pokemondata)
        {
            int candy = await session.Inventory.GetCandyCount(pokemondata.PokemonId).ConfigureAwait(false);

            var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
            {
                RequestType = RequestType.UseItemRareCandy,
                RequestMessage = new UseItemRareCandyMessage
                {
                    ItemId = item.ItemId,
                    PokemonId = pokemondata.PokemonId
                }.ToByteString()
            }, true);

            UseItemRareCandyResponse useItemRareCandyResponse = null;

            useItemRareCandyResponse = UseItemRareCandyResponse.Parser.ParseFrom(response);

            switch (useItemRareCandyResponse.Result)
            {
                case UseItemRareCandyResponse.Types.Result.Success:
                    Logger.Write($"Success {candy} ===> {candy + 1}", LogLevel.Info);
                    break;
                case UseItemRareCandyResponse.Types.Result.InvalidPokemonId:
                    Logger.Write($"Failed Invalid Pokemon!", LogLevel.Error);
                    break;
                case UseItemRareCandyResponse.Types.Result.ItemNotInInventory:
                    Logger.Write($"Error Item Not In Inventory!", LogLevel.Error);
                    break;
                case UseItemRareCandyResponse.Types.Result.NoPlayer:
                    Logger.Write($"No Player!", LogLevel.Error);
                    break;
                case UseItemRareCandyResponse.Types.Result.WrongItemType:
                    Logger.Write($"Wrong Item Type!", LogLevel.Error);
                    break;
                case UseItemRareCandyResponse.Types.Result.Unset:
                    Logger.Write($"Unset!", LogLevel.Warning);
                    break;
                default:
                    Logger.Write($"Failed to use {item.ItemId}!", LogLevel.Warning);
                    break;
            }
        }
    }
}
