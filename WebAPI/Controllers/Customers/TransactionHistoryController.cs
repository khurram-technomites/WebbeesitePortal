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

namespace WebAPI.Controllers.Customers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TransactionHistoryController : ControllerBase
    {
        private readonly ICustomerTransactionHistoryService _transactionService;
        private readonly IMapper _mapper;
        public TransactionHistoryController(ICustomerTransactionHistoryService transactionService, IMapper mapper)
        {
            _mapper = mapper;
            _transactionService = transactionService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var transactions = _mapper.Map<IEnumerable<CustomerTransactionHistoryDTO>>(await _transactionService.GetAllTransactionsAsync());
            return Ok(transactions);
        }
        [HttpGet("ByOrder/{orderId}")]
        public async Task<IActionResult> GetTransactionsByOrderIdAsync(long orderId)
        {
            return Ok(_mapper.Map<IEnumerable<CustomerTransactionHistoryDTO>>(await _transactionService.GetTransactionsByOrderIdAsync(orderId)));
        }
        [HttpGet("ByCustomer/{customerId}")]
        public async Task<IActionResult> GetTransactionsByCustomerIdAsync(long customerId)
        {
            return Ok(_mapper.Map<IEnumerable<CustomerTransactionHistoryDTO>>(await _transactionService.GetTransactionsByCustomerIdAsync(customerId)));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetTransactionsByIdAsync(long Id)
        {
            IEnumerable<CustomerTransactionHistoryDTO> cities = _mapper.Map<IEnumerable<CustomerTransactionHistoryDTO>>(await _transactionService.GetTransactionsByIdAsync(Id));
            return Ok(cities.FirstOrDefault());
        }
    }  
}
