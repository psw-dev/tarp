using AutoMapper;
using System.Globalization;
// using PSW.ITMS.Common.Helper;

namespace PSW.ITMS.Service.AutoMapper
{
    public class DTOToEntityMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "DTOToEntityMappings";
            }
        }

        public DTOToEntityMappingProfile()
        {
            var _culture = new CultureInfo("en-Us");
        }
    }
}
