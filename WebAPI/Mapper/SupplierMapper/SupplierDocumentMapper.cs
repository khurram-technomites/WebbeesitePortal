using AutoMapper;
using HelperClasses.DTOs.Supplier;
using WebAPI.Models;

namespace WebAPI.Mapper.SupplierMapper
{
    public class SupplierDocumentMapper : Profile
    {
        public SupplierDocumentMapper()
        {
            CreateMap<SupplierDocument, SupplierDocumentDTO>();
            CreateMap<SupplierDocumentDTO, SupplierDocument>();
        }
    }
}
