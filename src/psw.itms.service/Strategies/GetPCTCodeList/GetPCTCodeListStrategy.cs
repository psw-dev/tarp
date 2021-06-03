using System.Collections.Generic;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using System;

namespace PSW.ITMS.Service.Strategies
{
    public class GetPCTCodeListStrategy : ApiStrategy<GetPCTCodeListRequest, GetPCTCodeListResponse>
    {
        #region Constructors 
        public GetPCTCodeListStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetPCTCodeListStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                if(string.IsNullOrEmpty(RequestDTO.HsCode))
                {
                    return BadRequestReply("Hscode cannot be null");
                }

                List<string> TempPctCodeList = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetPCTCodeList(RequestDTO.HsCode);

                if(TempPctCodeList == null || TempPctCodeList.Count == 0)
                {
                    ResponseDTO = new GetPCTCodeListResponse 
                    {
                        PctCodeList = new List<string>(),
                        Message = "Product codes does not exist for the provided hscode."
                    };

                    return OKReply();
                }

                ResponseDTO = new GetPCTCodeListResponse
                {
                    Message = "Product codes exist for provided hscode.",
                    PctCodeList = TempPctCodeList
                };
                
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
