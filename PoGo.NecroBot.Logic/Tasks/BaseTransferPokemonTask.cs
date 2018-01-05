using Google.Protobuf;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Data;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public abstract class BaseTransferPokemonTask
    {
        public static async Task Execute(ISession session, IEnumerable<PokemonData> pokemonsToTransfer, CancellationToken cancellationToken)
        {
            if (pokemonsToTransfer.Count() > 0)
            {
                if (session.LogicSettings.UseBulkTransferPokemon)
                {
                    int page = pokemonsToTransfer.Count() / session.LogicSettings.BulkTransferSize + 1;
                    for (int i = 0; i < page; i++)
                    {
                        TinyIoC.TinyIoCContainer.Current.Resolve<MultiAccountManager>().ThrowIfSwitchAccountRequested();
                        var batchTransfer = pokemonsToTransfer.Skip(i * session.LogicSettings.BulkTransferSize).Take(session.LogicSettings.BulkTransferSize);
                        var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
                        {
                            RequestType = RequestType.ReleasePokemon,
                            RequestMessage = new ReleasePokemonMessage
                            {
                                PokemonIds = { batchTransfer.Select(x => x.Id).ToList() },
                                //PokemonId = 0
                            }.ToByteString()
                        }, true);

                        ReleasePokemonResponse t = null;

                        t = ReleasePokemonResponse.Parser.ParseFrom(response);

                        if (t.Result == ReleasePokemonResponse.Types.Result.Success)
                        {
                            foreach (var duplicatePokemon in batchTransfer)
                            {
                                await PrintPokemonInfo(session, duplicatePokemon).ConfigureAwait(false);
                            }
                        }
                        else session.EventDispatcher.Send(new WarnEvent() { Message = session.Translation.GetTranslation(TranslationString.BulkTransferFailed, pokemonsToTransfer.Count()) });
                    }
                }
                else
                {
                    foreach (var pokemon in pokemonsToTransfer)
                    {
                        TinyIoC.TinyIoCContainer.Current.Resolve<MultiAccountManager>().ThrowIfSwitchAccountRequested();
                        cancellationToken.ThrowIfCancellationRequested();

                        var response = await session.Client.ClientSession.RpcClient.SendRemoteProcedureCallAsync(new Request
                        {
                            RequestType = RequestType.ReleasePokemon,
                            RequestMessage = new ReleasePokemonMessage
                            {
                                PokemonId = pokemon.Id,
                                //PokemonIds = { }
                            }.ToByteString()
                        }, true);

                        ReleasePokemonResponse releasePokemonResponse = null;

                        releasePokemonResponse = ReleasePokemonResponse.Parser.ParseFrom(response);

                        await PrintPokemonInfo(session, pokemon).ConfigureAwait(false);

                        // Padding the TransferEvent with player-choosen delay before instead of after.
                        // This is to remedy too quick transfers, often happening within a second of the
                        // previous action otherwise

                        await DelayingUtils.DelayAsync(session.LogicSettings.TransferActionDelay, 0, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
        }

        public static async Task PrintPokemonInfo(ISession session, PokemonData duplicatePokemon)
        {
            var bestPokemonOfType = (session.LogicSettings.PrioritizeIvOverCp
                                        ? await session.Inventory.GetHighestPokemonOfTypeByIv(duplicatePokemon).ConfigureAwait(false)
                                        : await session.Inventory.GetHighestPokemonOfTypeByCp(duplicatePokemon).ConfigureAwait(false)) ??
                                    duplicatePokemon;

            var ev = new TransferPokemonEvent
            {
                Id = duplicatePokemon.Id,
                PokemonId = duplicatePokemon.PokemonId,
                Slashed = duplicatePokemon.IsBad,
                Perfection = PokemonInfo.CalculatePokemonPerfection(duplicatePokemon),
                Cp = duplicatePokemon.Cp,
                BestCp = bestPokemonOfType.Cp,
                BestPerfection = PokemonInfo.CalculatePokemonPerfection(bestPokemonOfType),
                Candy = await session.Inventory.GetCandyCount(duplicatePokemon.PokemonId).ConfigureAwait(false),
                Level = PokemonInfo.GetLevel(duplicatePokemon)
            };

            if (await session.Inventory.GetCandyFamily(duplicatePokemon.PokemonId).ConfigureAwait(false) != null)
            {
                ev.FamilyId = (await session.Inventory.GetCandyFamily(duplicatePokemon.PokemonId).ConfigureAwait(false)).FamilyId;
            }

            session.EventDispatcher.Send(ev);
        }
    }
}