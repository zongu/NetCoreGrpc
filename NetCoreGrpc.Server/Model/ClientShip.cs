
namespace NetCoreGrpc.Server.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Grpc.Core;
    using Microsoft.Extensions.Logging;
    using NetCoreGrpc.Service;

    public class ClientShip : IClientShip
    {
        private object _lck;

        private Dictionary<string, IServerStreamWriter<ActionModel>> dic;

        private ILogger<ClientShip> logger;

        public ClientShip(ILogger<ClientShip> logger)
        {
            this._lck = new object();
            this.dic = new Dictionary<string, IServerStreamWriter<ActionModel>>();
            this.logger = logger;
        }

        public void Boradcast(object action)
        {
            lock (this._lck)
            {
                var tasks = this.dic.Select(obj =>
                {
                    return Task.Run(() =>
                    {
                        try
                        {
                            obj.Value.WriteAsync(new ActionModel()
                            {
                                Action = action.GetType().Name,
                                Content = JsonSerializer.Serialize(action)
                            });
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, $"{this.GetType().Name} Broadcast exception id:{obj.Key}, content:{JsonSerializer.Serialize(action)}");
                        }
                    });
                }).ToArray();

                Task.WaitAll(tasks);
            }
        }

        public bool TryAdd(string id, IServerStreamWriter<ActionModel> caller)
        {
            lock (this._lck)
            {
                return this.dic.TryAdd(id, caller);
            }
        }

        public void TryRemove(string id)
        {
            lock (this._lck)
            {
                this.dic.Remove(id);
            }
        }
    }
}
