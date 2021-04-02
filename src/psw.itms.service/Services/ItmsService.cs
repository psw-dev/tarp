
// Microsoft Libraries
using System.Collections.Generic;
// Libraries Namespaces 
using AutoMapper;
// Project Namespaces 
using PSW.ITMS.Data;
using PSW.ITMS.Service.Exceptions;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.Strategies;
using System.Security.Claims;

namespace PSW.ITMS.Service
{
    public class ItmsService : IItmsService
    {
        #region properties 
        public readonly IMapper _mapper;
        public IUnitOfWork UnitOfWork { get; set; }
        public StrategyFactory StrategyFactory{get; set;}
        public IEnumerable<Claim> UserClaims { get; set; }
        #endregion


        #region constuctors & destroctors
        public ItmsService(IMapper mapper) => _mapper = mapper;

        public ItmsService()
        {
        }    
        
        #endregion

        #region Invoke Function 

        public CommandReply invokeMethod(CommandRequest request)
        {
           try
           {
                // Initialize Mapper in Command Request 
                request._mapper = this._mapper;
                //check if UnitOfWork is set otherwise set the service's UoW as default
                request.UnitOfWork = request.UnitOfWork ?? this.UnitOfWork;
                //check if UserClaims is set otherwise set the service's user claims as default
                request.UserClaims = request.UserClaims ?? this.UserClaims;
                //create strategy based on request. it can be dynamic
                Strategy strategy = this.StrategyFactory.CreateStrategy(request);
                //validate request for strategy
                bool isValide = strategy.Validate();
                //Execute strategy
                CommandReply reply = strategy.Execute();
                return reply;
           }
           catch(ServiceException ex)
           {
               //TODO: Catch the exception 
                //_logger.Log("Error Occured : " + ex.Message);
               throw ex;
           }
        }   

        #endregion


    }

}