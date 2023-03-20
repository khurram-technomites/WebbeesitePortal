using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CouponRedemptionService : ICouponRedemptionService
    {
        private readonly ICouponRedemptionRepo _repo;

        public CouponRedemptionService(ICouponRedemptionRepo repo)
        {
            _repo = repo;
        }

        public async Task<CouponRedemption> AddCouponRedemption(CouponRedemption Model)
        {
            return await _repo.InsertAsync(Model);
        }
    }
}
