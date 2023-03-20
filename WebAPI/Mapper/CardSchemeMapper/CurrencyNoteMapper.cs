using AutoMapper;
using HelperClasses.DTOs.CardScheme;
using WebAPI.Models;

namespace WebAPI.Mapper.CardSchemeMapper
{
    public class CardSchemeMapper:Profile
    {
        public CardSchemeMapper()
        {
            CreateMap<CardScheme, CardSchemeDTO>();
            CreateMap<CardSchemeDTO, CardScheme>();
        }
    }
}
