using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;
namespace WebAPI.Mapper
{
    public class ClientDomainSuggestionsMapper : Profile
    {
        public ClientDomainSuggestionsMapper()
        {
            CreateMap<ClientDomainSuggestions, ClientDomainSuggestionsDTO>();
            CreateMap<ClientDomainSuggestionsDTO, ClientDomainSuggestions>();
        }
    }
}
