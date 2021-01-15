using System;
using System.Globalization;
using AutoMapper;
// using PSW.ITMS.Common.Helper;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Service.DTO;

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
