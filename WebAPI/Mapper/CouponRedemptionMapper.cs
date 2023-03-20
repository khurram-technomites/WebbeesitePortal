using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class CouponRedemptionMapper : Profile
    {
        public CouponRedemptionMapper()
        {
            CreateMap<CouponRedemption, CouponRedemptionDTO>();
            CreateMap<CouponRedemptionDTO, CouponRedemption>();
        }
    }
}
