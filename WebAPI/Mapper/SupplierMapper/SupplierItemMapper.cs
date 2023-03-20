using AutoMapper;
using HelperClasses.DTOs.Supplier;
using WebAPI.Models;

namespace WebAPI.Mapper.SupplierMapper
{
    public class SupplierItemMapper : Profile
    {
        public SupplierItemMapper()
        {
            CreateMap<SupplierItemDTO, SupplierItem>();
            CreateMap<SupplierItem, SupplierItemDTO>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
