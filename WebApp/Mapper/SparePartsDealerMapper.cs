using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartsDealerMapper : Profile
    {
        public SparePartsDealerMapper()
        {
            CreateMap<SparePartsDealerViewModel, SparePartsDealerDTO>();
            CreateMap<SparePartsDealerDTO, SparePartsDealerViewModel>();

            CreateMap<SparePartsDealerDocumentViewModel, SparePartsDealerDocumentDTO>();
            CreateMap<SparePartsDealerDocumentDTO, SparePartsDealerDocumentViewModel>();


            CreateMap<SparePartsDealerScheduleViewModel, SparePartsDealerScheduleDTO>();
            CreateMap<SparePartsDealerScheduleDTO, SparePartsDealerScheduleViewModel>();

            CreateMap<SparePartsDealerSpecificationsViewModel, SparePartsDealerSpecificationsDTO>();
            CreateMap<SparePartsDealerSpecificationsDTO, SparePartsDealerSpecificationsViewModel>();

            CreateMap<SparePartDealerImagesViewModel, SparePartDealerImagesDTO>();
            CreateMap<SparePartDealerImagesDTO, SparePartDealerImagesViewModel>();
        }
    }
}
