
namespace NetCoreGrpc.Server.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Autofac.Features.Indexed;
    using Grpc.Core;
    using Microsoft.Extensions.Logging;
    using NetCoreGrpc.Server.Model;
    using NetCoreGrpc.Service;

    public class BidirectionalCommand : BidirectionalService.BidirectionalServiceBase
    {
        private IClientShip clientShip;

        private ILogger<BidirectionalCommand> logger;

        private IIndex<string, IActionHandler> handlerSets;

        public BidirectionalCommand(IClientShip clientShip, ILogger<BidirectionalCommand> logger, IIndex<string, IActionHandler> handlerSets)
        {
            this.clientShip = clientShip;
            this.logger = logger;
            this.handlerSets = handlerSets;
        }

        public override async Task ActionAsync(
            IAsyncStreamReader<ActionModel> requestStream,
            IServerStreamWriter<ActionModel> responseStream,
            ServerCallContext context)
        {
            var id = context.RequestHeaders.FirstOrDefault(p => p.Key == "id")?.Value ?? Guid.NewGuid().ToString();
            this.clientShip.TryAdd(id, responseStream);

            await Task.Run(async () =>
            {
                try
                {
                    await foreach (var action in requestStream.ReadAllAsync())
                    {
                        if(this.handlerSets.TryGetValue(action.Action.ToLower(), out var handler))
                        {
                            handler.Execute(action);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, $"{this.GetType().Name} ActionAsync Exception");
                    // client 斷線
                    this.clientShip.TryRemove(id);
                }
            });

            // client 結束通訊
            this.clientShip.TryRemove(id);
        }
    }
}
