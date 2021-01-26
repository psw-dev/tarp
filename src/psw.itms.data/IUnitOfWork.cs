/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using PSW.ITMS.Data.Repositories;
using PSW.ITMS.Data.Objects.Views;
using PSW.RabbitMq;

namespace PSW.ITMS.Data
{
    public interface IUnitOfWork : IDisposable
    {
        #region ITMS Repositories
        IAddDeclarationRequirementRepository AddDeclarationRequirementRepository { get; }
		IDeclarationCategoryRepository DeclarationCategoryRepository { get; }
		IDocumentRequirementRepository DocumentRequirementRepository { get; }
		IFinancialRequirementRepository FinancialRequirementRepository { get; }
        IHSCodeTARPRepository HSCodeTARPRepository { get; }		
        ILogicOperatorRepository LogicOperatorRepository { get; }
		INilRequirementRepository NilRequirementRepository { get; }
		IRefusalIntimationRepository RefusalIntimationRepository { get; }
		IRequestTypeRepository RequestTypeRepository { get; }
		IRequirementRepository RequirementRepository { get; }
		IRequirementCategoryRepository RequirementCategoryRepository { get; }
		IRequirementSetRepository RequirementSetRepository { get; }
        IRequirementStageRepository RequirementStageRepository { get; }
		ITermUoMRepository TermUoMRepository { get; }
		ITestingRequirementRepository TestingRequirementRepository { get; }
		IValidityTermRequirementRepository ValidityTermRequirementRepository { get; }
       
        #endregion

        #region TARP View Repositories
		IUV_DocumentaryRequirementRepository UV_DocumentaryRequirementRepository  { get; }

	   	#endregion
        #region SHRD Repositories
        IAgencyRepository AgencyRepository { get; }
		IAppConfigRepository AppConfigRepository { get; }
		IAttachedObjectFormatRepository AttachedObjectFormatRepository { get; }
		IAttachmentStatusRepository AttachmentStatusRepository { get; }
		ICityRepository CityRepository { get; }
		ICountryRepository CountryRepository { get; }
		ICountrySubEntityRepository CountrySubEntityRepository { get; }
		ICurrencyRepository CurrencyRepository { get; }
		IDialingCodeRepository DialingCodeRepository { get; }
		IDocumentTypeRepository DocumentTypeRepository { get; }
		IGenderRepository GenderRepository { get; }
		IMinistryRepository MinistryRepository { get; }
		IPortRepository PortRepository { get; }
		ITradePurposeRepository TradePurposeRepository { get; }
		ITradeTranTypeRepository TradeTranTypeRepository { get; }
		IUoMRepository UoMRepository { get; }
		IZoneRepository ZoneRepository { get; }
        #endregion

        #region Methods
        void BeginTransaction();
        void Commit();

        #endregion
    }
}
