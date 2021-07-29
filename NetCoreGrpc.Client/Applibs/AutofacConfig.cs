
namespace NetCoreGrpc.Client.Applibs
{
    using Autofac;
    using NetCoreGrpc.Service;

    public class AutofacConfig
    {
        private static IContainer container;

        public static IContainer Container
        {
            get
            {
                if (container == null)
                {
                    Register();
                }

                return container;
            }
        }

        private static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MemberService.MemberServiceClient>()
                .WithParameter("channel", GrpcChannelService.GrpcChannel)
                .SingleInstance();

            container = builder.Build();
        }
    }
}
