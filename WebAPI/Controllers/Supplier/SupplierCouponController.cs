using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/Supplier/Coupon")]
    [ApiController]
    [Authorize]
    public class SupplierCouponController : ControllerBase
    {
        private readonly ISupplierCouponService _service;
        private readonly IMapper _mapper;
        private readonly IFTPUpload _fTPUpload;

        public SupplierCouponController(ISupplierCouponService service, IMapper mapper, IFTPUpload fTPUpload)
        {
            _service = service;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("GetAll/{supplierId}")]
        public async Task<IActionResult> GetAll(long supplierId)

        {
            return Ok(_mapper.Map<IEnumerable<SupplierCouponDTO>>(await _service.GetAllAsync(supplierId)));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<SupplierCouponDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("GetAllAdmin")]
        public async Task<IActionResult> GetAdminAll()
        {
            return Ok(_mapper.Map<IEnumerable<SupplierCouponDTO>>(await _service.GetAllAdminAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<SupplierCouponDTO> List = _mapper.Map<IEnumerable<SupplierCouponDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Add(SupplierCouponDTO Model)
        {
            string LogoPath = "/Images/Supplier/Coupon/";
            if (!string.IsNullOrEmpty(Model.CoverImage))
            {
                if (_fTPUpload.MoveFile(Model.CoverImage, ref LogoPath))
                {
                    Model.CoverImage = LogoPath;
                }
            }

            return Ok(_mapper.Map<SupplierCouponDTO>(await _service.AddCouponAsync(_mapper.Map<SupplierCoupon>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(SupplierCouponDTO Model)
        {
            return Ok(_mapper.Map<SupplierCouponDTO>(await _service.UpdateCouponAsync(_mapper.Map<SupplierCoupon>(Model))));
        }

        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<SupplierCoupon> Coupon = await _service.GetByIdAsync(Id);
            SupplierCoupon make = Coupon.FirstOrDefault();

            if (make.Status == Enum.GetName(typeof(Status), Status.Active))
                make.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                make.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _service.UpdateCouponAsync(make));
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<SupplierCouponDTO>(await _service.ArchiveCouponAsync(Id)));
        }
    }
}
