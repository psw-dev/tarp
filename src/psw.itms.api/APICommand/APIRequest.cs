using PSW.ITMS.Common.Pagination;
using System.Text.Json;

namespace PSW.ITMS.Api.ApiCommand
{
    public class APIRequest
    {
        public string methodId { get; set; }
        public JsonElement data { get; set; }
        public string signature { get; set; }
        public ServerPaginationModel pagination { get; set; }

    }
}