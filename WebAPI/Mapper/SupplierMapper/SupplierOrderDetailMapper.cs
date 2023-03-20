using AutoMapper;
using HelperClasses.DTOs.Supplier;
using WebAPI.Models;

namespace WebAPI.Mapper.SupplierMapper
{
    public class SupplierOrderDetailMapper : Profile
    {
        public SupplierOrderDetailMapper()
        {
            CreateMap<SupplierOrderDetailDTO, SupplierOrderDetail>();
            CreateMap<SupplierOrderDetail, SupplierOrderDetailDTO>();
        }
    }
}
