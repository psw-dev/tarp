using Microsoft.AspNetCore.Mvc;
using PSW.ITMS.Api.ApiCommand;

namespace PSW.ITMS.Api.Contollers
{

    [Route("api/v1/itms/[controller]")]
    [ApiController]
    public class TestController
    {


        [HttpPost("secure")]
        public ActionResult<APIResponse> SecureRequest(APIRequest apiRequest)
        {
            return new APIResponse();
        }


        [HttpPost("open")]
        public ActionResult<object> OpenRequest(APIRequest apiRequest)
        {
            return new APIResponse();
        }


        [HttpPost("test")]
        public string test(APIRequest apiRequest)
        {
            return "Hello";
        }


    }
}