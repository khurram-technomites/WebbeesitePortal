using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _service;
        private readonly ICustomerCouponService _customerCoupon;
        private readonly IMapper _mapper;
        private readonly IFTPUpload _fTPUpload;

        public CouponController(ICouponService service, IMapper mapper, IFTPUpload fTPUpload , ICustomerCouponService customerCoupon)
        {
            _service = service;
            _customerCoupon = customerCoupon;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("GetAll/{restaurantId}")]
        public async Task<IActionResult> GetAll(long restaurantId)

        {
            return Ok(_mapper.Map<IEnumerable<CouponDTO>>(await _service.GetAllAsync(restaurantId)));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<CouponDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("GetAllAdmin")]
        public async Task<IActionResult> GetAdminAll()
        {
            return Ok(_mapper.Map<IEnumerable<CouponDTO>>(await _service.GetAllAdminAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<CouponDTO> List = _mapper.Map<IEnumerable<CouponDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Add(CouponDTO Model)
        {
            string LogoPath = "/Images/Restaurant/Coupon/";
            if (!string.IsNullOrEmpty(Model.CoverImage))
            {
                if (_fTPUpload.MoveFile(Model.CoverImage, ref LogoPath))
                {
                    Model.CoverImage = LogoPath;
                }
            }

            return Ok(_mapper.Map<CouponDTO>(await _service.AddCouponAsync(_mapper.Map<Coupon>(Model))));
        }

        [HttpPost("ValidateCoupon")]
        public async Task<IActionResult> ValidateCoupon(ValidateCouponDTO Model)
        {
            string message = string.Empty;
            object obj = new object();
            if (!string.IsNullOrEmpty(Model.CouponCode))
            {
                IEnumerable<Coupon> coupons = await _service.GetByCodeAsync(Model.CouponCode);

                if (coupons.Any())
                {
                    if (coupons.FirstOrDefault().RestaurantId == coupons.FirstOrDefault().RestaurantId)
                    {
                        if (coupons.FirstOrDefault().IsOpenToAll)
                        {
                            obj = new
                            {
                                success = true,
                            };
                            message = "Coupon is valid!";
                        }

                        else
                        {
                            IEnumerable<CustomerCoupon> customerCoupon = await _customerCoupon.GetCoupon(coupons.FirstOrDefault().Id, (long)Model.CustomerId);

                            if (customerCoupon.Any())
                            {
                                obj = new
                                {
                                    success = true,
                                };
                                message = "Coupon is valid!";
                            }
                            else
                            {
                                obj = new
                                {
                                    success = false,
                                };
                                message = "Coupon is not valid!";
                            }
                        }
                        
                    }

                    else
                    {
                        obj = new
                        {
                            success = false,
                        };
                        message = "Coupon is not valid!";
                    }
                }

                else
                {
                    obj = new
                    {
                        success = false,
                    };
                    message = "Coupon is not valid!";
                }
            }

            else
            {
                obj = new {
                    success = false,
                };
                message = "Please enter coupon code first!";
            }
             
            

            return Ok(new SuccessResponse<object>(message, obj));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CouponDTO Model)
        {
            return Ok(_mapper.Map<CouponDTO>(await _service.UpdateCouponAsync(_mapper.Map<Coupon>(Model))));
        }

        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<Coupon> Coupon = await _service.GetByIdAsync(Id);
            Coupon make = Coupon.FirstOrDefault();

            if (make.Status == Enum.GetName(typeof(Status), Status.Active))
                make.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                make.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _service.UpdateCouponAsync(make));
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<CouponDTO>(await _service.ArchiveCouponAsync(Id)));
        }
    }
}
