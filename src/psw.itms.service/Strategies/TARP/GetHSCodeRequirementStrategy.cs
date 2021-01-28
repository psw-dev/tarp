
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

namespace PSW.ITMS.Service.Strategies
{
    public class GetHSCodeRequirementStrategy : ApiStrategy<GetHSCodeRequirementsRequestDTO, List<TestDTO>>
    {

        #region Properties 
        #endregion 
//FLOWW
//
//GET ALL FROM HSCODETARP RECORDS
//IF GET COMMA SEPEARTED VALUES, ADD Another instance
// return
//1-List of rules
//2-List of VAlid Purposes (Done)
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

                
                List<GetPurposeOfImportByHSCodeResponseDTO> validTardePurpose=new List<GetPurposeOfImportByHSCodeResponseDTO>();
               
                // Retreive All required Data from DB
                IEnumerable<TradePurpose> tradePurposes= GetAllTradePurposes(RequestDTO.AgencyID);
                IEnumerable<DocumentType> documentList=GetAllDocumentType(RequestDTO.AgencyID);
                IEnumerable<UoM> UoMList=GetUoMs();
                
                //Get applicable Rules
                var HSCodeRequirements = GetHSCodeRequirements(RequestDTO.AgencyID,RequestDTO.HSCode);

                if(!HSCodeRequirements.Any())
                    return NotFoundReply();

                //Creating List for Valid Purposes
                foreach(UV_DocumentaryRequirement record in HSCodeRequirements){
                    IList<string> PuposeIDList=StringSplitter(record.PurposeIDList);
                    foreach(string purposeID in PuposeIDList){
                        if(!validTardePurpose.Any(p=>p.ID==Convert.ToInt32(purposeID))){
                            GetPurposeOfImportByHSCodeResponseDTO purpose=searchImportPurposeID(tradePurposes,Convert.ToInt32(purposeID));
                            GetPurposeOfImportByHSCodeResponseDTO finalPurpose=new GetPurposeOfImportByHSCodeResponseDTO(){
                                    ID=purpose.ID,
                                    Name=purpose.Name
                            };
                            validTardePurpose.Add(finalPurpose);
                        }                        
                    }                   
                }

                //  //Creating List for Documents
                // List<DocumentResponseDTO> Documents=new List<DocumentResponseDTO>();
 
                // foreach(UV_DocumentaryRequirement record in HSCodeRequirements){
                //     if(!Documents.Any(p=>p.DocumentCode==record.RequiredDocumentTypeCode)){
                //         DocumentResponseDTO document=searchDocumentCode(documentList,record.RequiredDocumentTypeCode);
                //            DocumentResponseDTO finaldocument=new DocumentResponseDTO(){
                //                 DocumentCode=document.DocumentCode,
                //                 DocumentName=document.DocumentName
                //         };
                //         Documents.Add(finaldocument);
                //         }
                //     if(!Documents.Any(p=>p.DocumentCode==record.RequestedDocument)){
                //         DocumentResponseDTO document=searchDocumentCode(documentList,record.RequestedDocument);
                //            DocumentResponseDTO finaldocument=new DocumentResponseDTO(){
                //                 DocumentCode=document.DocumentCode,
                //                 DocumentName=document.DocumentName
                //         };
                //         Documents.Add(finaldocument);
                //         }                                                                   
                // }


                
                // List<UOMResponseDTO> UOMs=new List<UOMResponseDTO>();
 
                // foreach(UV_DocumentaryRequirement record in HSCodeRequirements){
                //     if(!UOMs.Any(p=>p.ID==record.UoMID)){
                //         UOMResponseDTO uom=searchUoM(UoMList,record.UoMID);
                //            UOMResponseDTO finalUoM=new UOMResponseDTO(){
                //                 ID=uom.ID,
                //                 Name=uom.Name
                //         };
                //         UOMs.Add(finalUoM);
                //         }                                                                 
                // }

                             
                
                
                //Actual Response
                //IList<ListOfRules> LORs=HSCodeRequirements.Select(item => this.Command._mapper.Map<ListOfRules>(item)).ToList();

                // ResponseDTO=new GetHSCodeRequirementsResponseDTO(){
                //     rules=LORs,
                //     purposesOfImport=validTardePurpose,
                //     documents=Documents,
                //     UoMs=UOMs
                // };

                ResponseDTO=CreateResponse(HSCodeRequirements,documentList,UoMList,validTardePurpose);


                //ResponseDTO = HSCodeRequirements.Select(item => this.Command._mapper.Map<GetHSCodeRequirementsResponseDTO>(item)).ToList();
    

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
        public List<TestDTO>  CreateResponse(IList<UV_DocumentaryRequirement> HSCodeRequirements,IEnumerable<DocumentType> documentList,IEnumerable<UoM> UoMList,List<GetPurposeOfImportByHSCodeResponseDTO> validTardePurpose){
            List<TestDTO> testResp=new List<TestDTO>();

                //Testing Respose
                foreach(UV_DocumentaryRequirement record in HSCodeRequirements){
                    TestDTO test= new TestDTO();
                    test= this.Command._mapper.Map<TestDTO>(record);
                    test.UoM=searchUoM(UoMList,record.UoMID);
                    test.purposesOfImport=validTardePurpose;

                    DocumentResponseDTO document=searchDocumentCode(documentList,record.RequiredDocumentTypeCode);
                           DocumentResponseDTO finaldocument=new DocumentResponseDTO(){
                                DocumentCode=document.DocumentCode,
                                DocumentName=document.DocumentName
                        };
                     test.RequiredDocument=document;  
                     test.RequestedDocument=searchDocumentCode(documentList,record.RequestedDocument);
 
                        
                    //test.RequiredDocument=RequiredDocuments;
                    //test.RequestedDocument=RequestedDocuments;
                    // testResp.Add(new TestDTO{

                    // });
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
