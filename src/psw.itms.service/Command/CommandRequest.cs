using AutoMapper;
using PSW.ITMS.Data;
using PSW.ITMS.Common.Pagination;
using System.Text.Json;
using PSW.Common.Crypto;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using StackExchange.Redis;

namespace PSW.ITMS.Service.Command
{
    public class CommandRequest
    {
        public JsonElement data { get; set; }
        public string methodId { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        public IMapper _mapper { get; set; }
        public ICryptoAlgorithm CryptoAlgorithm { get; set; }
        public IEnumerable<Claim> UserClaims { get; set; }
        public ClaimsPrincipal CurrentUser { get; set; }
        public ServerPaginationModel pagination { get; set; }
        public IConnectionMultiplexer RedisConnection { get; set; }

        public string SubscriptionTypeCode
        {
            get
            {
                return UserClaims?.First(claim => claim.Type == "subscriptionTypeCode").Value;
            }
        }

        // public int ParentUserRoleID
        // {
        //     get
        //     {
        //         return UserClaims?.First(claim => claim.Type == "parentUserRoleID").Value.ToIntOrDefault() ?? 0;
        //     }
        // }
    }
}