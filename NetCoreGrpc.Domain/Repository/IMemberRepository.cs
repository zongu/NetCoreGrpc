
namespace NetCoreGrpc.Domain.Repository
{
    using System;
    using System.Collections.Generic;
    using NetCoreGrpc.Model;

    /// <summary>
    /// 會員持久層
    /// </summary>
    public interface IMemberRepository
    {
        /// <summary>
        /// 取得所有資料
        /// </summary>
        /// <returns></returns>
        (Exception exception, IEnumerable<Member> members) GetAll();

        /// <summary>
        /// 創建資料
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        Exception Insert(Member member);
    }
}
