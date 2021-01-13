using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using PSW.ITMS.Common.Pagination;


namespace PSW.ITMS.Api.ApiCommand
{
    public class APIResponse 
    {
        public string methodId { get; set; }        
        public JsonElement data { get; set; }  
        public string signature { get; set; }
        public ServerPaginationModel pagination { get; set; }
        public ResponseMessage message { get; set; }
        public ErrorModel error { get; set; }

        public APIResponse (){
            
        } 

    }

    public class ErrorModel
    {
        public string exception { set; get; }
        public string shortDescription { get; set; }
        public string fullDescription { get; set; }
    }
    public class ResponseMessage
    {
        public string code { get; set; }
        public string description { set; get; }
      
    }
}