


using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using PSW.ITMS.Data.Objects.Views;


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
    public class GetRequiredDocumentTypeStrategy : ApiStrategy<GetRequiredDocumentTypeRequestDTO, List<GetRequiredDocumentTypeResponseDTO>>
    {

        #region Properties 
        #endregion 

        #region Constructors 
        public GetRequiredDocumentTypeStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetRequiredDocumentTypeStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  
        public override CommandReply Execute()
        {
            try{
               // Map DTO To Entity   

                // Query Database  
                //Get applicable Rules                
                var HSCodeRequirements = GetHSCodeRequirements(RequestDTO.AgencyId,RequestDTO.HSCode);
                if(!HSCodeRequirements.Any())
                    return NotFoundReply();


//List<GetRequiredDocumentTypeResponseDTO> GetDocuments(IList<UV_DocumentaryRequirement> rules, int agencyId)
                ResponseDTO = GetDocuments(HSCodeRequirements, RequestDTO.AgencyId,RequestDTO.ImportPurposeID.ToString());

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
       
        public List<GetRequiredDocumentTypeResponseDTO> GetDocuments(IList<UV_DocumentaryRequirement> rules, int agencyId, string importPurposeId){
            IEnumerable<DocumentType> documentList= GetAllDocumentType(agencyId);
            List<GetRequiredDocumentTypeResponseDTO> Documents=new List<GetRequiredDocumentTypeResponseDTO>();
 
                foreach(UV_DocumentaryRequirement record in rules){
                    if(!Documents.Any(p=>p.DocumentCode==record.RequiredDocumentTypeCode)){
                        Documents.Add(searchDocumentCode(documentList,record.RequiredDocumentTypeCode));
                    }                                                                                      
                }
            return   Documents;  
        }
         public IEnumerable<DocumentType> GetAllDocumentType(int agencyId)
        {
            try
            {
                //TODO: Query ITMS to get Document Type w.r.t HSCode and Agency Id

                //For The Time being Getting the desire values from shared DB directly
                
                
                // Begin Transaction  
                this.Command.UnitOfWork.BeginTransaction();

                // Query Database 
                IEnumerable<DocumentType> AllDocmumentType =
                    this.Command.UnitOfWork.DocumentTypeRepository.Where(
                        new{
                            AgencyID=agencyId,  
                            //HSCode=HSCode //Will be required when querying tom ITMS                        
                        }
                    );

                // Commit Transaction  
                this.Command.UnitOfWork.Commit();

                return AllDocmumentType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        
        public GetRequiredDocumentTypeResponseDTO searchDocumentCode(IEnumerable<DocumentType> documentList, string documentCode){
             GetRequiredDocumentTypeResponseDTO response=new GetRequiredDocumentTypeResponseDTO();
            var doc=documentList.FirstOrDefault(x => x.Code == documentCode);                       
            if(doc!= null){
                response.DocumentCode=documentCode;
                response.DocumentName=doc.Name;
                
                return response;
            }
            return response;            
        }
       
        #endregion 

    }
}
