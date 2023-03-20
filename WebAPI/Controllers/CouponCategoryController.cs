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
    public class CouponCategoryController : ControllerBase
    {
        private readonly ICouponCategoryService _service;
        private readonly IMapper _mapper;
        public CouponCategoryController(ICouponCategoryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("Coupons/{CouponId}")]
        public async Task<IActionResult> GetAll(long CouponId)
        {
            IEnumerable<CouponCategoryDTO> CouponCategory = _mapper.Map<IEnumerable<CouponCategoryDTO>>(await _service.GetCouponCategoriesByCoupon(CouponId));
            return Ok(CouponCategory);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<CouponCategoryDTO> List = _mapper.Map<IEnumerable<CouponCategoryDTO>>(await _service.GetCouponCategory(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpGet("Coupons/{CouponId}/Categories/{CategoryId}")]
        public async Task<IActionResult> GetCouponCategoryByCouponAndCategoryId(long CouponId , long CategoryId)
        {
            IEnumerable<CouponCategoryDTO> List = _mapper.Map<IEnumerable<CouponCategoryDTO>>(await _service.GetCouponCategoryByCouponAndCategoryId(CouponId , CategoryId));
            return Ok(List.FirstOrDefault());
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetByCouponCategory()
        {
            IEnumerable<CouponDTO> List = _mapper.Map<IEnumerable<CouponDTO>>(await _service.GetCouponCategories());
            return Ok(List);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CouponCategoryDTO Model)
        {

            return Ok(_mapper.Map<CouponCategoryDTO>(await _service.AddCouponCategoryAsync(_mapper.Map<CouponCategory>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CouponCategoryDTO Model)
        {
            return Ok(_mapper.Map<CouponCategoryDTO>(await _service.UpdateCouponCategoryAsync(_mapper.Map<CouponCategory>(Model))));
        }

        [HttpDelete("{Id}")]
        public IActionResult Archive(long Id)
        {
            return Ok(_service.ArchiveCouponCategoryAsync(Id));
        }
    }
}
