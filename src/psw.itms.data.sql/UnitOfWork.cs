/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

using PSW.ITMS.Common;
using PSW.ITMS.Data.Repositories;
using PSW.ITMS.Data.Sql.Repositories;
using PSW.ITMS.Data.Objects.Views;

namespace PSW.ITMS.Data.Sql
{

    public class UnitOfWork : IUnitOfWork
    {
        #region Private Properties SHRD
        private IAddDeclarationRequirementRepository _addDeclarationRequirementRepository;
        private IDeclarationCategoryRepository _declarationCategoryRepository;
        private IDocumentRequirementRepository _documentRequirementRepository;
        private IFinancialRequirementRepository _financialRequirementRepository;
        private IHSCodeTARPRepository _hSCodeTARPRepository;
        private ILogicOperatorRepository _logicOperatorRepository;
        private INilRequirementRepository _nilRequirementRepository;
        private IRefusalIntimationRepository _refusalIntimationRepository;
        private IRequestTypeRepository _requestTypeRepository;
        private IRequirementRepository _requirementRepository;
        private IRequirementCategoryRepository _requirementCategoryRepository;
        private IRequirementSetRepository _requirementSetRepository;
        private IRequirementStageRepository _requirementStageRepository;
        private ITermUoMRepository _termUoMRepository;
        private ITestingRequirementRepository _testingRequirementRepository;
        private IValidityTermRequirementRepository _validityTermRequirementRepository;
        #endregion

        #region Private Properties TARP Views
        private IUV_DocumentaryRequirementRepository _uv_DocumentaryRequirementRepository;
        #endregion

        #region Private Properties SHRD
        private IAgencyRepository _agencyRepository;
        private IAppConfigRepository _appConfigRepository;
        private IAttachedObjectFormatRepository _attachedObjectFormatRepository;
        private IAttachmentStatusRepository _attachmentStatusRepository;
        private ICityRepository _cityRepository;
        private ICountryRepository _countryRepository;
        private ICountrySubEntityRepository _countrySubEntityRepository;
        private ICurrencyRepository _currencyRepository;
        private IDialingCodeRepository _dialingCodeRepository;
        private IDocumentTypeRepository _documentTypeRepository;
        private IGenderRepository _genderRepository;
        private IMinistryRepository _ministryRepository;
        private IPortRepository _portRepository;
        private ITradePurposeRepository _tradePurposeRepository;
        private ITradeTranTypeRepository _tradeTranTypeRepository;
        private IUoMRepository _uoMRepository;
        private IZoneRepository _zoneRepository;

        #endregion 

        #region Public Properties ITMS
        public IAddDeclarationRequirementRepository AddDeclarationRequirementRepository => _addDeclarationRequirementRepository ?? (_addDeclarationRequirementRepository = new AddDeclarationRequirementRepository(_connection));
        public IDeclarationCategoryRepository DeclarationCategoryRepository => _declarationCategoryRepository ?? (_declarationCategoryRepository = new DeclarationCategoryRepository(_connection));
        public IDocumentRequirementRepository DocumentRequirementRepository => _documentRequirementRepository ?? (_documentRequirementRepository = new DocumentRequirementRepository(_connection));
        public IFinancialRequirementRepository FinancialRequirementRepository => _financialRequirementRepository ?? (_financialRequirementRepository = new FinancialRequirementRepository(_connection));
        public IHSCodeTARPRepository HSCodeTARPRepository => _hSCodeTARPRepository ?? (_hSCodeTARPRepository = new HSCodeTARPRepository(_connection));
        public ILogicOperatorRepository LogicOperatorRepository => _logicOperatorRepository ?? (_logicOperatorRepository = new LogicOperatorRepository(_connection));
        public INilRequirementRepository NilRequirementRepository => _nilRequirementRepository ?? (_nilRequirementRepository = new NilRequirementRepository(_connection));
        public IRefusalIntimationRepository RefusalIntimationRepository => _refusalIntimationRepository ?? (_refusalIntimationRepository = new RefusalIntimationRepository(_connection));
        public IRequestTypeRepository RequestTypeRepository => _requestTypeRepository ?? (_requestTypeRepository = new RequestTypeRepository(_connection));
        public IRequirementRepository RequirementRepository => _requirementRepository ?? (_requirementRepository = new RequirementRepository(_connection));
        public IRequirementCategoryRepository RequirementCategoryRepository => _requirementCategoryRepository ?? (_requirementCategoryRepository = new RequirementCategoryRepository(_connection));
        public IRequirementSetRepository RequirementSetRepository => _requirementSetRepository ?? (_requirementSetRepository = new RequirementSetRepository(_connection));

        public IRequirementStageRepository RequirementStageRepository => _requirementStageRepository ?? (_requirementStageRepository = new RequirementStageRepository(_connection));
        public ITermUoMRepository TermUoMRepository => _termUoMRepository ?? (_termUoMRepository = new TermUoMRepository(_connection));
        public ITestingRequirementRepository TestingRequirementRepository => _testingRequirementRepository ?? (_testingRequirementRepository = new TestingRequirementRepository(_connection));
        public IValidityTermRequirementRepository ValidityTermRequirementRepository => _validityTermRequirementRepository ?? (_validityTermRequirementRepository = new ValidityTermRequirementRepository(_connection));
        #endregion

        #region Public Properties ITMS Views
        public IUV_DocumentaryRequirementRepository UV_DocumentaryRequirementRepository => _uv_DocumentaryRequirementRepository ?? (_uv_DocumentaryRequirementRepository = new UV_DocumentaryRequirementRepository(_connection));

        #endregion

        #region Public Properties SHRD

        public IAgencyRepository AgencyRepository => _agencyRepository ?? (_agencyRepository = new AgencyRepository(_connection));
        public IAppConfigRepository AppConfigRepository => _appConfigRepository ?? (_appConfigRepository = new AppConfigRepository(_connection));
        public IAttachedObjectFormatRepository AttachedObjectFormatRepository => _attachedObjectFormatRepository ?? (_attachedObjectFormatRepository = new AttachedObjectFormatRepository(_connection));
        public IAttachmentStatusRepository AttachmentStatusRepository => _attachmentStatusRepository ?? (_attachmentStatusRepository = new AttachmentStatusRepository(_connection));
        public ICityRepository CityRepository => _cityRepository ?? (_cityRepository = new CityRepository(_connection));
        public ICountryRepository CountryRepository => _countryRepository ?? (_countryRepository = new CountryRepository(_connection));
        public ICountrySubEntityRepository CountrySubEntityRepository => _countrySubEntityRepository ?? (_countrySubEntityRepository = new CountrySubEntityRepository(_connection));
        public ICurrencyRepository CurrencyRepository => _currencyRepository ?? (_currencyRepository = new CurrencyRepository(_connection));
        public IDialingCodeRepository DialingCodeRepository => _dialingCodeRepository ?? (_dialingCodeRepository = new DialingCodeRepository(_connection));
        public IDocumentTypeRepository DocumentTypeRepository => _documentTypeRepository ?? (_documentTypeRepository = new DocumentTypeRepository(_connection));
        public IGenderRepository GenderRepository => _genderRepository ?? (_genderRepository = new GenderRepository(_connection));
        public IMinistryRepository MinistryRepository => _ministryRepository ?? (_ministryRepository = new MinistryRepository(_connection));
        public IPortRepository PortRepository => _portRepository ?? (_portRepository = new PortRepository(_connection));
        public ITradePurposeRepository TradePurposeRepository => _tradePurposeRepository ?? (_tradePurposeRepository = new TradePurposeRepository(_connection));
        public ITradeTranTypeRepository TradeTranTypeRepository => _tradeTranTypeRepository ?? (_tradeTranTypeRepository = new TradeTranTypeRepository(_connection));
        public IUoMRepository UoMRepository => _uoMRepository ?? (_uoMRepository = new UoMRepository(_connection));
        public IZoneRepository ZoneRepository => _zoneRepository ?? (_zoneRepository = new ZoneRepository(_connection));

        #endregion

        //private IEventBus _eventBus;

        //public IEventBus eventBus => _eventBus;
        private IConfiguration _configuration;

        public UnitOfWork(IConfiguration configuration) //, IEventBus evBus)
        {
            //_eventBus = evBus;
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString"));

        }
        // public UnitOfWork(IConfiguration configuration)
        // {
        //     _configuration = configuration;
        //     _connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString"));
        // }
        public UnitOfWork()
        {
            // TODO : Get Connection String From appSetting.json
            string connectionString = "Server=127.0.0.1;Initial Catalog=TARIP;User ID=sa;Password=@Password1;";
            _connection = new SqlConnection(connectionString);
            // _connection.Open();
        }

        public UnitOfWork(string connectionString)
        {

            _connection = new SqlConnection(connectionString);
        }

        public IDbConnection Connection
        {
            get
            {
                if (_connection == null) _connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString"));

                return _connection;
            }

            set
            {
                _connection = value;
            }
        }

        #region Private variables 
        private IDbConnection _connection; // = new SqlConnection(_configuration.GetConnectionString("SQLConnectionString")); ;	
        private IDbTransaction _transaction;
        #endregion


        #region Public methods

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
        }

        public List<object> ExecuteQuery(string query)
        {
            return (List<object>)_connection.Query<List<object>>(query);
        }

        public void OpenConnection()
        {
            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void CloseConnection()
        {
            try
            {
                if (_transaction != null)
                    _transaction.Commit();
                if (_connection != null && _connection.State != ConnectionState.Closed)
                    Connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void BeginTransaction()
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                _transaction = _connection.BeginTransaction();

                SetTransactions(_transaction);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void SetTransactions(IDbTransaction transaction)
        {
            var repositories = this.GetType().GetProperties();

            foreach (PropertyInfo r in repositories)
            {
                if (Utility.IsAssignableToGenericType(r.PropertyType, typeof(IRepository<>)))
                {

                    var repo = r.GetValue(this) as IRepositoryTransaction;
                    repo.SetTransaction(transaction);

                }
            }
        }

        public void Commit()
        {
            try
            {
                if (_transaction != null)
                    _transaction.Commit();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void Rollback()
        {
            try
            {
                if (_transaction != null)
                    _transaction.Rollback();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        #endregion
    }
}