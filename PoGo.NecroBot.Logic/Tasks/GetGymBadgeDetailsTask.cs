using Google.Protobuf;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Map.Fort;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class GetGymBadgeDetailsTask
    {
        public static async Task Execute(ISession session, FortData fort)
        {
            var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
            {
                RequestType = RequestType.GetGymBadgeDetails,
                RequestMessage = new GetGymBadgeDetailsMessage
                {
                    FortId = fort.Id,
                    Latitude = fort.Latitude,
                    Longitude = fort.Longitude
                }.ToByteString()
            }, true);

            GetGymBadgeDetailsResponse getGymBadgeDetailsResponse = null;

            getGymBadgeDetailsResponse = GetGymBadgeDetailsResponse.Parser.ParseFrom(response);
        }
    }
}
