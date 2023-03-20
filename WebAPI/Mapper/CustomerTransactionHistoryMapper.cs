using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class CustomerTransactionHistoryMapper : Profile
    {
        public CustomerTransactionHistoryMapper()
        {
            CreateMap<CustomerTransactionHistory, CustomerTransactionHistoryDTO>()
                .ForMember(x => x.RestaurantName, x => x.MapFrom(y => y.Order.Restaurant.NameAsPerTradeLicense));
            CreateMap<CustomerTransactionHistoryDTO, CustomerTransactionHistory>()
                 .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
