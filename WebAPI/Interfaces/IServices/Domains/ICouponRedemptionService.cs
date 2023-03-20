﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICouponRedemptionService
    {
        Task<CouponRedemption> AddCouponRedemption(CouponRedemption Model);
    }
}
