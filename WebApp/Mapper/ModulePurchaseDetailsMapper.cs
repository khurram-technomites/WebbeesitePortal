using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebAPI.Mapper
{
    public class ModulePurchaseDetailsMapper:Profile
    {
        public ModulePurchaseDetailsMapper()
        {
            CreateMap<ModulePurchaseDetailsViewModel, ModulePurchaseDetailsDTO>();
            CreateMap<ModulePurchaseDetailsDTO, ModulePurchaseDetailsViewModel>();
        }
    }
}
