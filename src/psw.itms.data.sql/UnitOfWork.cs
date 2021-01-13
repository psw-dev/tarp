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

namespace PSW.ITMS.Data.Sql
{

    public class UnitOfWork : IUnitOfWork
    {
    
        #region Public Properties SHRD
        private IAgencyRepository _agencyRepository;
        private IAppConfigRepository _appConfigRepository;
        private IAttachedObjectFormatRepository _attachedObjectFormatRepository;
        private IAttachmentStatusRepository _attachmentStatusRepository;
        private IBankRepository _bankRepository;
        private IBranchRepository _branchRepository;
        private IChannelRepository _channelRepository;
        private ICityRepository _cityRepository;
        private ICollectorateRepository _collectorateRepository;
        private IConsignmentCategoryRepository _consignmentCategoryRepository;
        private IConsignmentModeRepository _consignmentModeRepository;
        private ICountryRepository _countryRepository;
        private ICountryWithDialingCodeRepository _countryWithDialingCodeRepository;

        private ICountrySubEntityRepository _countrySubEntityRepository;
        private ICurrencyRepository _currencyRepository;
        private IDeclarationTypeRepository _declarationTypeRepository;
        private IDeliveryTermRepository _deliveryTermRepository;
        private IDialingCodeRepository _dialingCodeRepository;
        private IDocumentTypeRepository _documentTypeRepository;
        private IGenderRepository _genderRepository;
        private IHazardClassRepository _hazardClassRepository;
        private IItemImportTypeRepository _itemImportTypeRepository;
        private IMinistryRepository _ministryRepository;
        private IPayChannelRepository _payChannelRepository;
        private IPayModeRepository _payModeRepository;
        private IPayTermRepository _payTermRepository;
        private IPCTCodeRepository _pCTCodeRepository;
        private IPortRepository _portRepository;
        private IPortTypeRepository _portTypeRepository;
        private IShedRepository _shedRepository;
        private ITradePurposeRepository _tradePurposeRepository;
        private ITradeTranTypeRepository _tradeTranTypeRepository;
        private IUoMRepository _uoMRepository;
        private IZoneRepository _zoneRepository;

        #endregion 


   
        #region Public Properties SHRD

        public IAgencyRepository AgencyRepository => _agencyRepository ?? (_agencyRepository = new AgencyRepository(_connection));
        public IAppConfigRepository AppConfigRepository => _appConfigRepository ?? (_appConfigRepository = new AppConfigRepository(_connection));
        public IAttachedObjectFormatRepository AttachedObjectFormatRepository => _attachedObjectFormatRepository ?? (_attachedObjectFormatRepository = new AttachedObjectFormatRepository(_connection));
        public IAttachmentStatusRepository AttachmentStatusRepository => _attachmentStatusRepository ?? (_attachmentStatusRepository = new AttachmentStatusRepository(_connection));
        public IBankRepository BankRepository => _bankRepository ?? (_bankRepository = new BankRepository(_connection));
        public IBranchRepository BranchRepository => _branchRepository ?? (_branchRepository = new BranchRepository(_connection));
        public IChannelRepository ChannelRepository => _channelRepository ?? (_channelRepository = new ChannelRepository(_connection));
        public ICityRepository CityRepository => _cityRepository ?? (_cityRepository = new CityRepository(_connection));
        public ICollectorateRepository CollectorateRepository => _collectorateRepository ?? (_collectorateRepository = new CollectorateRepository(_connection));
        public IConsignmentCategoryRepository ConsignmentCategoryRepository => _consignmentCategoryRepository ?? (_consignmentCategoryRepository = new ConsignmentCategoryRepository(_connection));
        public IConsignmentModeRepository ConsignmentModeRepository => _consignmentModeRepository ?? (_consignmentModeRepository = new ConsignmentModeRepository(_connection));
        public ICountryRepository CountryRepository => _countryRepository ?? (_countryRepository = new CountryRepository(_connection));
        public ICountryWithDialingCodeRepository CountryWithDialingCodeRepository => _countryWithDialingCodeRepository ?? (_countryWithDialingCodeRepository = new CountryWithDialingCodeRepository(_connection));
        public ICountrySubEntityRepository CountrySubEntityRepository => _countrySubEntityRepository ?? (_countrySubEntityRepository = new CountrySubEntityRepository(_connection));
        public ICurrencyRepository CurrencyRepository => _currencyRepository ?? (_currencyRepository = new CurrencyRepository(_connection));
        public IDeclarationTypeRepository DeclarationTypeRepository => _declarationTypeRepository ?? (_declarationTypeRepository = new DeclarationTypeRepository(_connection));
        public IDeliveryTermRepository DeliveryTermRepository => _deliveryTermRepository ?? (_deliveryTermRepository = new DeliveryTermRepository(_connection));
        public IDialingCodeRepository DialingCodeRepository => _dialingCodeRepository ?? (_dialingCodeRepository = new DialingCodeRepository(_connection));
        public IDocumentTypeRepository DocumentTypeRepository => _documentTypeRepository ?? (_documentTypeRepository = new DocumentTypeRepository(_connection));
        public IGenderRepository GenderRepository => _genderRepository ?? (_genderRepository = new GenderRepository(_connection));
        public IHazardClassRepository HazardClassRepository => _hazardClassRepository ?? (_hazardClassRepository = new HazardClassRepository(_connection));
        public IItemImportTypeRepository ItemImportTypeRepository => _itemImportTypeRepository ?? (_itemImportTypeRepository = new ItemImportTypeRepository(_connection));
        public IMinistryRepository MinistryRepository => _ministryRepository ?? (_ministryRepository = new MinistryRepository(_connection));
        public IPayChannelRepository PayChannelRepository => _payChannelRepository ?? (_payChannelRepository = new PayChannelRepository(_connection));
        public IPayModeRepository PayModeRepository => _payModeRepository ?? (_payModeRepository = new PayModeRepository(_connection));
        public IPayTermRepository PayTermRepository => _payTermRepository ?? (_payTermRepository = new PayTermRepository(_connection));
        public IPCTCodeRepository PCTCodeRepository => _pCTCodeRepository ?? (_pCTCodeRepository = new PCTCodeRepository(_connection));
        public IPortRepository PortRepository => _portRepository ?? (_portRepository = new PortRepository(_connection));
        public IPortTypeRepository PortTypeRepository => _portTypeRepository ?? (_portTypeRepository = new PortTypeRepository(_connection));
        public IShedRepository ShedRepository => _shedRepository ?? (_shedRepository = new ShedRepository(_connection));
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
            string connectionString = "Server=127.0.0.1;Initial Catalog=ITMS;User ID=sa;Password=@Password1;";
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