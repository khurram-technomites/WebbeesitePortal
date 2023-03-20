using AutoMapper;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SupplierPackageMapper : Profile
    {
        public SupplierPackageMapper()
        {
            CreateMap<SupplierPackageDTO, SupplierPackageViewModel>();
            CreateMap<SupplierPackageViewModel, SupplierPackageDTO>();
        }
    }
}
