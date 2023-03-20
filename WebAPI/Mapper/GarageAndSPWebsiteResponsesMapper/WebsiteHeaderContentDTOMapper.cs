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
    public class WebsiteHeaderContentDTOMapper : Profile
    {
        public WebsiteHeaderContentDTOMapper()
        {
            CreateMap<GarageBannerSetting, WebsiteHeaderBanners>();
            CreateMap<GarageBannerSetting, WebsitePromoHeaderBanners>();

            CreateMap<Garage, WebsiteHeaderContentDTO>()
                .ForMember(x => x.GarageId, x => x.MapFrom(y => y.Id))
                .ForPath(x => x.ContactDetails.Email, x => x.MapFrom(y => y.ContactPersonEmail))
                .ForPath(x => x.ContactDetails.PhoneNumber, x => x.MapFrom(y => y.ContactPersonNumber))
                .ForPath(x => x.Favicon, x => x.MapFrom(y => y.GarageContentManagement.Favicon))
                .ForPath(x => x.Title, x => x.MapFrom(y => y.GarageContentManagement.Title))
                .ForPath(x => x.ContactDetails.SocialMedia.Facebook, x => x.MapFrom(y => y.GarageBusinessSetting.Facebook))
                .ForPath(x => x.ContactDetails.SocialMedia.Instagram, x => x.MapFrom(y => y.GarageBusinessSetting.Instagram))
                .ForPath(x => x.ContactDetails.SocialMedia.Twitter, x => x.MapFrom(y => y.GarageBusinessSetting.Twitter))
                .ForMember(x => x.Banners, x => x.MapFrom(y => y.GarageBannerSettings.Where(x => x.BannerType == Enum.GetName(typeof(BannerType), BannerType.Banner))))
                .ForMember(x => x.Menu, x => x.MapFrom(y => y.GarageMenuManagement));

            CreateMap<GarageMenuManagement, WebsiteHeaderMenu>()
                .ForMember(x => x.Position, x => x.MapFrom(y => y.Position))
                .ForMember(x => x.Route, x => x.MapFrom(y => y.GarageMenu.Route))
                .ForMember(x => x.Title, x => x.MapFrom(y => y.GarageMenu.Title));

            CreateMap<SparePartBannerSetting, WebsiteHeaderBanners>();
            CreateMap<SparePartBannerSetting, WebsitePromoHeaderBanners>();

            CreateMap<SparePartsDealer, WebsiteHeaderContentDTO>()
                .ForMember(x => x.GarageId, x => x.MapFrom(y => y.Id))
                .ForPath(x => x.ContactDetails.Email, x => x.MapFrom(y => y.ContactPersonEmail))
                .ForPath(x => x.ContactDetails.PhoneNumber, x => x.MapFrom(y => y.ContactPersonNumber))
                .ForPath(x => x.ContactDetails.SocialMedia.Facebook, x => x.MapFrom(y => y.SparePartBusinessSetting.Facebook))
                .ForPath(x => x.ContactDetails.SocialMedia.Instagram, x => x.MapFrom(y => y.SparePartBusinessSetting.Instagram))
                .ForPath(x => x.ContactDetails.SocialMedia.Twitter, x => x.MapFrom(y => y.SparePartBusinessSetting.Twitter))
                .ForMember(x => x.Banners, x => x.MapFrom(y => y.SparePartBannerSettings.Where(x => x.BannerType == Enum.GetName(typeof(BannerType), BannerType.Banner))))
                .ForMember(x => x.Menu, x => x.MapFrom(y => y.SparePartMenuManagement));

            CreateMap<SparePartMenuManagement, WebsiteHeaderMenu>()
                .ForMember(x => x.Position, x => x.MapFrom(y => y.Position))
                .ForMember(x => x.Route, x => x.MapFrom(y => y.SparePartMenu.Route))
                .ForMember(x => x.Title, x => x.MapFrom(y => y.SparePartMenu.Title));
        }


    }
}
