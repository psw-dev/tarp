
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data.Objects.Views;
using System;
using AutoMapper;
using PSW.ITMS.Service.Mapper;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace PSW.ITMS.Service.Strategies
{
    public class GetHSCodeRequirementStrategy : ApiStrategy<GetHSCodeRequirementsRequestDTO, List<GetHSCodeRequirementsResponseDTO>>
    {

        #region Properties 
        #endregion 

        #region Constructors 
        public GetHSCodeRequirementStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetHSCodeRequirementStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  
        public override CommandReply Execute()
        {
            try{
               // Map DTO To Entity   

                // Query Database  
                var HSCodeRequirements = GetHSCodeRequirements(RequestDTO.AgencyID,RequestDTO.HSCode);

                if(!HSCodeRequirements.Any())
                    return NotFoundReply();

                ResponseDTO = HSCodeRequirements.Select(item => this.Command._mapper.Map<GetHSCodeRequirementsResponseDTO>(item)).ToList();
    

                //ResponseDTO = HSCodeRequirements.Select(item => item.HSCode).ToList();

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

        public IList<UV_DocumentaryRequirement> GetHSCodeRequirements(int agencyId,string hsCode)
        {
            try
            {
                // Begin Transaction  
                this.Command.UnitOfWork.BeginTransaction();

                // Query Database 
                // IList<string> HSCodeRequirements =
                //     this.Command.UnitOfWork.HSCodeTARPRepository.GetHSCode(hsCode);
                IList<UV_DocumentaryRequirement> HSCodeRequirements =this.Command.UnitOfWork.UV_DocumentaryRequirementRepository.Where(new {
                    AgencyID= agencyId,
                    HSCode=hsCode
                });


                // Commit Transaction  
                this.Command.UnitOfWork.Commit();

                return HSCodeRequirements;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        

        #endregion 

    }
}
