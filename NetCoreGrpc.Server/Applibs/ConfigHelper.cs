﻿
namespace NetCoreGrpc.Server.Applibs
{
    using System.IO;
    using Microsoft.Extensions.Configuration;

    internal static class ConfigHelper
    {
        private static IConfiguration _config;

        private static IConfiguration config
        {
            get
            {
                if(_config == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();

                    _config = builder.Build();
                }

                return _config;
            }
        }

        /// <summary>
        /// 服務位址 (gRPC要有TLS)
        /// </summary>
        public static readonly string ServiceUrl = @"https://*:8085";

        /// <summary>
        /// mongodb連線字串
        /// </summary>
        public static readonly string MongoDBConn = config["MongoDBConn"];
    }
}
