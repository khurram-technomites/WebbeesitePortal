using AutoMapper;
using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartsDealerMapper
{
    public class SparePartDealerImagesDTOMapper : Profile
    {
        public SparePartDealerImagesDTOMapper()
        {
            CreateMap<DealerImage, SparePartDealerImagesDTO>();
            CreateMap<SparePartDealerImagesDTO, DealerImage>();
        }
    }
}
