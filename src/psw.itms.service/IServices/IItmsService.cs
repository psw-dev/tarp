using System.Security.Claims;
using System.Collections.Generic;

namespace PSW.ITMS.Service
{
    public interface IItmsService : IService
    {
        // CommandReply invokeMethod(CommandRequest request);
        IEnumerable<Claim> UserClaims { get; set; }
    }

}