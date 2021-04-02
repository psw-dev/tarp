using System.Text.Json;
using PSW.ITMS.Data;
using AutoMapper;
using System.Collections.Generic;
using System.Security.Claims;
using PSW.ITMS.Common.Pagination;

namespace PSW.ITMS.Service.Command
{
    public class CommandRequest
    {
        public JsonElement data { get; set; }
        public string methodId { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        public IMapper _mapper { get; set; }
        public IEnumerable<Claim> UserClaims { get; set; }
        public ServerPaginationModel pagination { get; set; }
    }
}