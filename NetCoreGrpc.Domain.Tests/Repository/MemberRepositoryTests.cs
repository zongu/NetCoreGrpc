
namespace NetCoreGrpc.Domain.Tests.Repository
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MongoDB.Driver;
    using NetCoreGrpc.Domain.Model;
    using NetCoreGrpc.Domain.Repository;
    using NetCoreGrpc.Model;

    [TestClass]
    public class MemberRepositoryTests
    {
        private IMemberRepository repo;

        [TestInitialize]
        public void Init()
        {
            var client = new MongoClient(@"mongodb://localhost:27017");
            var db = client.GetDatabase("NetCoreGrpc");
            db.DropCollection("Member");

            this.repo = new MemberRepository(client);
        }

        [TestMethod]
        public void 新建資料測試()
        {
            var result = this.repo.Insert(new Member()
            {
                MemberId = "TEST001",
                NickName = "TEST001",
                CreateDateTimeStamp = TimeStampHelper.ToUtcTimeStamp(DateTime.Now)
            });

            Assert.IsNull(result);
        }

        [TestMethod]
        public void 取得資料測試()
        {
            var insertResult = this.repo.Insert(new Member()
            {
                MemberId = "TEST001",
                NickName = "TEST001",
                CreateDateTimeStamp = TimeStampHelper.ToUtcTimeStamp(DateTime.Now)
            });

            Assert.IsNull(insertResult);

            var getResult = this.repo.GetAll();
            Assert.IsNull(getResult.exception);
            Assert.IsNotNull(getResult.members);
            Assert.AreEqual(getResult.members.Count(), 1);
        }
    }
}
