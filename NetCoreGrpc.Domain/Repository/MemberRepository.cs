
namespace NetCoreGrpc.Domain.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using NetCoreGrpc.Model;

    /// <summary>
    /// 會員持久層
    /// </summary>
    public class MemberRepository : IMemberRepository
    {
        private const string dbName = "NetCoreGrpc";

        private const string collectionName = "Member";

        private MongoClient client { get; set; }
        private IMongoDatabase db { get; set; }
        private IMongoCollection<Member> collection { get; set; }

        static MemberRepository()
        {
            BsonClassMap.RegisterClassMap<Member>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(p => p.MemberId);
            });
        }

        public MemberRepository(MongoClient mongoClient)
        {
            this.client = mongoClient;
            this.db = this.client.GetDatabase(dbName);
            this.collection = this.db.GetCollection<Member>(collectionName);
        }

        /// <summary>
        /// 取得所有資料
        /// </summary>
        /// <returns></returns>
        public (Exception exception, IEnumerable<Member> members) GetAll()
        {
            try
            {
                var filter = Builders<Member>.Filter.Empty;
                var result = this.collection.Find(filter).ToList();
                return (null, result);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        /// <summary>
        /// 創建資料
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public Exception Insert(Member member)
        {
            try
            {
                this.collection.InsertOne(member);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
