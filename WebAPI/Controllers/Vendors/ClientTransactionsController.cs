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
    //[Authorize(Roles = "Vendor")]
    public class ClientTransactionsController : ControllerBase
    {
        private readonly IClientModulePurchaseTransactionsService _transactionService;
        private readonly IMapper _mapper;
        public ClientTransactionsController(IClientModulePurchaseTransactionsService transactionService, IMapper mapper)
        {
            _mapper = mapper;
            _transactionService = transactionService;
        }
        [HttpGet("ByVendor/{vendorId}")]
        public async Task<IActionResult> GetByVendorIdAsync(long vendorId)
        {
            var transactions = _mapper.Map<IEnumerable<ClientModulePurchaseTransactionsDTO>>(await _transactionService.GetClientModulePurchaseTransactionsByVendorIDAsync(vendorId));
            return Ok(transactions);
        }
    }
}
