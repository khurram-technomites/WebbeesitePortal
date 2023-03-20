using AutoMapper;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SupplierOrderMapper : Profile
    {
        public SupplierOrderMapper()
        {
            CreateMap<SupplierOrderDTO, SupplierOrderViewModel>();
            CreateMap<SupplierOrderViewModel, SupplierOrderDTO>();
            CreateMap<SupplierOrderDetailDTO, SupplierOrderDetailViewModel>();
            CreateMap<SupplierOrderDetailViewModel, SupplierOrderDetailDTO>();
            CreateMap<SupplierOrderPlacementDTO, SupplierOrderPlacementViewModel>();
            CreateMap<SupplierOrderPlacementViewModel, SupplierOrderPlacementDTO>();
            CreateMap<SupplierOrderItemsDTO, SupplierOrderItemsViewModel>();
            CreateMap<SupplierOrderItemsViewModel, SupplierOrderItemsDTO>();
        }
    }
}
