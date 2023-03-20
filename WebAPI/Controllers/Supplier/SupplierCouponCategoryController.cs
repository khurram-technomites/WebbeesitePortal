using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Supplier
{
    [Route("api/SupplierCouponCategory")]
    [ApiController]
    public class SupplierCouponCategoryController : ControllerBase
    {
        private readonly ISupplierCouponCategoryService _service;
        private readonly IMapper _mapper;
        public SupplierCouponCategoryController(ISupplierCouponCategoryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("Coupons/{CouponId}")]
        public async Task<IActionResult> GetAll(long CouponId)
        {
            IEnumerable<SupplierCouponCategoryDTO> SupplierCouponCategory = _mapper.Map<IEnumerable<SupplierCouponCategoryDTO>>(await _service.GetCouponCategoriesByCoupon(CouponId));
            return Ok(SupplierCouponCategory);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<SupplierCouponCategoryDTO> List = _mapper.Map<IEnumerable<SupplierCouponCategoryDTO>>(await _service.GetSupplierCouponCategory(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpGet("Coupons/{CouponId}/Categories/{CategoryId}")]
        public async Task<IActionResult> GetSupplierCouponCategoryByCouponAndCategoryId(long CouponId, long CategoryId)
        {
            IEnumerable<SupplierCouponCategoryDTO> List = _mapper.Map<IEnumerable<SupplierCouponCategoryDTO>>(await _service.GetSupplierCouponCategoryByCouponAndCategoryId(CouponId, CategoryId));
            return Ok(List.FirstOrDefault());
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetBySupplierCouponCategory()
        {
            IEnumerable<SupplierCouponDTO> List = _mapper.Map<IEnumerable<SupplierCouponDTO>>(await _service.GetCouponCategories());
            return Ok(List);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SupplierCouponCategoryDTO Model)
        {

            return Ok(_mapper.Map<SupplierCouponCategoryDTO>(await _service.AddSupplierCouponCategoryAsync(_mapper.Map<SupplierCouponCategory>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(SupplierCouponCategoryDTO Model)
        {
            return Ok(_mapper.Map<SupplierCouponCategoryDTO>(await _service.UpdateSupplierCouponCategoryAsync(_mapper.Map<SupplierCouponCategory>(Model))));
        }

        [HttpDelete("{Id}")]
        public IActionResult Archive(long Id)
        {
            return Ok(_service.ArchiveSupplierCouponCategoryAsync(Id));
        }
    }
}
