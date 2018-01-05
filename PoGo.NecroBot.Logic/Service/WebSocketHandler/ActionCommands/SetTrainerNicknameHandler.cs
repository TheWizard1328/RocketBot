using System.Threading.Tasks;
using Google.Protobuf;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Model;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;
using SuperSocket.WebSocket;

namespace PoGo.NecroBot.Logic.Service.WebSocketHandler.ActionCommands
{
    public class SetTrainerNicknameHandler : IWebSocketRequestHandler
    {
        public string Command { get; private set; }

        public SetTrainerNicknameHandler()
        {
            Command = "SetNickname";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            string nickname = message.Data;

            if (nickname.Length > 15)
            {
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = "You selected too long Desired name, max length: 15!"
                });
                return;
            }
            if (nickname == session.Profile.PlayerData.Username) return;


            using (var blocker = new BlockableScope(session, BotActions.UpdateProfile))
            {
                if (!await blocker.WaitToRun()) return;

                var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
                {
                    RequestType = RequestType.ClaimCodename,
                    RequestMessage = new ClaimCodenameMessage
                    {
                       Codename = nickname,
                       //Force = true
                    }.ToByteString()
                }, true);

                ClaimCodenameResponse claimCodenameResponse = null;

                claimCodenameResponse = ClaimCodenameResponse.Parser.ParseFrom(response);

                if (claimCodenameResponse.Status == ClaimCodenameResponse.Types.Status.Success)
                {
                    session.EventDispatcher.Send(new NoticeEvent()
                    {
                        Message = $"Your name is now: {claimCodenameResponse.Codename}"
                    });

                    session.EventDispatcher.Send(new NicknameUpdateEvent()
                    {
                        Nickname = claimCodenameResponse.Codename
                    });
                }
                else
                {
                    session.EventDispatcher.Send(new NoticeEvent()
                    {
                        Message = $"Couldn't change your nickname"
                    });
                }
            }
        }
    }
}