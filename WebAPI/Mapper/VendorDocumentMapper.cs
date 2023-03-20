using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class VendorDocumentMapper : Profile
    {
        public VendorDocumentMapper()
        {
            CreateMap<VendorDocument, VendorDocumentDTO>();
            CreateMap<VendorDocumentDTO, VendorDocument>();
        }
    }
}
