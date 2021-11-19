using AutoMapper;

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

        }
    }
}
