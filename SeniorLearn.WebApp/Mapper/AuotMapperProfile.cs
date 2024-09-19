using AutoMapper;

namespace SeniorLearn.WebApp.Mapper
{
    public class AuotMapperProfile : Profile
    {
        public AuotMapperProfile()
        {
            CreateMap<Data.Member, Areas.Administration.Models.Member.Register>().ReverseMap();
            CreateMap<Data.Member, Areas.Administration.Models.Member.Manage>().ReverseMap();

            CreateMap<Data.Payment, Areas.Administration.Models.Payment.Create>().ReverseMap(); 
        }
    }
}
