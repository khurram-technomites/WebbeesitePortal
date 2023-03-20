using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelperClasses.DTOs;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;
using Fingers10.ExcelExport.ActionResults;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Handlers;
using Microsoft.AspNetCore.Authorization;
using HelperClasses.Classes;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CustomerTransactionHistoryController : Controller
    {
        private readonly ICustomerTransactionHistoryClient _transactionService;
        private readonly IMapper _mapper;
        [BindProperty]
        public CustomerTransactionHistoryViewModel Model { get; set; }
        public CustomerTransactionHistoryController(ICustomerTransactionHistoryClient transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<CustomerTransactionHistoryViewModel>>(await _transactionService.GetAllTransactionsAsync());
            return View(info);
        }
    }
}
