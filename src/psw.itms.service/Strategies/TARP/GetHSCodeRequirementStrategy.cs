using System.Collections.Generic;
using System.Linq;
using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data.Objects.Views;
using System;
using System.Globalization;
using PSW.ITMS.Common;
using PSW.ITMS.Data.Entities;
using psw.security.Encryption;

namespace PSW.ITMS.Service.Strategies
{
    public class GetHsCodeRequirementStrategy : ApiStrategy<GetHsCodeRequirementsRequestDto, GetHsCodeRequirementsResponseDto>
    {
        #region Constructors

        public GetHsCodeRequirementStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
        }

        #endregion 

        #region Distructors

        ~GetHsCodeRequirementStrategy()
        {

        }

        #endregion 

        #region Strategy Excecution

        public override CommandReply Execute()
        {
            try 
            {
                // Retrieve All required Data from DB
                List<TradePurpose> tradePurposes = GetAllTradePurposes(RequestDTO);

                // Get Applicable Rules on HSCode and Document Type Code
                var hsCodeRequirements = GetHsCodeRequirements(RequestDTO.DocumentTypeCode, RequestDTO.HsCode);
                if(!hsCodeRequirements.Any())
                    return NotFoundReply("No data found against this HS Code");

                // Creating Response
                GetHsCodeRequirementsResponseDto dto = CreateResponse(hsCodeRequirements, tradePurposes);

                //Create Response               
                ResponseDTO = dto;

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

        /// <summary>
        /// Creating response by parsing HS Code Rules
        /// </summary>
        /// <param name="hsCodeRequirements"></param>
        /// <param name="tradePurposes"></param>
        /// <returns></returns>
        public GetHsCodeRequirementsResponseDto CreateResponse(IList<UV_DocumentaryRequirement> hsCodeRequirements, List<TradePurpose> tradePurposes)
        {
            GetHsCodeRequirementsResponseDto response = new GetHsCodeRequirementsResponseDto();
            UV_DocumentaryRequirement requirement = hsCodeRequirements.ElementAt(0);

            response.HsCodeExt = requirement.HSCodeExt;
            response.ItemDescription = requirement.ItemDescription;
            response.TechnicalName = requirement.TechnicalName;
            response.RequestedDocumentCode = requirement.RequestedDocument;

            if (requirement.UoMID != 0)
            {
                int uomId = Convert.ToInt32(requirement.UoMID);
                UoM UoM = Command.UnitOfWork.UoMRepository.Find(uomId);
                UOMResponseDTO uomDto = new UOMResponseDTO
                {
                    ID = uomId,
                    Name = UoM.Name
                };

                response.UoM = uomDto;
            }

            // Checking if Permitted or not
            var uvPermittedRequirements = hsCodeRequirements.Where(x =>
                x.RequirementCategoryID == (int)psw.itms.common.Enums.RequirementCategory.NotPermitted);

            if (uvPermittedRequirements.Any())
            {
                response.IsNotPermitted = true;
                return response;
            }

            // Specific FSCRD Check
            string fscrdDocumentCode = "D13";
            var uvDocumentaryRequirements = hsCodeRequirements.Where(x =>
                x.RequirementCategoryID == (int) psw.itms.common.Enums.RequirementCategory.Documentary && x.RequiredDocumentTypeCode == fscrdDocumentCode);

            if (uvDocumentaryRequirements.Any()) response.IsFscrdEnlistmentRequired = true;

            var distinctPurposes = hsCodeRequirements.Select(x => x.PurposeIDList).Distinct().ToList();

            if (distinctPurposes.Count > 0)
            {
                foreach (string purposes in distinctPurposes)
                {
                    PurposeWiseRequirement purposeWiseRequirement = new PurposeWiseRequirement();
                    var financialRequirements = hsCodeRequirements.Where(x =>
                        x.RequirementCategoryID == (int)psw.itms.common.Enums.RequirementCategory.Financial && x.PurposeIDList == purposes).ToList();

                    if (financialRequirements.Count > 0)
                    {
                        var financialRequirement = financialRequirements.ElementAt(0);
                        purposeWiseRequirement.BillAmount = PSWEncryption.encrypt(financialRequirement.BillAmount.ToString(CultureInfo.CurrentCulture));
                    }

                    var documentaryRequirements = hsCodeRequirements.Where(x =>
                        x.RequirementCategoryID == (int)psw.itms.common.Enums.RequirementCategory.Documentary && x.PurposeIDList == purposes).ToList();

                    if (documentaryRequirements.Count > 0)
                    {
                        IList<string> purposeIdList = Utility.StringSplitter(purposes);
                        foreach (string purposeId in purposeIdList)
                        {
                            if (purposeWiseRequirement.TradePurposes.All(p => p.ID != Convert.ToInt32(purposeId)))
                            {
                                GetPurposeOfImportByHSCodeResponseDTO purposeDto = SearchImportPurposeId(tradePurposes, Convert.ToInt32(purposeId));
                                if (purposeDto != null) purposeWiseRequirement.TradePurposes.Add(purposeDto);
                            }
                        }

                        foreach (var documentaryRequirement in documentaryRequirements)
                        {
                            if (documentaryRequirement.RequiredDocumentTypeCode != null && documentaryRequirement.RequiredDocumentTypeName != null)
                            {
                                purposeWiseRequirement.RequiredDocument.Add(new DocumentResponseDTO
                                {
                                    DocumentCode = documentaryRequirement.RequiredDocumentTypeCode,
                                    DocumentName = documentaryRequirement.RequiredDocumentTypeName
                                });
                            }
                        }
                    }

                    response.PurposeWiseRequirements.Add(purposeWiseRequirement);
                }
            }

            return response;
        }

        /// <summary>
        /// Search Import Purpose by Import Purpose Id and return Data Transfer Object
        /// </summary>
        /// <param name="importPurposeList"></param>
        /// <param name="importPurposeId"></param>
        /// <returns></returns>
        public GetPurposeOfImportByHSCodeResponseDTO SearchImportPurposeId(IEnumerable<TradePurpose> importPurposeList, int importPurposeId)
        {
            GetPurposeOfImportByHSCodeResponseDTO response = new GetPurposeOfImportByHSCodeResponseDTO();
            var importPurpose = importPurposeList.FirstOrDefault(x => x.ID == importPurposeId);
            if (importPurpose == null) return null;

            response.ID = importPurposeId;
            response.Name = importPurpose.Name;

            return response;
        }

        /// <summary>
        /// Pulling Trade Purposes on the basis of Agency Id and Trade Tran Type Id
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public List<TradePurpose> GetAllTradePurposes(GetHsCodeRequirementsRequestDto requestDto)
        {
            try
            {
                // Begin Transaction  
                Command.UnitOfWork.BeginTransaction();

                // Query Database 
                IEnumerable<TradePurpose> tradePurposes = Command.UnitOfWork.TradePurposeRepository.Where(new {
                    AgencyID = requestDto.AgencyId,
                    TradeTranTypeID = requestDto.TradeTranType
                });

                // Commit Transaction  
                Command.UnitOfWork.Commit();

                return tradePurposes.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Pulling data from view on 12 digit HSCode and Document Type Code
        /// </summary>
        /// <param name="requestedDocument"></param>
        /// <param name="hsCode"></param>
        /// <returns></returns>
        public IList<UV_DocumentaryRequirement> GetHsCodeRequirements(string requestedDocument, string hsCode)
        {
            try
            {
                // Begin Transaction  
                Command.UnitOfWork.BeginTransaction();

                // Query Database 
                IList<UV_DocumentaryRequirement> hsCodeRequirements = Command.UnitOfWork.UV_DocumentaryRequirementRepository.Where(new {
                    RequestedDocument = requestedDocument,
                    HSCodeExt = hsCode
                });

                // Commit Transaction  
                Command.UnitOfWork.Commit();

                return hsCodeRequirements;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion 
    }
}
