using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Supplier
{
    [Route("api/Supplier/Package")]
    [ApiController]
    public class SupplierPackageController : ControllerBase
    {

        private readonly ISupplierPackageService _SupplierPackageService;
        private readonly IMapper _mapper;

        public SupplierPackageController(ISupplierPackageService supplierPackageService, IMapper mapper)
        {
            _SupplierPackageService = supplierPackageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<SupplierPackageDTO>>(await _SupplierPackageService.GetAllAsync()));
        }

        [HttpGet("GetBy/{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            IEnumerable<SupplierPackageDTO> cities = _mapper.Map<IEnumerable<SupplierPackageDTO>>(await _SupplierPackageService.GetByIdAsync(Id));
            return Ok(cities);
        }

        [HttpPost]
        public async Task<IActionResult> AddPackage(SupplierPackageDTO Model)
        {
            return Ok(_mapper.Map<SupplierPackageDTO>(await _SupplierPackageService.AddSupplierPackageAsync(_mapper.Map<SupplierPackage>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePakage(SupplierPackageDTO Model)
        {
            return Ok(new SuccessResponse<SupplierPackageDTO>("", _mapper.Map<SupplierPackageDTO>(await _SupplierPackageService.UpdateSupplierPackageAsync(_mapper.Map<SupplierPackage>(Model)))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<SupplierPackageDTO>(await _SupplierPackageService.ArchiveSupplierPackageAsync(Id)));
        }
        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<SupplierPackage> supplierPackagelist = await _SupplierPackageService.GetByIdAsync(Id);
            SupplierPackage supplierPackage = supplierPackagelist.FirstOrDefault();

            if (supplierPackage.Status == Enum.GetName(typeof(Status), Status.Active))
                supplierPackage.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                supplierPackage.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<SupplierPackageDTO>(await _SupplierPackageService.UpdateSupplierPackageAsync(supplierPackage)));
        }
    }
}
