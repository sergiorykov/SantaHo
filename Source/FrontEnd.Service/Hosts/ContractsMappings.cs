using AutoMapper;
using SantaHo.Domain.Letters;
using SantaHo.FrontEnd.ServiceContracts.Letters;

namespace SantaHo.FrontEnd.Service.Hosts
{
    public static class ContractsMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<WishListLetterRequest, Letter>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Name));

            Mapper.AssertConfigurationIsValid();
        }
    }
}