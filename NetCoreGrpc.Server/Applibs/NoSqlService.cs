
namespace NetCoreGrpc.Server.Applibs
{
    using System;
    using MongoDB.Driver;

    internal static class NoSqlService
    {
        private static Lazy<MongoClient> lazyMongoConnetion;

        public static MongoClient MongoConnetion
        {
            get
            {
                if (lazyMongoConnetion == null)
                {
                    lazyMongoConnetion = new Lazy<MongoClient>(() =>
                    {
                        return new MongoClient(ConfigHelper.MongoDBConn);
                    });
                }

                return lazyMongoConnetion.Value;
            }
        }
    }
}
