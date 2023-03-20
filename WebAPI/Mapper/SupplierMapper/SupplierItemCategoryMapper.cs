using AutoMapper;
using HelperClasses.DTOs.Supplier;
using WebAPI.Models;

namespace WebAPI.Mapper.SupplierMapper
{
    public class SupplierItemCategoryMapper : Profile
    {
        public SupplierItemCategoryMapper()
        {
            CreateMap<SupplierItemCategoryDTO, SupplierItemCategory>();
            CreateMap<SupplierItemCategory, SupplierItemCategoryDTO>();

        }
    }
}
