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
    public class WebsitePromoSectionResponseDTOMapper : Profile
    {
        public WebsitePromoSectionResponseDTOMapper()
        {
            CreateMap<Garage, WebsitePromoSectionResponseDTO>()
                .ForMember(x => x.PromoBanners, x => x.MapFrom(y => y.GarageBannerSettings.Where(x => x.BannerType == Enum.GetName(typeof(BannerType), BannerType.PromotionBanner))))
                .ForMember(x => x.FirstSection, x => x.MapFrom(y => y.GarageContentManagement.PromoSection01))
                .ForMember(x => x.FirstSectionCount, x => x.MapFrom(y => y.GarageContentManagement.PromoSection01Count))
                .ForMember(x => x.SecondSection, x => x.MapFrom(y => y.GarageContentManagement.PromoSection02))
                .ForMember(x => x.SecondSectionCount, x => x.MapFrom(y => y.GarageContentManagement.PromoSection02Count))
                .ForMember(x => x.ThirdSection, x => x.MapFrom(y => y.GarageContentManagement.PromoSection03))
                .ForMember(x => x.ThirdSectionCount, x => x.MapFrom(y => y.GarageContentManagement.PromoSection03Count));

            CreateMap<SparePartsDealer, WebsitePromoSectionResponseDTO>()
                .ForMember(x => x.PromoBanners, x => x.MapFrom(y => y.SparePartBannerSettings.Where(x => x.BannerType == Enum.GetName(typeof(BannerType), BannerType.PromotionBanner))))
                .ForMember(x => x.FirstSection, x => x.MapFrom(y => y.SparePartContentManagement.PromoSection01))
                .ForMember(x => x.FirstSectionCount, x => x.MapFrom(y => y.SparePartContentManagement.PromoSection01Count))
                .ForMember(x => x.SecondSection, x => x.MapFrom(y => y.SparePartContentManagement.PromoSection02))
                .ForMember(x => x.SecondSectionCount, x => x.MapFrom(y => y.SparePartContentManagement.PromoSection02Count))
                .ForMember(x => x.ThirdSection, x => x.MapFrom(y => y.SparePartContentManagement.PromoSection03))
                .ForMember(x => x.ThirdSectionCount, x => x.MapFrom(y => y.SparePartContentManagement.PromoSection03Count));
        }
    }
}
