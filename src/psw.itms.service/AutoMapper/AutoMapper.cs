using AutoMapper;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Service.DTO;


namespace PSW.ITMS.Service.Mapper
{

    public class ObjectMapper
    {
        public IMapper _mapper { get; set; }

        public ObjectMapper()
        {
            ConfigureMappings();
        }

        public IMapper GetMapper()
        {
            return _mapper;
        }

        public void ConfigureMappings()
        {
            try
            {
                // Place All Mappings Here
                var config = new MapperConfiguration(cfg =>
                {
                    //cfg.CreateMap<CountryWithDialingCodes, GetAllCountryCodesResponseDTO>();
                    // cfg.CreateMap<Country, GetAllCountriesResponseDTO>();
                    // cfg.CreateMap<PCTCode, GetHSCodeListResponseDTO>();
                    //cfg.CreateMap<TransportMeans, GetMeanOfTransportResponseDTO>();
                });
                _mapper = config.CreateMapper();
            }
            catch
            {
                throw;
            }
        }

    }
}
