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
                
                /*----------- RECORDS FOR HSCODE 1214.9000-----------------*/
                SeedData record1=new SeedData(){
                    AgencyId=2,
                    HSCode="1214.9000",
                    HSCodeExt="",
                    CommodityName="ALFALFA / ALFALFA HAY",
                    Purpose="Animal Feed",
                    IPDocumentaryRequirements="Application on DPP Prescribed Form 4|Proforma Invoice|Fee Challan",
                    IPFees="5000 Per 6 months" ,
                    TechnicalName="Medicago sativa",
                    UoMID=1,
                    UoMName="gram",
                    BannedCountries=new string[]{},
                    AllowedCountries=new string[]{"SUADAN", "U.S.A","ARGENTINA"}                
                };
                
                SeedData record2=new SeedData(){
                    AgencyId=2,
                    HSCode="1214.9000",
                    HSCodeExt="",
                    CommodityName="CLOVER MIXTURE",
                    Purpose="Commercial Sowing",
                    IPDocumentaryRequirements="Application on DPP prescribed form 3|Proforma Invoice|Fee Challan|Enlisted variety proof from FSC&RD if import is for planting or propagation purpose|NOC from NBC if the imported product is GMO",
                    IPFees="5000 Per 6 months" ,
                    TechnicalName="",
                    UoMID=1,
                    UoMName="gram",
                    BannedCountries=new string[]{},
                    AllowedCountries=new string[]{"INDONESIA"}
                };
                
                SeedData record3=new SeedData(){
                    AgencyId=2,
                    HSCode="1214.9000",
                    HSCodeExt="",
                    CommodityName="CLOVER MIXTURE",
                    Purpose="Screening/Research/Trail",
                    IPDocumentaryRequirements="Application on DPP prescribed form 2|Proforma Invoice|Fee Challan |NOC from NBC if the imported product is GMO",
                    IPFees="5000 Per 6 months" ,
                    TechnicalName="",
                    UoMID=1,
                    UoMName="gram",
                    BannedCountries=new string[]{},
                    AllowedCountries=new string[]{"INDONESIA"}
                };
                /*-----------END RECORDS FOR HSCODE 1214.9000-----------------*/

                /*----------- RECORDS FOR HSCODE 0904.2120-----------------*/
                


                SeedData record_09042120_1=new SeedData(){
                    AgencyId=2,
                    HSCode="0904.2120",
                    HSCodeExt="",
                    CommodityName="RED CHILLI SEEDS FOR SOWING",
                    Purpose="Commercial Sowing",
                    IPDocumentaryRequirements="Application on DPP prescribed form 3|Proforma Invoice|Fee Challan|Enlisted variety proof from FSC&RD if import is for planting or propagation purpose|NOC from NBC if the imported product is GMO",
                    IPFees="5000 Per 6 months" ,
                    TechnicalName="Capsicum annuum",
                    UoMID=1,
                    UoMName="gram",
                    BannedCountries=new string[]{"FRANCE", "JAPAN", "U.S.A"},
                    AllowedCountries=new string[]{"CHINA" , "EGYPT" , "U.A.E"}

                };
                 SeedData record_09042120_2=new SeedData(){
                    AgencyId=2,
                    HSCode="0904.2120",
                    HSCodeExt="",
                    CommodityName="Dry Red Chilli",
                    Purpose="Consumption",
                    IPDocumentaryRequirements="Application on DPP prescribed form 4|Proforma Invoice|Fee Challan",
                    IPFees="5000 Per 6 months" ,
                    TechnicalName="Capsicum annum",
                    UoMID=1,
                    UoMName="gram",
                    BannedCountries=new string[]{},
                    AllowedCountries=new string[]{"China"}
                };
                 SeedData record_09042120_3=new SeedData(){
                    AgencyId=2,
                    HSCode="0904.2120",
                    HSCodeExt="",
                    CommodityName="RED CHILLI SEEDS FOR SOWING",
                    Purpose="Screening/Research/Trail",
                    IPDocumentaryRequirements="Application on DPP prescribed form 2|Proforma Invoice|Fee Challan|NOC from NBC if the imported product is GMO",
                    IPFees="5000 Per 6 months" ,
                    TechnicalName="Capsicum annuum",
                    UoMID=1,
                    UoMName="gram",
                    BannedCountries=new string[]{},
                    AllowedCountries=new string[]{"All"}
                };
                
                List<SeedData> seedData= new List<SeedData>();

                seedData.Add(record1);
                seedData.Add(record2);
                seedData.Add(record3);

                seedData.Add(record_09042120_1);
                seedData.Add(record_09042120_2);
                seedData.Add(record_09042120_3);

                return seedData;
        }

        public List<object> FilterData(string request,string HSCode,string importPurpose){
            List<object> filteredData=new List<object>();
            List<SeedData> desiredList=new List<SeedData>();
            
            switch(request)
            {
                case "Import Purpose": 
                   var ImportPurposeList=GetAllImportPurpose();
                   desiredList=MockData.FindAll(x => x.HSCode == HSCode);
                   //int id=1;
                    foreach(SeedData item in desiredList){
                        
                        int purposeID=searchImportPurposeID(ImportPurposeList,item.Purpose);
                        //var pupose=ImportPurposeList.Find(x => (x.Name == item.Purpose));
                        GetPurposeOfImportByHSCodeResponseDTO response=new GetPurposeOfImportByHSCodeResponseDTO(){
                            ID=purposeID,
                            Name=item.Purpose
                        };
                        filteredData.Add(response) ;
                    }                    
                    return filteredData;
                    

                case "Technical Name":                   
                    desiredList=MockData.FindAll(x => (x.HSCode == HSCode && x.Purpose==importPurpose));
                    foreach(SeedData item in desiredList){
                        GetNamesOfPlantAndPlantProductsResponseDTO response=new GetNamesOfPlantAndPlantProductsResponseDTO(){                                                      
                            Name=item.TechnicalName
                        };
                        filteredData.Add(item.TechnicalName) ;
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

                case "Banned Countries": 
                    desiredList=MockData.FindAll(x => (x.HSCode == HSCode && x.Purpose==importPurpose));                
                    foreach(SeedData item in desiredList){
                        // GetIPFeesResponseDTO response=new GetIPFeesResponseDTO(){
                        //     HSCode=item.HSCode,
                        //     Purpose=item.Purpose,                            
                        //     IPFees=item.IPFees
                        // };
                        filteredData.Add(item.BannedCountries) ;
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
        
        public List<TradePurpose> GetAllImportPurpose()
        {
            try
            {
                //TODO: Query ITMS to get Document Type w.r.t HSCode and Agency Id

                //For The Time being Getting the desire values from shared DB directly
                
                
                // Begin Transaction  
                this.Command.UnitOfWork.BeginTransaction();

                // Query Database 
                IEnumerable<TradePurpose> ImportPurposeList =
                    this.Command.UnitOfWork.TradePurposeRepository.Get();

                // Commit Transaction  
                this.Command.UnitOfWork.Commit();

                return ImportPurposeList.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        
        public int searchImportPurposeID(IEnumerable<TradePurpose> importPurposeList, string importPurposeName){
            var ImportPurpose=importPurposeList.FirstOrDefault(x => x.Name == importPurposeName);                       
            if(ImportPurpose== null){
                Random _random = new Random();
                return _random.Next(11, 99);
            }
            return ImportPurpose.ID;
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

       