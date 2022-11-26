using AutoMapper;
using PSW.ITMS.Data;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.Exceptions;
using PSW.ITMS.Service.Strategies;
using System.Security.Claims;
using System.Collections.Generic;
using PSW.Common.Crypto;
using StackExchange.Redis;

namespace PSW.ITMS.Service
{
    public class ItmsSecureService : IItmsSecureService
    {
        #region properties 
        public readonly IMapper _mapper;
        public IUnitOfWork UnitOfWork { get; set; }
        public IStrategyFactory StrategyFactory { get; set; }
        public ICryptoAlgorithm CryptoAlgorithm { get; set; }
        public IEnumerable<Claim> UserClaims { get; set; }
        public IConnectionMultiplexer RedisConnection { get; set; }
        #endregion


        #region constuctors & destroctors
        public ItmsSecureService(IMapper mapper) => _mapper = mapper;

        public ItmsSecureService(IMapper mapper, ICryptoAlgorithm cryptoAlgorithm)
        {
            _mapper = mapper;
            this.CryptoAlgorithm = cryptoAlgorithm;
        }

        public ItmsSecureService(IUnitOfWork unitOfWork, StrategyFactory strategyFactory, ICryptoAlgorithm cryptoAlgorithm)
        {
            this.UnitOfWork = unitOfWork;
            this.StrategyFactory = strategyFactory;
            this.CryptoAlgorithm = cryptoAlgorithm;

        }
        #endregion

        #region Invoke Function 

        public CommandReply invokeMethod(CommandRequest request)
        {
            try
            {
                // Initialize Mapper in Command Request 
                request._mapper = _mapper;
                //check if UnitOfWork is set otherwise set the service's UoW as default
                request.UnitOfWork = request.UnitOfWork ?? UnitOfWork;
                // Check if CryptoAlgorith is set otherwise set the service's Crypto Algorithm as default
                request.CryptoAlgorithm = request.CryptoAlgorithm ?? this.CryptoAlgorithm;
                //check if UserClaims is set otherwise set the service's user claims as default
                request.UserClaims = request.UserClaims ?? this.UserClaims;
                //create strategy based on request. it can be dynamic
                Strategy strategy = StrategyFactory.CreateStrategy(request);
                //validate request for strategy
                var isSecure = strategy.Validate();
                //Execute strategy
                var reply = isSecure
                    ? strategy.Execute()
                    : strategy.BadRequestReply(strategy.ValidationMessage);
                return reply;
            }
            catch (ServiceException ex)
            {
                //TODO: Catch the exception 
                //_logger.Log("Error Occured : " + ex.Message);
                throw ex;
            }
        }

        #endregion
    }
}
