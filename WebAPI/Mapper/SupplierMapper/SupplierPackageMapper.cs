using AutoMapper;
using HelperClasses.DTOs.Supplier;
using WebAPI.Models;

namespace WebAPI.Mapper.SupplierMapper
{
    public class SupplierPackageMapper : Profile
    {
        public SupplierPackageMapper()
        {
            CreateMap<SupplierPackageDTO, SupplierPackage>();
            CreateMap<SupplierPackage, SupplierPackageDTO>();
        }
    }
}
