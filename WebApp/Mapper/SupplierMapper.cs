using AutoMapper;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SupplierMapper : Profile
    {
        public SupplierMapper()
        {
            CreateMap<SupplierDTO, SupplierViewModel>();
            CreateMap<SupplierViewModel, SupplierDTO>();
            CreateMap<SupplierCardDTO, SupplierCardViewModel>();
            CreateMap<SupplierCardViewModel, SupplierCardDTO>();
            CreateMap<SupplierDocumentDTO, SupplierDocumentViewModel>();
            CreateMap<SupplierDocumentViewModel, SupplierDocumentDTO>();

            CreateMap<SupplierDashboardDTO, SupplierDashboardViewModel>();
            CreateMap<SupplierDashboardViewModel, SupplierDashboardDTO>();

            CreateMap<SupplierItemImageDTO, SupplierItemImagesViewModel>();
            CreateMap<SupplierItemImagesViewModel, SupplierItemImageDTO>();

            CreateMap<SupplierPackageDTO, SupplierPackageViewModel>();
            CreateMap<SupplierPackageViewModel, SupplierPackageDTO>();
            CreateMap<SupplierItemViewModel, SupplierItemDTO>();
            CreateMap<SupplierItemDTO, SupplierItemViewModel>()

             .ForMember(x => x.DiscountPercentage, x => x.MapFrom(y => 100 - (y.SalePrice / y.RegularPrice * 100)));
            CreateMap<SupplierItemCategoryDTO, SupplierItemCategoryViewModel>();
            CreateMap<SupplierItemCategoryViewModel, SupplierItemCategoryDTO>();
        }

    }
}
