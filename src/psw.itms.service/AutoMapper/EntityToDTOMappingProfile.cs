using System.Collections.Generic;
using AutoMapper;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Service.DTO;
using PSW.ITMS.Data.Objects.Views;

namespace PSW.ITMS.Service.AutoMapper
{
    public class EntityToDTOMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "EntityToDTOMappings";
            }
        }

        public EntityToDTOMappingProfile()
        {
            CreateMap<UV_DocumentaryRequirement, GetHSCodeRequirementsResponseDTO>()
            .ForMember(dest => dest.RequestedDocument, opt => opt.Ignore())
            .ForMember(dest => dest.RequiredDocument, opt => opt.Ignore())
            .ForMember(dest => dest.purposesOfImport, opt => opt.Ignore())
            .ForMember(dest => dest.UoM, opt => opt.Ignore());

            CreateMap<HSCodeTARP, HSCodesData>()
            .ForMember(dest => dest.HSCode, opt => opt.Ignore())
            .ForMember(dest => dest.HSCodeExt, opt => opt.Ignore())
            .ForMember(dest => dest.ItemDescription, opt => opt.Ignore())
            .ForMember(dest => dest.TechnicalName, opt => opt.Ignore());
        }
    }
}
