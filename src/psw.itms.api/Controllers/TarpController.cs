using Microsoft.AspNetCore.Mvc;
using PSW.ITMS.Api.ApiCommand;
using PSW.ITMS.Service;
using PSW.ITMS.Service.Strategies;
using PSW.ITMS.Data;
using Microsoft.AspNetCore.Http;
using psw.itms.api.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace PSW.ITMS.Api.Contollers
{

    [Route("api/v1/tarp/[controller]")]
    [ApiController]
    public class TarpController : BaseController
    {

        #region Properties
        private IItmsService service { get; set; }
       
        #endregion

        #region Constructors

        public TarpController(IItmsService service, IItmsOpenService openService, IUnitOfWork uow, IHttpContextAccessor httpContextAccessor) : base(service, openService, uow)
        {
            // Dependency Injection of services
            this.service = service;
            this.service.UnitOfWork = uow;
            this.service.StrategyFactory = new StrategyFactory(uow);
            this.service.UserClaims = httpContextAccessor.HttpContext.User.Claims;
        }

        #endregion

        #region End Points 

        [HttpPost("secure")]
        [Authorize("authorizedUserPolicy")]
        public override ActionResult<APIResponse> SecureRequest(APIRequest apiRequest)
        {
            return base.SecureRequest(apiRequest);
        }

        [HttpPost("open")]
        public override ActionResult<object> OpenRequest(APIRequest apiRequest)
        {
            return base.OpenRequest(apiRequest);
        }

        [HttpGet("query/{methodId}")]
        public override ActionResult<object> OpenRequest(string methodId, string username, int duration)
        {
            return base.OpenRequest(methodId, username, duration);
        }

        [HttpGet("version")]
        public override ActionResult<object> GetVersion()
        {
            return "20210219_144630";
        }

        [HttpGet("env")]
        public ActionResult<object> GetEnv()
        {
            return System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }

        #endregion
    }
} 