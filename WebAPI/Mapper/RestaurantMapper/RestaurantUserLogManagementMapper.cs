using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
	public class RestaurantUserLogManagementMapper : Profile
	{
		public RestaurantUserLogManagementMapper()
		{
			CreateMap<RestaurantUserLogManagement, RestaurantUserLogManagementDTO>()
				.ForMember(x => x.Email, y => y.MapFrom(x => x.User != null ? x.User.Email : null))
				.ForMember(x => x.UserName, y => y.MapFrom(x => x.User != null ? x.User.FirstName + " " + x.User.LastName : null))
				.ForMember(x => x.PhoneNumber, y => y.MapFrom(x => x.User != null ? x.User.PhoneNumber : null))
				.ForMember(x => x.BranchName, y => y.MapFrom(x => x.RestaurantBranch != null ? x.RestaurantBranch.NameAsPerTradeLicense : null))
				;
			CreateMap<RestaurantUserLogManagementDTO, RestaurantUserLogManagement>();
		}
	}
}
