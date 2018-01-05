using Google.Protobuf;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Inventory.Item;
using POGOProtos.Map.Fort;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;
using System;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class UseFortItemsTask
    {
        public static async Task Execute(ISession session, FortData fort, ItemData item)
        {
            var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
            {
                RequestType = RequestType.AddFortModifier,
                RequestMessage = new AddFortModifierMessage
                {
                    FortId = fort.Id,
                    ModifierType = item.ItemId,
                    PlayerLatitude = session.Client.ClientSession.Player.Latitude,
                    PlayerLongitude = session.Client.ClientSession.Player.Longitude
                }.ToByteString()
            }, true);

            AddFortModifierResponse addFortModifierResponse = null;

            addFortModifierResponse = AddFortModifierResponse.Parser.ParseFrom(response);

            switch (addFortModifierResponse.Result)
            {
                case AddFortModifierResponse.Types.Result.Success:
                    Logger.Write($"{item.ItemId} is valid until: {DateTime.Now.AddMinutes(30)}");
                    break;
                case AddFortModifierResponse.Types.Result.FortAlreadyHasModifier:
                    Logger.Write($"An {item.ItemId} is already active!", LogLevel.Warning);
                    break;
                case AddFortModifierResponse.Types.Result.NoItemInInventory:
                    Logger.Write($"{item.ItemId} no found!", LogLevel.Error);
                    break;
                case AddFortModifierResponse.Types.Result.NoResultSet:
                    Logger.Write($"{item.ItemId} no result set!", LogLevel.Error);
                    break;
                case AddFortModifierResponse.Types.Result.PoiInaccessible:
                    Logger.Write($"Pokestop poi inaccessible!", LogLevel.Error);
                    break;
                case AddFortModifierResponse.Types.Result.TooFarAway:
                    Logger.Write($"Pokestop too far away!", LogLevel.Error);
                    break;
                default:
                    Logger.Write($"Failed to use an {item.ItemId}!", LogLevel.Error);
                    break;
            }
        }
    }
}
