using WebAPI.Models;
using HelperClasses.DTOs;
using AutoMapper;
namespace WebAPI.Mapper
{
    public class VendorMapper:Profile
    {
        public VendorMapper()
        {
            CreateMap<Vendor, VendorDTO>();
            CreateMap<VendorDTO, Vendor>();
        }
    }
}
