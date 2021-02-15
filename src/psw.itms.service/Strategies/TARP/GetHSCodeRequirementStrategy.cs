
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
using PSW.ITMS.Data.Entities;
using psw.security.Encryption;

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
                 
                // Retreive All required Data from DB
                IEnumerable<TradePurpose> tradePurposes = GetAllTradePurposes(RequestDTO.AgencyID);                
                if(!tradePurposes.Any())
                    return NotFoundReply("Trade Purpose not Found");


                IEnumerable<DocumentType> documentList = GetAllDocumentType(RequestDTO.AgencyID);
                if(!documentList.Any())
                    return NotFoundReply("Documents not Found");


                IEnumerable<UoM> UoMList = GetUoMs();
                if(!UoMList.Any())
                    return NotFoundReply("UOMs not found");
                

                //Get applicable Rules                
                var HSCodeRequirements = GetHSCodeRequirements(RequestDTO.AgencyID, RequestDTO.HSCode);
                if(!HSCodeRequirements.Any())
                    return NotFoundReply();
                
                //Create Response               
                ResponseDTO=CreateResponse(HSCodeRequirements,documentList,UoMList,tradePurposes);


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
        public List<GetHSCodeRequirementsResponseDTO>  CreateResponse(IList<UV_DocumentaryRequirement> HSCodeRequirements,IEnumerable<DocumentType> documentList,IEnumerable<UoM> UoMList,IEnumerable<TradePurpose> tradePurposes){
            List<GetHSCodeRequirementsResponseDTO> testResp=new List<GetHSCodeRequirementsResponseDTO>();

                //Testing Respose
                foreach(UV_DocumentaryRequirement record in HSCodeRequirements){
                    GetHSCodeRequirementsResponseDTO test= new GetHSCodeRequirementsResponseDTO();
                    test= this.Command._mapper.Map<GetHSCodeRequirementsResponseDTO>(record);
                    
                    test.UoM=searchUoM(UoMList,record.UoMID);                    
                    test.RequiredDocument=searchDocumentCode(documentList,record.RequiredDocumentTypeCode);  
                    test.RequestedDocument=searchDocumentCode(documentList,record.RequestedDocument);
                    
                    List<GetPurposeOfImportByHSCodeResponseDTO> validTradePurpose=new List<GetPurposeOfImportByHSCodeResponseDTO>();

                    IList<string> PurposeIDList=StringSplitter(record.PurposeIDList);
                    foreach(string purposeID in PurposeIDList){
                        if(!validTradePurpose.Any(p=>p.ID==Convert.ToInt32(purposeID))){                            
                            validTradePurpose.Add(searchImportPurposeID(tradePurposes,Convert.ToInt32(purposeID)));
                        }                        
                    } 
                    
                    test.purposesOfImport=validTradePurpose;
                    test.Amount = PSWEncryption.encrypt("5000");
                    testResp.Add(test);
                } 
                return   testResp;
        }  
        public GetPurposeOfImportByHSCodeResponseDTO searchImportPurposeID(IEnumerable<TradePurpose> importPurposeList, int importPurposeId){
             GetPurposeOfImportByHSCodeResponseDTO response=new GetPurposeOfImportByHSCodeResponseDTO();
            var ImportPurpose=importPurposeList.FirstOrDefault(x => x.ID == importPurposeId);                       
            if(ImportPurpose!= null){
                response.ID=importPurposeId;
                response.Name=ImportPurpose.Name;
                
                return response;
            }
            return response;
        }
        
        public List<string> StringSplitter(string value){
            return value.Split(',').ToList();
        }
        public IEnumerable<TradePurpose> GetAllTradePurposes(int agencyId)
        {
            try
            {
                // Begin Transaction  
                this.Command.UnitOfWork.BeginTransaction();

                // Query Database 
                // IList<string> HSCodeRequirements =
                //     this.Command.UnitOfWork.HSCodeTARPRepository.GetHSCode(hsCode);
                IEnumerable<TradePurpose> tradePurposes =this.Command.UnitOfWork.TradePurposeRepository.Where(new {
                    AgencyID= agencyId
                   
                });


                // Commit Transaction  
                this.Command.UnitOfWork.Commit();

                return tradePurposes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public IList<HSCodeTARP> GetFromHSCodeTARP(int agencyId,string hsCode)
        {
            try
            {
                // Begin Transaction  
                this.Command.UnitOfWork.BeginTransaction();

                // Query Database 
                // IList<string> HSCodeRequirements =
                //     this.Command.UnitOfWork.HSCodeTARPRepository.GetHSCode(hsCode);
                IList<HSCodeTARP> HSCodeTARPList =this.Command.UnitOfWork.HSCodeTARPRepository.Where(new {
                    AgencyID= agencyId,
                    HSCode=hsCode
                   
                });


                // Commit Transaction  
                this.Command.UnitOfWork.Commit();

                return HSCodeTARPList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        

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
                    HSCodeExt=hsCode
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
        
        public DocumentResponseDTO searchDocumentCode(IEnumerable<DocumentType> documentList, string documentCode){
             DocumentResponseDTO response=new DocumentResponseDTO();
            var doc=documentList.FirstOrDefault(x => x.Code == documentCode);                       
            if(doc!= null){
                response.DocumentCode=documentCode;
                response.DocumentName=doc.Name;
                
                return response;
            }
            return response;            
        }
        
        public IEnumerable<UoM> GetUoMs()
        {
            try
            {
                //TODO: Query ITMS to get Document Type w.r.t HSCode and Agency Id

                //For The Time being Getting the desire values from shared DB directly
                
                
                // Begin Transaction  
                this.Command.UnitOfWork.BeginTransaction();

                // Query Database 
                IEnumerable<UoM> UoMs =
                    this.Command.UnitOfWork.UoMRepository.Get();

                // Commit Transaction  
                this.Command.UnitOfWork.Commit();

                return UoMs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        
        public UOMResponseDTO searchUoM(IEnumerable<UoM> uomList, int uomId){
             UOMResponseDTO response=new UOMResponseDTO();
            var uom=uomList.FirstOrDefault(x => x.ID == uomId);                       
            if(uom!= null){
                response.ID=uomId;
                response.Name=uom.Name;
                
                return response;
            }
            return response;            
        }
                                
        #endregion 

    }
}
