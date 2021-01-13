using System.Collections.Generic;
using AutoMapper;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Service.DTO;

namespace PSW.ITMS.Service.AutoMapper
{
    public class EntityToDTOMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "EntityToDTOMappings"; }
        }
        public EntityToDTOMappingProfile()
        {

           
            
        
        }
    }
}