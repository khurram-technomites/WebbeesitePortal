using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Supplier.Controllers
{
    [Area("Supplier")]
    [Authorize(Roles = "Supplier")]
    public class ItemController : Controller
    {
        private readonly ISupplierItemClient _supplierItemClient;
        private readonly ISupplierCategoryClient _supplierCategoryClient;
        private readonly IUserSessionManager _sessionManager;
        private readonly IMapper _mapper;

        public ItemController(ISupplierItemClient supplierItemClient, IUserSessionManager sessionManager, ISupplierCategoryClient supplierCategoryClient, IMapper mapper)
        {
            _supplierItemClient = supplierItemClient;
            _sessionManager = sessionManager;
            _supplierCategoryClient = supplierCategoryClient;

            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            long supplierId = _sessionManager.GetSupplierStore().Id;
            var Supplier = await _supplierItemClient.GetBySupplierAsync(supplierId);
            return View(Supplier);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = await _supplierCategoryClient.GetAllAsync();
            return View(new SupplierItemViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SupplierItemViewModel Model)
        {

           
            var supplierId = _sessionManager.GetSupplierStore().Id;
            Model.SupplierId = supplierId;
            var result = await _supplierItemClient.AddAsync(_mapper.Map<SupplierItemDTO>(Model));

            return Json(new
            {
                success = true,
                message = "Item added successfully",
                data = result.Id
            });
        }

        public async Task<IActionResult> Edit(long Id)
        {
            ViewBag.CategoryId = await _supplierCategoryClient.GetAllAsync();
            var item = await _supplierItemClient.GetByIdAsync(Id);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SupplierItemViewModel Model)
        {
            var result = await _supplierItemClient.UpdateAsync(_mapper.Map<SupplierItemDTO>(Model));

            return Json(new
            {
                success = true,
                message = "Item updated successfully",
                data = result.Id
            });
        }

        public async Task<IActionResult> Details(long Id)
        {
            ViewBag.CategoryId = await _supplierCategoryClient.GetAllAsync();
            var details = _mapper.Map<SupplierItemViewModel>(await _supplierItemClient.GetByIdAsync(Id));
            return View(details);
        }

        public async Task<IActionResult> Delete(long Id)
        {
            var result = await _supplierItemClient.ArchiveAsync(Id);

            return Json(new
            {
                success = true,
                message = "Item deleted successfully",
                data = result.Id
            });
        }

        public async Task<IActionResult> ToggleStatus(long Id)
        {
            var result = await _supplierItemClient.ToggleActiveStatusAsync(Id);

            return Json(new
            {
                success = true,
                message = "Status changed successfully",
                data = result.Id
            });
        }
    }
}
