using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageAndSPWebsiteResponsesMapper.WebsiteResponses
{
    public class SparePartFooterContentDTOMapper : Profile
    {
        public SparePartFooterContentDTOMapper()
        {
            CreateMap<Garage, WebsiteFooterContentResponseDTO>()
                .ForPath(x => x.ContactUs.Email, x => x.MapFrom(y => y.ContactPersonEmail))
                .ForPath(x => x.ContactUs.PhoneNumber, x => x.MapFrom(y => y.ContactPersonNumber))
                .ForPath(x => x.ContactUs.Address, x => x.MapFrom(y => y.Address))
                .ForPath(x => x.ContactUs.Whatsapp, x => x.MapFrom(y => y.GarageBusinessSetting.Whatsapp))
                .ForPath(x => x.ContactUs.Lat, x => x.MapFrom(y => y.Latitude))
                .ForPath(x => x.ContactUs.Lng, x => x.MapFrom(y => y.Longitude))
                .ForPath(x => x.SocialMedia.Facebook, x => x.MapFrom(y => y.GarageBusinessSetting.Facebook))
                .ForPath(x => x.SocialMedia.Instagram, x => x.MapFrom(y => y.GarageBusinessSetting.Instagram))
                .ForPath(x => x.SocialMedia.Twitter, x => x.MapFrom(y => y.GarageBusinessSetting.Twitter))
                .ForMember(x => x.AboutUS, x => x.MapFrom(y => y.GarageContentManagement.ShortIntro))
                .ForMember(x => x.FooterImage, x => x.MapFrom(y => y.GarageContentManagement.FooterImage))
                .ForMember(x => x.Explore, x => x.MapFrom(y => y.GarageMenuManagement));

            CreateMap<GarageMenuManagement, WebsiteHeaderMenu>()
                .ForMember(x => x.Position, x => x.MapFrom(y => y.Position))
                .ForMember(x => x.Route, x => x.MapFrom(y => y.GarageMenu.Route))
                .ForMember(x => x.Title, x => x.MapFrom(y => y.GarageMenu.Title));

            CreateMap<SparePartsDealer, WebsiteFooterContentResponseDTO>()
                .ForPath(x => x.ContactUs.Email, x => x.MapFrom(y => y.ContactPersonEmail))
                .ForPath(x => x.ContactUs.PhoneNumber, x => x.MapFrom(y => y.ContactPersonNumber))
                .ForPath(x => x.ContactUs.Address, x => x.MapFrom(y => y.Address))
                .ForPath(x => x.ContactUs.Whatsapp, x => x.MapFrom(y => y.SparePartBusinessSetting.Whatsapp))
                .ForPath(x => x.ContactUs.Lat, x => x.MapFrom(y => y.Latitude))
                .ForPath(x => x.ContactUs.Lng, x => x.MapFrom(y => y.Longitude))
                .ForPath(x => x.SocialMedia.Facebook, x => x.MapFrom(y => y.SparePartBusinessSetting.Facebook))
                .ForPath(x => x.SocialMedia.Instagram, x => x.MapFrom(y => y.SparePartBusinessSetting.Instagram))
                .ForPath(x => x.SocialMedia.Twitter, x => x.MapFrom(y => y.SparePartBusinessSetting.Twitter))
                .ForMember(x => x.AboutUS, x => x.MapFrom(y => y.SparePartContentManagement.ShortIntro))
                .ForMember(x => x.FooterImage, x => x.MapFrom(y => y.SparePartContentManagement.FooterImage))
                .ForMember(x => x.Explore, x => x.MapFrom(y => y.SparePartMenuManagement));

            CreateMap<SparePartMenuManagement, WebsiteHeaderMenu>()
                .ForMember(x => x.Position, x => x.MapFrom(y => y.Position))
                .ForMember(x => x.Route, x => x.MapFrom(y => y.SparePartMenu.Route))
                .ForMember(x => x.Title, x => x.MapFrom(y => y.SparePartMenu.Title));
        }
    }
}
