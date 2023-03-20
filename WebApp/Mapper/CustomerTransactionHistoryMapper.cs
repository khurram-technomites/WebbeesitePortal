using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Admin.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class CustomerTransactionHistoryMapper : Profile
    {
        public CustomerTransactionHistoryMapper()
        {
            CreateMap<CustomerTransactionHistoryDTO, CustomerTransactionHistoryViewModel>();
            CreateMap<CustomerTransactionHistoryViewModel, CustomerTransactionHistoryDTO>();
        }
    }
}
