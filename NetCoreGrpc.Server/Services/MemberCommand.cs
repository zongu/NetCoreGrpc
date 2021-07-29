
namespace NetCoreGrpc.Server.Services
{
    using System;
    using System.Threading.Tasks;
    using Grpc.Core;
    using Microsoft.Extensions.Logging;
    using NetCoreGrpc.Domain.Repository;
    using NetCoreGrpc.Model;
    using NetCoreGrpc.Service;

    /// <summary>
    /// 會員相關指令
    /// </summary>
    public class MemberCommand : MemberService.MemberServiceBase
    {
        private ILogger<MemberCommand> logger;

        private IMemberRepository repo;

        public MemberCommand(ILogger<MemberCommand> logger, IMemberRepository repo)
        {
            this.logger = logger;
            this.repo = repo;
        }

        /// <summary>
        /// 取得所有資料
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
        {
            this.logger.LogTrace($"{this.GetType().Name} GetAll");

            try
            {
                var getResult = this.repo.GetAll();

                if (getResult.exception != null)
                {
                    throw getResult.exception;
                }

                var result = new GetAllResponse();
                result.Members.Add(getResult.members);

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"{this.GetType().Name} GetAll Exception");
                throw;
            }
        }

        /// <summary>
        /// 建立資料
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<Member> Insert(Member request, ServerCallContext context)
        {
            this.logger.LogTrace($"{this.GetType().Name} Insert");

            try
            {
                var insertResult = this.repo.Insert(request);

                if(insertResult != null)
                {
                    throw new Exception();
                }

                return Task.FromResult(request);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"{this.GetType().Name} Insert Exception");
                throw;
            }
        }
    }
}
