using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerCouponController : ControllerBase
    {
        private readonly ICustomerCouponService _service;
        private readonly IMapper _mapper;
        public CustomerCouponController(ICustomerCouponService service, IMapper mapper )
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("Customers/{CustomerId}/Coupons/{CouponId}")]  
        public async Task<IActionResult> GetAll(long CustomerId , long CouponId)
        {
            IEnumerable<CustomerCouponDTO> customerCoupon = _mapper.Map<IEnumerable<CustomerCouponDTO>>(await _service.GetCoupon(CustomerId, CouponId));
            return Ok(customerCoupon.FirstOrDefault());
        }

        [HttpGet("Coupons/{CouponId}")]
        public async Task<IActionResult> GetById(long CouponId)
        {
            IEnumerable<CustomerCouponDTO> List = _mapper.Map<IEnumerable<CustomerCouponDTO>>(await _service.GetCouponsByCouponID(CouponId));
            return Ok(List);
        }

        [HttpGet("Customers/{CustomerId}")]
        public async Task<IActionResult> GetByCustomerCoupon(long CustomerId)
        {
            IEnumerable<CouponDTO> List = _mapper.Map<IEnumerable<CouponDTO>>(await _service.GetCustomerCoupons(CustomerId));
            return Ok(List);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CustomerCouponDTO Model)
        {

            return Ok(_mapper.Map<CustomerCouponDTO>(await _service.AddCustomerCouponAsync(_mapper.Map<CustomerCoupon>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CustomerCouponDTO Model)
        {
            return Ok(_mapper.Map<CustomerCouponDTO>(await _service.UpdateCustomerCouponAsync(_mapper.Map<CustomerCoupon>(Model))));
        }

        //[HttpDelete("{Id}")]
        //public async Task<IActionResult> Archive(long Id)
        //{
        //    return Ok(_mapper.Map<CustomerCouponDTO>(await _service.ArchiveCustomerCouponAsync(Id)));
        //} uzaif
        [HttpDelete("{Id}")]
        public IActionResult CustomerCoupon(long Id)
        {
            return Ok(_service.DeleteCustomerCouponAsync(Id));
        }

    }
}
