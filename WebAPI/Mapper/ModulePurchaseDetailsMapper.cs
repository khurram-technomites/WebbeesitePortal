using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;
namespace WebAPI.Mapper
{
    public class ModulePurchaseDetailsMapper:Profile
    {
        public ModulePurchaseDetailsMapper()
        {
            CreateMap<ModulePurchaseDetails, ModulePurchaseDetailsDTO>();
            CreateMap<ModulePurchaseDetailsDTO, ModulePurchaseDetails>();
        }
    }
}
