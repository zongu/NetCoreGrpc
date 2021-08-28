
namespace NetCoreGrpc.Server.Model
{
    using Grpc.Core;
    using NetCoreGrpc.Service;

    public interface IClientShip
    {
        void Boradcast(object action);

        bool TryAdd(string id, IServerStreamWriter<ActionModel> caller);

        void TryRemove(string id);
    }
}
