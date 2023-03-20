using AutoMapper;
using HelperClasses.DTOs.DeliveryStaff;
using HelperClasses.DTOs.ServiceAndDeliveryStaffDTO;
using WebAPI.Models;
using WebApp.Areas.Admin.Models;
using WebApp.Areas.Admin.Models.DeliveryStaff;

namespace WebApp.Mapper
{
    public class DeliveryStaffMapper : Profile
    {
        public DeliveryStaffMapper()
        {
            CreateMap<DeliveryStaffDTO, DeliveryStaffViewModel>();
            CreateMap<DeliveryStaffViewModel, DeliveryStaffDTO>();
            CreateMap<DeliveryStaff, DeliveryStaffDTO>();
            CreateMap<DeliveryStaffDTO, DeliveryStaff>();
            CreateMap<DeliveryStaffRegisterViewModel, DeliveryStaffRegisterDTO>();
            CreateMap<DeliveryStaffRegisterDTO, DeliveryStaffRegisterViewModel>();
            CreateMap<ServiceAndDeliveryStaffRegisterDTO, ServiceAndDeliveryStaffRegisteredViewModel>();
            CreateMap<ServiceAndDeliveryStaffRegisteredViewModel, ServiceAndDeliveryStaffRegisterDTO>();
        }
     
    }
}
