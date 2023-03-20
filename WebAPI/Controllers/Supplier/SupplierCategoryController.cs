using AutoMapper;
using HelperClasses.Classes;
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

namespace WebAPI.Controllers.Supplier
{
    [Route("api/Supplier")]
    [ApiController]
    public class SupplierCategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISupplierItemCategoryService _supplierItemCategoryService;
        private readonly ISupplierItemService _supplierItemService;


        public SupplierCategoryController(IMapper mapper, ISupplierItemCategoryService supplierItemCategoryService, ISupplierItemService supplierItemService)
        {
            _mapper = mapper;
            _supplierItemCategoryService = supplierItemCategoryService;
            _supplierItemService = supplierItemService;
        }

        [HttpGet("{Id}/Category")]
        public async Task<IActionResult> GetAllCategory(long Id)
        {
            IEnumerable<Models.SupplierItem> Items = await _supplierItemService.GetAllBySupplierId(Id);
            //IEnumerable<Models.SupplierItem> ItemsDist = Items.Distinct();
            List<SupplierItemCategoryDTO> categoryDTO = new();


            foreach (var item in Items)
            {
                if (!categoryDTO.Any(x => x.Name == item.Category.Name))
                {
                    SupplierItemCategoryDTO dTO = new()
                    {
                        Id = item.CategoryId,
                        Name = item.Category.Name
                    };

                    categoryDTO.Add(dTO);
                }

            }

            return Ok(categoryDTO);
        }


        [HttpGet("Category")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<SupplierItemCategoryDTO>>(await _supplierItemCategoryService.GetAllAsync()));
        }


        [HttpGet("Category/By/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SupplierItemCategoryDTO>>(await _supplierItemCategoryService.GetByIdAsync(Id)));
        }

        [HttpPost("Category")]
        public async Task<IActionResult> Post(SupplierItemCategoryDTO Model)
        {

            Model.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<SupplierItemCategoryDTO>(await _supplierItemCategoryService.AddSupplierItemCategoryAsync(_mapper.Map<SupplierItemCategory>(Model))));
        }

        [HttpPut("Category")]
        public async Task<IActionResult> Put(SupplierItemCategoryDTO Model)
        {
            IEnumerable<SupplierItemCategory> List = await _supplierItemCategoryService.GetByIdAsync(Model.Id);
            SupplierItemCategory supplierItem = List.FirstOrDefault();
            supplierItem = _mapper.Map(Model, supplierItem);

            return Ok(_mapper.Map<SupplierItemCategoryDTO>(await _supplierItemCategoryService.UpdateSupplierItemCategoryAsync(_mapper.Map<SupplierItemCategory>(supplierItem))));
        }

        [HttpDelete("Category/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<SupplierItemCategoryDTO>(await _supplierItemCategoryService.ArchiveSupplierItemCategoryAsync(Id)));
        }

        [HttpGet("Category/ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<SupplierItemCategory> cityList = await _supplierItemCategoryService.GetByIdAsync(Id);
            SupplierItemCategory supplierCategory = cityList.FirstOrDefault();

            if (supplierCategory.Status == Enum.GetName(typeof(Status), Status.Active))
                supplierCategory.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                supplierCategory.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<SupplierItemCategoryDTO>(await _supplierItemCategoryService.UpdateSupplierItemCategoryAsync(supplierCategory)));
        }
    }
}
