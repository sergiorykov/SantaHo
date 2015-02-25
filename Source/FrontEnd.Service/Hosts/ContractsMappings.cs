using System;
using AutoMapper;
using SantaHo.Domain.SantaOffice.Letters;
using SantaHo.FrontEnd.ServiceContracts.Letters;

namespace SantaHo.FrontEnd.Service.Hosts
{
    public static class ContractsMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<WishListLetterRequest, IncomingChildLetter>()
                  .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Name));

            Mapper.AssertConfigurationIsValid();
        }
    }
}
