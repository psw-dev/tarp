using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSW.ITMS.Api.ApiCommand;
using PSW.ITMS.Common.Pagination;
using PSW.ITMS.Data;
using PSW.ITMS.Service;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.Strategies;
using System;
using System.Security.Cryptography;
using System.Text.Json;

namespace psw.itms.api.Controllers
{
    public class BaseController : Controller
    {
        public IService service { get; set; }
        public IService openService { get; set; }

        public BaseController(IItmsService service, IItmsOpenService openService, IUnitOfWork uow)
        {
            // Dependency Injection of services
            this.service = service;
            this.service.UnitOfWork = uow;
            this.service.StrategyFactory = new StrategyFactory(uow);

            this.openService = openService;
            this.openService.UnitOfWork = uow;
            this.openService.StrategyFactory = new StrategyFactory(uow);
        }

        #region End Points 

        // TODO: Authentication
        // Assuming Request is Authenticated.
        [HttpPost("secure")]
        [Authorize("authorizedUserPolicy")]
        public virtual ActionResult<APIResponse> SecureRequest(APIRequest apiRequest)
        {
            //TODO: Client Id Authentication here
            APIResponse apiResponse = new APIResponse()
            {
                methodId = apiRequest.methodId,
                message = new ResponseMessage()
                {
                    code = "404",
                    description = "Action not found."
                },
                //TODO: Add error object if required
                error = new ErrorModel(),
                //TODO: Add pagination if required
                pagination = new ServerPaginationModel()
            };

            try
            {
                //TODO: Resourse Authorization (Middleware)
                //TODO: Pass User Detials and Method ID to Middleware for Action/Method/Resourse Authorization
                // Assuming Request is Authenticated 
                bool authenticated = true;
                if (authenticated)
                {
                    // Crate JsonElement for service
                    // Calling Service 
                    CommandReply commandReply = service.invokeMethod(
                       new CommandRequest()
                       {
                           methodId = apiRequest.methodId,
                           data = apiRequest.data,
                           pagination = apiRequest.pagination
                       }
                   );

                    // Preparing Resposnse 
                    apiResponse = APIResponseByCommand(commandReply, apiResponse);

                }
                else
                {
                    apiResponse.message.code = "404";
                    apiResponse.message.description = "Not Authorized";
                }

            }
            catch (Exception ex)
            {
                // Log 
                //  _logger.Log(ex.Message);
                // // Prepare Appropriate Response 
                apiResponse.message.code = "404";
                apiResponse.message.description = "Error : " + ex.Message;
            }
            // catch (ServiceException ex)
            // {
            //     // Log 
            //     //  _logger.Log(ex.Message);
            //     // // Prepare Appropriate Response 
            //     apiResponse.message.code = "404";
            //     apiResponse.message.message = "Error : " + ex.Message;
            //     // throw;
            // }

            // Log 
            // _logger.Log("Sending This Response");

            // Send Response 
            return apiResponse;
        }

        [HttpPost("open")]
        public virtual ActionResult<object> OpenRequest(APIRequest apiRequest)
        {
            APIResponse apiResponse = new APIResponse()
            {
                methodId = apiRequest.methodId,
                message = new ResponseMessage()
                // requestStartTime = DateTime.Now 
            };

            try
            {
                //TODO: Resourse Authorization (Middleware)
                //TODO: Pass User Detials and Method ID to Middleware for Action/Method/Resourse Authorization
                // Assuming Request is Authenticated 



                // Calling Service 
                CommandReply commandReply = openService.invokeMethod(
                    new CommandRequest()
                    {
                        methodId = apiRequest.methodId,
                        data = apiRequest.data
                    }
                );

                // Write a function to do this 
                // Preparing Resposnse 
                apiResponse = APIResponseByCommand(commandReply, apiResponse);





            }
            catch (Exception ex)
            {
                // Log 
                //  _logger.Log(ex.Message);
                // // Prepare Appropriate Response 
                apiResponse.message.code = "404";
                apiResponse.message.description = "Error : " + ex.Message;
            }
            // catch (ServiceException ex)
            // {
            //     // Log 
            //     //  _logger.Log(ex.Message);
            //     // // Prepare Appropriate Response 
            //     apiResponse.message.code = "404";
            //     apiResponse.message.message = "Error : " + ex.Message;
            //     // throw;
            // }

            // Log 
            // _logger.Log("Sending This Response");

            // Send Response 
            return apiResponse;
        }

        [HttpGet("query/{methodId}")]
        public virtual ActionResult<object> OpenRequest(string methodId, string username, int duration)
        {
            //TODO: Client Id Authentication here
            APIResponse apiResponse = new APIResponse()
            {
                methodId = methodId,
                message = new ResponseMessage()
                {
                    code = "404",
                    description = "Action not found."
                },
                //TODO: Add error object if required
                error = null,
                //TODO: Add pagination if required
                pagination = null
            };

            try
            {
                //TODO: Resourse Authorization (Middleware)
                //TODO: Pass User Detials and Method ID to Middleware for Action/Method/Resourse Authorization
                // Assuming Request is Authenticated 
                bool authenticated = true;
                if (authenticated)
                {
                    // Crate JsonElement for service
                    string json = "{" + string.Format(@"""username"":""{0}"",""duration"":{1}", username, duration) + "}";
                    JsonElement element = JsonDocument.Parse(json).RootElement;
                    // Calling Service 
                    CommandReply commandReply = openService.invokeMethod(
                       new CommandRequest()
                       {
                           methodId = methodId,
                           data = element
                       }
                   );

                    // Write a function to do this 
                    // Preparing Resposnse 
                    apiResponse.data = commandReply.data;
                    apiResponse.message.code = commandReply.code;
                    apiResponse.message.description = commandReply.message;
                    //   -- Sign Data 
                    string signature = Sign(commandReply.data);
                    apiResponse.signature = signature;



                }
                else
                {
                    apiResponse.message.code = "404";
                    apiResponse.message.description = "Not Authorized";
                }

            }
            catch (Exception ex)
            {
                // Log 
                //  _logger.Log(ex.Message);
                // // Prepare Appropriate Response 
                apiResponse.message.code = "404";
                apiResponse.message.description = "Error : " + ex.Message;
            }
            // catch (ServiceException ex)
            // {
            //     // Log 
            //     //  _logger.Log(ex.Message);
            //     // // Prepare Appropriate Response 
            //     apiResponse.message.code = "404";
            //     apiResponse.message.message = "Error : " + ex.Message;
            //     // throw;
            // }

            // Log 
            // _logger.Log("Sending This Response");

            // Send Response 
            return apiResponse;
        }

        [HttpGet("version")]
        public virtual ActionResult<object> GetVersion()
        {
            return "20210208_081653";
        }

        #endregion

        #region Helper Methods 

        protected string Sign(JsonElement data)
        {
            // TODO Call Sign API to Get Signature  
            using (HashAlgorithm algorithm = SHA256.Create())
                return System.Convert.ToBase64String(algorithm.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(data.ToString())));
        }

        protected APIResponse APIResponseByCommand(CommandReply commandReply, APIResponse apiResponse)
        {
            try
            {
                apiResponse.data = commandReply.data; // TODO: Safe Check on Data
                string signature = Sign(commandReply.data); // TODO: Safe Check Data
                apiResponse.signature = signature; // Sign Data 

                apiResponse.message.code = string.IsNullOrEmpty(commandReply.code) ? "400" : commandReply.code;
                apiResponse.message.description = string.IsNullOrEmpty(commandReply.message) ? "Bad Request" : commandReply.message;

                if (apiResponse.error != null)
                {
                    apiResponse.error.exception = string.IsNullOrEmpty(commandReply.exception) ? "" : commandReply.exception;
                    apiResponse.error.shortDescription = string.IsNullOrEmpty(commandReply.shortDescription) ? "" : commandReply.shortDescription;
                    apiResponse.error.fullDescription = string.IsNullOrEmpty(commandReply.fullDescription) ? "" : commandReply.fullDescription;
                }
                if (apiResponse.pagination != null)
                {
                    apiResponse.pagination = commandReply.pagination;
                }


            }
            catch (System.Exception ex)
            {
                apiResponse.message.code = "400";
                apiResponse.message.description = "Cannot Prepare Response";
                apiResponse.error.exception = "Exception : " + ex.ToString();
            }

            return apiResponse;

        }
        #endregion
    }
}
