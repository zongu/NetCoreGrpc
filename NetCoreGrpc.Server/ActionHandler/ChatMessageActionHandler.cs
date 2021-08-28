
namespace NetCoreGrpc.Server.ActionHandler
{
    using System;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using NetCoreGrpc.Action;
    using NetCoreGrpc.Server.Model;
    using NetCoreGrpc.Service;

    public class ChatMessageActionHandler : IActionHandler
    {
        private ILogger<ChatMessageActionHandler> logger;

        private IClientShip clientShip;

        public ChatMessageActionHandler(ILogger<ChatMessageActionHandler> logger, IClientShip clientShip)
        {
            this.logger = logger;
            this.clientShip = clientShip;
        }

        public bool Execute(ActionModel actionModel)
        {
            try
            {
                var action = JsonSerializer.Deserialize<ChatMessageAction>(actionModel.Content);
                this.clientShip.Boradcast(action);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"{this.GetType().Name} Execute Exception");
                return false;
            }
        }
    }
}
