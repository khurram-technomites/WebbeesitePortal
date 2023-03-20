using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
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
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Vendors
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Vendor,GarageOwner")]
    public class ModulePurchaseDetailsController : Controller
    {
        private readonly IModulePurchaseDetailsService _modulePurchaseDetailsService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;
        public ModulePurchaseDetailsController(IModulePurchaseDetailsService modulePurchaseDetailsService, IMapper mapper, IFTPUpload fTPUpload)
        {
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _modulePurchaseDetailsService = modulePurchaseDetailsService;
        }


        [HttpGet("PurchaseId/{PurchaseId}")]
        public async Task<IActionResult> GetDetailsByPurchaseId(long PurchaseId)
        {
            return Ok(_mapper.Map<IEnumerable<ModulePurchaseDetailsDTO>>(await _modulePurchaseDetailsService.GetDetailsByPurchaseId(PurchaseId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDetailsByIdAsync(long Id)
        {
            IEnumerable<ModulePurchaseDetailsDTO> PurchaseDetails = _mapper.Map<IEnumerable<ModulePurchaseDetailsDTO>>(await _modulePurchaseDetailsService.GetDetailsByIdAsync(Id));
            return Ok(PurchaseDetails.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Post(ModulePurchaseDetails Model)
        {

            return Ok(_mapper.Map<ModulePurchaseDetailsDTO>(await _modulePurchaseDetailsService.AddAsync(_mapper.Map<ModulePurchaseDetails>(Model))));
        }
        [HttpPost("Range")]
        public async Task<IActionResult> PostRange(List<ModulePurchaseDetailsDTO> Model)
        {

            return Ok(_mapper.Map<List<ModulePurchaseDetailsDTO>>(await _modulePurchaseDetailsService.AddRangeAsync(_mapper.Map<List<ModulePurchaseDetails>>(Model))));
        }
        [HttpPut("Range")]
        public async Task<IActionResult> UpdateRange(List<ModulePurchaseDetailsDTO> Model)
        {

            return Ok(_mapper.Map<List<ModulePurchaseDetailsDTO>>(await _modulePurchaseDetailsService.UpdateRangeAsync(_mapper.Map<List<ModulePurchaseDetails>>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Put(ModulePurchaseDetailsDTO Model)
        {

            //IEnumerable<ModulePurchaseDetailsDTO> List = await _modulePurchaseDetailsService.GetDetailsByIdAsync(Model.Id);
            return Ok(_mapper.Map<ModulePurchaseDetailsDTO>(await _modulePurchaseDetailsService.UpdateAsync(_mapper.Map<ModulePurchaseDetails>(Model))));
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            await _modulePurchaseDetailsService.DeleteAsync(Id);
            return Ok(new SuccessResponse<string>("Package deleted successfully", ""));
          
        }
        [HttpGet("{Id}/ByName/{Name}")]
        public async Task<IActionResult> GetDetailByName(long Id, string Name)
        {
            int Qunatity = 0;
            var modulepurchase = await _modulePurchaseDetailsService.GetDetailsByPurchaseIdandName(Id, Name);
            if(modulepurchase != null && modulepurchase.Count() > 0)
            {
                 Qunatity = (int)modulepurchase.FirstOrDefault().Quantity;
            }
            return Ok(Qunatity);
        }


    }

}
