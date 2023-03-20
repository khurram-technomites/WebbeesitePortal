using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageDocumentViewMapper : Profile
    {
        public GarageDocumentViewMapper()
        {
                CreateMap<GarageDocumentViewModel, GarageDocumentDTO>();
                CreateMap<GarageDocumentDTO, GarageDocumentViewModel>();
        }
    }
}
