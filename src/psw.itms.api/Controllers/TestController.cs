using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using PSW.ITMS.Api.ApiCommand;
using PSW.ITMS.Service;
using PSW.ITMS.Service.Strategies;
using PSW.ITMS.Service.Exceptions;
using PSW.ITMS.Data;
using PSW.ITMS.Service.Command;
using System.Security.Cryptography;

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