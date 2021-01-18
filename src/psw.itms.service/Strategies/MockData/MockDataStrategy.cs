using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data.Entities;
using AutoMapper;
using PSW.ITMS.Service.Mapper;

using PSW.ITMS.Service.ModelValidators;

namespace PSW.ITMS.Service.Strategies
{
    public class MockDataStrategy : ApiStrategy<MockDataRequestDTO, List<Object>>
    {

        #region Properties
        public List<SeedData> MockData { get; set; } 
        #endregion 

        #region Constructors 
        public MockDataStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
            this.MockData=InsertMockData();
        }
        #endregion 

        #region Distructors 
        ~MockDataStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  
        public override CommandReply Execute()
        {
            try
            {               
                // STEP 1 : Perform Business Logic
                List<Object> requiredData = FilterData(RequestDTO.Request,RequestDTO.HSCode,RequestDTO.ImportPurpose); 
                
                if(!requiredData.Any())
                    return NotFoundReply();

                // STEP 2 : Prepare Response => Entity To DTO Mapping & Set Response DTO  
                ResponseDTO = this.Command._mapper.Map<List<object>>(requiredData);
                 
                // STEP 3 : Send Command Reply
                return OKReply();
            }
            catch (Exception ex)
            {
                return InternalServerErrorReply(ex);
            }
        }
        #endregion


        #region Methods  
        public List<SeedData> InsertMockData(){
                
                SeedData record1=new SeedData(){
                AgencyId=2,
                HSCode="1214.9000",
                CommodityName="ALFALFA / ALFALFA HAY",
                Purpose="Animal feed",
                IPDocumentaryRequirements="Application on DPP Prescribed Form 4|Proforma Invoice|Fee Challan",
                IPFees="5000 Per 6 months" ,
                TechnicalName="Medicago sativa"                
                };
                
                SeedData record2=new SeedData(){
                    AgencyId=2,
                    HSCode="1214.9000",
                    CommodityName="CLOVER MIXTURE",
                    Purpose="Commercial Sowing",
                    IPDocumentaryRequirements="Application on DPP prescribed form 3|Proforma Invoice|Fee Challan|Enlisted variety proof from FSC&RD if import is for planting or propagation purpose|NOC from NBC if the imported product is GMO",
                    IPFees="5000 Per 6 months" ,
                    TechnicalName=""
                };
                
                SeedData record3=new SeedData(){
                    AgencyId=2,
                    HSCode="1214.9000",
                    CommodityName="CLOVER MIXTURE",
                    Purpose="Screening / Research/ Trails",
                    IPDocumentaryRequirements="Application on DPP prescribed form 2|Proforma Invoice|Fee Challan |NOC from NBC if the imported product is GMO",
                    IPFees="5000 Per 6 months" ,
                    TechnicalName=""
                };

                List<SeedData> seedData= new List<SeedData>();

                seedData.Add(record1);
                seedData.Add(record2);
                seedData.Add(record3);

                return seedData;
        }

        public List<object> FilterData(string request,string HSCode,string importPurpose){
            List<object> filteredData=new List<object>();
            List<SeedData> desiredList=new List<SeedData>();
            
            switch(request)
            {
                case "Import Purpose": 
                   desiredList=MockData.FindAll(x => x.HSCode == HSCode);
                    foreach(SeedData item in desiredList){
                        GetPurposeOfImportByHSCodeResponseDTO response=new GetPurposeOfImportByHSCodeResponseDTO(){
                            HSCode=item.HSCode,
                            Name=item.Purpose
                        };
                        filteredData.Add(response) ;
                    }                    
                    return filteredData;
                    

                case "Technical Name":                   
                    desiredList=MockData.FindAll(x => (x.HSCode == HSCode && x.Purpose==importPurpose));
                    foreach(SeedData item in desiredList){
                        GetNamesOfPlantAndPlantProductsResponseDTO response=new GetNamesOfPlantAndPlantProductsResponseDTO(){
                            HSCode=item.HSCode,
                            Purpose=item.Purpose,                            
                            Name=item.TechnicalName
                        };
                        filteredData.Add(response) ;
                    }                    
                    return filteredData;

                case "Document Type": 
                    var docmentList=GetAllDocumentType(2,HSCode);
                    desiredList=MockData.FindAll(x => (x.HSCode == HSCode && x.Purpose==importPurpose)); 
                    foreach(SeedData item in desiredList){
                        IList<string> documentNameList=StringSplitter(item.IPDocumentaryRequirements);                       
                        IList<DocumentResponseDTO> RequiredDocs =new List<DocumentResponseDTO>();                

                        foreach(String documentName in documentNameList){
                            string docCode=searchDocumentCode(docmentList,documentName);
                            RequiredDocs.Add(new DocumentResponseDTO{
                                DocumentCode=docCode,
                                DocumentName=documentName
                            });
                        }
                        GetAllDocumentTypeResponseDTO response=new GetAllDocumentTypeResponseDTO(){
                            HSCode=item.HSCode,
                            Purpose=item.Purpose,
                            Documents=RequiredDocs
                        };
                        filteredData.Add(response) ;
                    }                                        
                    return filteredData;

                case "IP Fees": 
                    desiredList=MockData.FindAll(x => (x.HSCode == HSCode && x.Purpose==importPurpose));                
                    foreach(SeedData item in desiredList){
                        GetIPFeesResponseDTO response=new GetIPFeesResponseDTO(){
                            HSCode=item.HSCode,
                            Purpose=item.Purpose,                            
                            IPFees=item.IPFees
                        };
                        filteredData.Add(response) ;
                    }                    
                    return filteredData;    
                                
                default: return filteredData;
            }                            
        }

        
        public List<string> StringSplitter(string value){
            return value.Split('|').ToList();
        }
        public IEnumerable<DocumentType> GetAllDocumentType(int agencyId,string HScode)
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
        
        public string searchDocumentCode(IEnumerable<DocumentType> documentList, string documentName){
            var doc=documentList.FirstOrDefault(x => x.Name == documentName);                       
            if(doc== null){
                Random _random = new Random();
                return "D"+_random.Next(9, 99).ToString("D2");
            }
            return doc.Code;
        }
        #endregion 

    }
}

       