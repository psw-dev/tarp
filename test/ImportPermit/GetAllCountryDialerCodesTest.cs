using System;
using Xunit;
using System.Text.Json;
using PSW.ITMS.Data.Sql;
using PSW.ITMS.Service.Strategies;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using Microsoft.Extensions.Configuration;

namespace PSW.ITMS.Service.Test
{
    public class GetAllCountryDialerCodesTest
    {

        [Fact]
        public void GetAllCountryDialerCodes()
        {
            //Create Service Object
            OGAService _service = new OGAService();

            UnitOfWork uow = new UnitOfWork();

            _service.UnitOfWork = uow;
            _service.StrategyFactory = new StrategyFactory(uow);


            CommandRequest commandRequest = new CommandRequest
            {
                methodId = "1617"
            };

            //Hitting Service Layer
            CommandReply resData = _service.invokeMethod(commandRequest);


            Assert.Contains("FTN User", resData.data.ToString());
            Assert.Contains("Entity", resData.data.ToString());
            Assert.Contains("Subscription Type Fetched", resData.message);
            Assert.Equal(resData.code, "200");
            // uow.CloseConnection();

        }

        [Fact]
        public void GetAllCountryDialerCodesNotFound()
        {
            //Create Service Object
            OGAService _service = new OGAService();
            UnitOfWork uow = new UnitOfWork();
            _service.UnitOfWork = uow;
            _service.StrategyFactory = new StrategyFactory(uow);


            CommandRequest commandRequest = new CommandRequest
            {
                methodId = "1617",
            };

            //Hitting Service Layer
            CommandReply resData = _service.invokeMethod(commandRequest);


            Assert.DoesNotContain("[]", resData.data.ToString());
            Assert.NotEqual(resData.code, "404");

            Assert.DoesNotContain("No Subscription Type Found", resData.message);

        }


    }
}
