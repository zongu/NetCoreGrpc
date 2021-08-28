
namespace NetCoreGrpc.Server.Model
{
    using NetCoreGrpc.Service;

    public interface IActionHandler
    {
        bool Execute(ActionModel actionModel);
    }
}
