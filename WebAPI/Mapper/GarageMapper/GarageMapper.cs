using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageMapper
{
    public class GarageMapper : Profile
    {
        public GarageMapper()
        {
            CreateMap<GarageDTO, Garage>()
                .ForMember(x => x.IsServicesAllowed, act => act.Ignore())
                .ForMember(x => x.IsBlogsAllowed, act => act.Ignore())
                .ForMember(x => x.IsAppoinmnetsAllowed, act => act.Ignore())
                .ForMember(x => x.IsTeamsAllowed, act => act.Ignore())
                .ForMember(x => x.IsFeedbackAllowed, act => act.Ignore())
                .ForMember(x => x.IsCareersAllowed, act => act.Ignore())
                //.ForMember(x => x.IsDomainRequired, act => act.Ignore())
                .ForMember(x => x.IsExpertisAllowed, act => act.Ignore())
                .ForMember(x => x.IsPartnerAllowed, act => act.Ignore())
                .ForMember(x => x.IsProjectAllowed, act => act.Ignore())
                .ForMember(x => x.IsTestimonialAllowed, act => act.Ignore())
                .ForMember(x => x.IsAwardAllowed, act => act.Ignore())
                .ForMember(x => x.IsCustomerAppoinmentAllowed, act => act.Ignore())
                .ForMember(x => x.IsMenusAllowed, act => act.Ignore())
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));


            CreateMap<Garage, GarageDTO>()
                .ForMember(x => x.AvgRating, x => x.MapFrom(y => y.GarageRatings.DefaultIfEmpty().Average(z => z.Rating)))
                .ForMember(x => x.RatingCount, x => x.MapFrom(y => y.GarageRatings.Count))
                .ForMember(x => x.Logo, x => x.MapFrom(y => y.Logo.Replace(" ", "%20")))
                .ForMember(x => x.PhoneNumber, x => x.MapFrom(y => y.User.PhoneNumber));
        }
    }
}
