using AutoMapper;
using HelperClasses.DTOs;
using WebApp.ViewModels;
namespace WebApp.Mapper
{
    public class VendorDocumentMapper : Profile
    {
        public VendorDocumentMapper()
        {
            CreateMap<VendorDocumentViewModel, VendorDocumentDTO>();
            CreateMap<VendorDocumentDTO, VendorDocumentViewModel>();
        }
    }
}
