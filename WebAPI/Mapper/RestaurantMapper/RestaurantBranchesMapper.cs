using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantBranchesMapper : Profile
    {
        public RestaurantBranchesMapper()
        {
            CreateMap<RestaurantBranch, RestaurantBranchDTO>()
                .ForMember(x => x.AvgRating, x => x.MapFrom(y => y.Restaurant.RestaurantRatings.DefaultIfEmpty().Average(z => z.Rating)))
                .ForMember(x => x.RatingCount, x => x.MapFrom(y => y.Restaurant.RestaurantRatings.Count))
                .ForMember(x => x.OrderingPhoneNumber, x => x.MapFrom(y => y.OrderingPhoneNumber.Replace("971", "")));

            CreateMap<RestaurantBranchDTO, RestaurantBranch>()
				.ForAllMembers(x => x.Condition((source, destination, member) => member != null))
                ;
        }
    }
}
