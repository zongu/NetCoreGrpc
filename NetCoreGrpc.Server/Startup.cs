

namespace NetCoreGrpc.Server
{
    using System.Reflection;
    using Autofac;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NetCoreGrpc.Domain.Repository;
    using NetCoreGrpc.Server.Applibs;
    using NetCoreGrpc.Server.Model;
    using NetCoreGrpc.Server.Services;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                // 繫結gRPC service
                endpoints.MapGrpcService<MemberCommand>();
                endpoints.MapGrpcService<BidirectionalCommand>();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var asm = Assembly.GetExecutingAssembly();

            // 指定處理client指令的handler
            builder.RegisterAssemblyTypes(asm)
                .Where(t => t.IsAssignableTo<IActionHandler>())
                .Named<IActionHandler>(t => t.Name.Replace("Handler", string.Empty).ToLower())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            builder.RegisterType<MemberRepository>()
                .WithParameter("mongoClient", NoSqlService.MongoConnetion)
                .As<IMemberRepository>()
                .SingleInstance();

            builder.RegisterType<ClientShip>()
                .As<IClientShip>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();
        }
    }
}
