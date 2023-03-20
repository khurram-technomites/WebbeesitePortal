using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Linq;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.Mapper
{
	public class RestaurantCustomerMapper : Profile
	{
		public RestaurantCustomerMapper()
		{
			CreateMap<RestaurantCustomer, RestaurantCustomerDTO>()
			.AfterMap((src, dest) =>
			{
				if (src.RestaurantBranch != null && src.RestaurantBranch.Latitude != 0 && src.RestaurantBranch.Longitude != 0)
					foreach (var item in dest.Customer.CustomerAddresses)
						item.Distance = DistanceHelper.DistanceTo((double)src.RestaurantBranch.Latitude, (double)src.RestaurantBranch.Longitude, (double)item.Latitude, (double)item.Longitude);

				foreach (string name in Enum.GetNames(typeof(CustomerAddressStatus)))
					if (dest != null && dest.Customer != null && dest.Customer.CustomerAddresses.FirstOrDefault(x => x.Type == name) == null)
						dest.Customer.CustomerAddresses.Add(GetEmptyDtoOfAddress(name));

				if (dest != null && dest.Customer != null && dest.Customer.CustomerAddresses.Count > 0)
					dest.Customer.CustomerAddresses = dest.Customer.CustomerAddresses.OrderBy(x => x.Type).ToList();
			})
			;
			CreateMap<RestaurantCustomerDTO, RestaurantCustomer>();
		}

		private CustomerAddressDTO GetEmptyDtoOfAddress(string type)
		{
			CustomerAddressDTO customerAddress = new()
			{
				Type = type,
				Address = "empty",
			};

			return customerAddress;
		}
	}
}
