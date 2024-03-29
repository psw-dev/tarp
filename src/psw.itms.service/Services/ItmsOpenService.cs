﻿using AutoMapper;
using PSW.ITMS.Data;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.Exceptions;
using PSW.ITMS.Service.Strategies;
using PSW.Common.Crypto;

namespace PSW.ITMS.Service
{
    public class ItmsOpenService : IItmsOpenService
    {
        #region properties 
        public readonly IMapper _mapper;
        public IUnitOfWork UnitOfWork { get; set; }
        public IStrategyFactory StrategyFactory { get; set; }
        public ICryptoAlgorithm CryptoAlgorithm { get; set; }
        #endregion


        #region constuctors & destroctors
        public ItmsOpenService(IMapper mapper) => _mapper = mapper;

        public ItmsOpenService()
        {
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
                //create strategy based on request. it can be dynamic
                var strategy = StrategyFactory.CreateStrategy(request);
                //validate request for strategy
                var isValide = strategy.Validate();
                //Execute strategy
                var reply = strategy.Execute();
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
