using AutoMapper;
using HelperClasses.DTOs.Supplier;
using WebAPI.Models;

namespace WebAPI.Mapper.SupplierMapper
{
    public class SupplierItemImageMapper : Profile
    {
        public SupplierItemImageMapper()
        {
            CreateMap<SupplierItemImageDTO, SupplierItemImage>();
            CreateMap<SupplierItemImage, SupplierItemImageDTO>();
        }
    }
}
