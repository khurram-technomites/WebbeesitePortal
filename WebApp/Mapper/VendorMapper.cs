using AutoMapper;
using HelperClasses.DTOs;
using WebApp.ViewModels;
namespace WebApp.Mapper
{
    public class VendorMapper:Profile
    {
        public VendorMapper()
        {
            CreateMap<VendorDTO, VendorViewModel>();
            CreateMap<VendorViewModel, VendorDTO>();
        }
    }
}
