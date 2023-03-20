using AutoMapper;
using HelperClasses.DTOs.Supplier;
using WebAPI.Models;

namespace WebAPI.Mapper.SupplierMapper
{
    public class SupplierMapper : Profile
    {
        public SupplierMapper()
        {
            CreateMap<Supplier, SupplierDTO>();
            CreateMap<SupplierDTO, Supplier>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
