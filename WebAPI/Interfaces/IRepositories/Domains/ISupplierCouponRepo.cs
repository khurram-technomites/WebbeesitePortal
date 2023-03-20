using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface ISupplierCouponRepo : IRepository<SupplierCoupon>
    {
        Task<IEnumerable<SupplierCoupon>> GetCouponByCustomer(long supllierId, long CustomerId);
    }
}
