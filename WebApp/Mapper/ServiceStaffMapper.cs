using AutoMapper;
using HelperClasses.DTOs.ServiceAndDeliveryStaffDTO;
using HelperClasses.DTOs.ServiceStaff;
using WebAPI.Models;
using WebApp.Areas.Admin.Models;

namespace WebApp.Mapper
{
    public class ServiceStaffMapper : Profile
    {
        public ServiceStaffMapper()
        {
            CreateMap<ServiceStaffDTO, ServiceStaffViewModel>();
            CreateMap<ServiceStaffViewModel, ServiceStaffDTO>();
            CreateMap<ServiceStaff, ServiceStaffDTO>();
            CreateMap<ServiceStaffDTO, ServiceStaff>();
            CreateMap<ServiceStaffRegisterViewModel, ServiceStaffRegisterDTO>();
            CreateMap<ServiceStaffRegisterDTO, ServiceStaffRegisterViewModel>();
            CreateMap<ServiceAndDeliveryStaffRegisterDTO, ServiceAndDeliveryStaffRegisteredViewModel>();
            CreateMap<ServiceAndDeliveryStaffRegisteredViewModel, ServiceAndDeliveryStaffRegisterDTO>();
        }
    }
}
