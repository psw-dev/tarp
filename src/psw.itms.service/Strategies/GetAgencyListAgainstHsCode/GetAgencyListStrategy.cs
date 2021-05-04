using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data.Entities;
using System;
using AutoMapper;
using PSW.ITMS.Service.Mapper;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace PSW.ITMS.Service.Strategies
{
    public class GetAgencyListStrategy : ApiStrategy<GetListOfAgencyAgainstHscodeRequest, GetListOfAgencyAgainstHscodeResponse>
    {
        #region Constructors 
        public GetAgencyListStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetAgencyListStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try 
            {
                
                // Send Command Reply 
                return OKReply();
            }
            catch (Exception ex)
            {
                return InternalServerErrorReply(ex);
            }
        }
        
        #endregion 
    }
}
