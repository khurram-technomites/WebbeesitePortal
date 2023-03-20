using AutoMapper;
using HelperClasses.DTOs.CardScheme;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class CardSchemeMapper : Profile
    {
        public CardSchemeMapper()
        {
            CreateMap<CardSchemeDTO, CardSchemeViewModel>();
            CreateMap<CardSchemeViewModel, CardSchemeDTO>();
        }
    }
}
