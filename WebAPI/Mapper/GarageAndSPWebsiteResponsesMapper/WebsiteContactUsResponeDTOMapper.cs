using AutoMapper;
using WebAPI.Models;
using HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses;

namespace WebAPI.Mapper.GarageAndSPWebsiteResponsesMapper
{
    public class WebsiteContactUsResponeDTOMapper:Profile
    {
        public WebsiteContactUsResponeDTOMapper()
        {
            CreateMap<Garage, WebisteContactUsResponseDTO>()
                    .ForMember(x => x.GarageId, x => x.MapFrom(y => y.GarageBusinessSetting.GarageId))
                    .ForPath(x => x.Latitude, x => x.MapFrom(y => y.GarageBusinessSetting.Latitude))
                    .ForPath(x => x.Longitude, x => x.MapFrom(y => y.GarageBusinessSetting.Longitude))
                    .ForPath(x => x.Address, x => x.MapFrom(y => y.GarageBusinessSetting.CompleteAddress))
                    .ForPath(x => x.Email, x => x.MapFrom(y => y.GarageBusinessSetting.Email))
                    .ForPath(x => x.PhoneNumber, x => x.MapFrom(y => y.GarageBusinessSetting.Contact01))
                    .ForPath(x => x.PhoneText, x => x.MapFrom(y => y.GarageBusinessSetting.PhoneText))
                    .ForPath(x => x.EmailText, x => x.MapFrom(y => y.GarageBusinessSetting.EmailText));
        }
    }
}
