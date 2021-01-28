
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
    public class GetHSCodeListStrategy : ApiStrategy<GetHSCodeListRequestDTO, List<string>>
    {

        #region Properties 
        #endregion 

        #region Constructors 
        public GetHSCodeListStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetHSCodeListStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  
        public override CommandReply Execute()
        {
            try{
               // Map DTO To Entity   

                // Query Database  
                var HSCodeList = GetHSCodeList(RequestDTO.AgencyID);

                if(!HSCodeList.Any())
                    return NotFoundReply();

                ResponseDTO = HSCodeList;

                // Send Command Reply 
                return OKReply();
                
            }
            catch (Exception ex)
            {
                return InternalServerErrorReply(ex);
            }
        }
        #endregion


        #region Methods  

        public List<string> GetHSCodeList(int agencyId)
        {
            try
            {
                // Begin Transaction  
                this.Command.UnitOfWork.BeginTransaction();

                // Query Database 
                // IList<string> HSCodeList =
                //     this.Command.UnitOfWork.HSCodeTARPRepository.GetHSCode(hsCode);
                List<string> HSCodeList =this.Command.UnitOfWork.HSCodeTARPRepository.GetHSCode(new {
                    AgencyID= agencyId
                });


                // Commit Transaction  
                this.Command.UnitOfWork.Commit();

                return HSCodeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        

        #endregion 

    }
}
