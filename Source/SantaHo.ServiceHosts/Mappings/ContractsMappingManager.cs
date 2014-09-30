using AutoMapper;
using SantaHo.Domain.IncomingLetters;
using SantaHo.ServiceContracts.Letters;

namespace SantaHo.ServiceHosts.Mappings
{
    public static class ContractsMappingManager
    {
        public static void Configure()
        {
            Mapper.CreateMap<WishListLetterRequest, Letter>();

            Mapper.AssertConfigurationIsValid();
        }
    }
}