using System;
using System.Threading.Tasks;
using Google.Protobuf;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class UseIncenseTask
    {
        public static async Task Execute(ISession session)
        {
            var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
            {
                RequestType = RequestType.UseIncense,
                RequestMessage = new UseIncenseMessage
                {
                   IncenseType = ItemId.ItemIncenseOrdinary
                }.ToByteString()
            }, true);

            UseIncenseResponse useIncenseResponse = null;

            useIncenseResponse = UseIncenseResponse.Parser.ParseFrom(response);

            switch (useIncenseResponse.Result)
            {
                case UseIncenseResponse.Types.Result.Success:
                    Logger.Write($"Incense is Valid until: {DateTime.Now.AddMinutes(30)}");
                    break;
                case UseIncenseResponse.Types.Result.IncenseAlreadyActive:
                    Logger.Write($"An incense is already active!", LogLevel.Warning);
                    break;
                case UseIncenseResponse.Types.Result.LocationUnset:
                    Logger.Write($"Bot must be running first!", LogLevel.Error);
                    break;
                case UseIncenseResponse.Types.Result.Unknown:
                    break;
                case UseIncenseResponse.Types.Result.NoneInInventory:
                    break;
                default:
                    Logger.Write($"Failed to use an incense!", LogLevel.Error);
                    break;
            }
        }
    }
}