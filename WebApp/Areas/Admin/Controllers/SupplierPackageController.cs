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
using WebApp.Interfaces;
using HelperClasses.DTOs.Supplier;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, B2B Manager")]
    public class SupplierPackageController : Controller
    {
        private readonly ISupplierPackageClient _client;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IMapper _mapper;
        public SupplierPackageController(ISupplierPackageClient client, IUserSessionManager userSessionManager
            , IMapper mapper)
        {
            _client = client;
            _userSessionManager = userSessionManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var Info = _mapper.Map<IEnumerable<SupplierPackageViewModel>>(await _client.GetAllAsync());
            return View(Info);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(SupplierPackageViewModel model)
        {
            string message = string.Empty;
            try
            {
                switch (model.BillingPeriod)
                {
                    case "Monthly":
                        model.MonthCount = 1;
                        break;
                    case "Quarterly":
                        model.MonthCount = 3;
                        break;
                    case "Half Yearly":
                        model.MonthCount = 6;
                        break;
                    case "Yearly":
                        model.MonthCount = 12;
                        break;

                    default:
                        break;
                }
                model.Status = Enum.GetName(typeof(Status), Status.Active);
                SupplierPackageDTO Result = await _client.AddSupplierPackageAsync(_mapper.Map<SupplierPackageDTO>(model));

                return Json(new
                {
                    success = true,
                    url = "/Admin/SupplierPackage/Index",
                    message = "Supplier Package Created Successfully",
                    data = new
                    {
                        Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.Name,
                        ID = Result.Id,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                        Price = Result.Price
                    }
                });
            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    url = "/Admin/SupplierPackage/Index",
                    success = false,
                    message = err.Message
                });
            }

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });
        }
        public async Task<IActionResult> Details(long Id)
        {
            var supplierPackage = _mapper.Map<IEnumerable<SupplierPackageViewModel>>(await _client.GetByIdAsync(Id));
            return View(supplierPackage.FirstOrDefault());
        }
        public async Task<ActionResult> ToggleActiveStatus(long Id)
        {
            try
            {
                SupplierPackageViewModel Result = _mapper.Map<SupplierPackageViewModel>(await _client.ToggleActiveStatus(Id));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.Name,
                        ID = Result.Id,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                        Price = Result.Price
                    }

                });
            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                    message = err.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SupplierPackageViewModel model)
        {
            try
            {
                switch (model.IsFree)
                {
                    case true:
                        model.Price = 0;
                        break;
                }
                switch (model.BillingPeriod)
                {
                    case "Monthly":
                        model.MonthCount = 1;
                        break;
                    case "Quarterly":
                        model.MonthCount = 3;
                        break;
                    case "Half Yearly":
                        model.MonthCount = 6;
                        break;
                    case "Yearly":
                        model.MonthCount = 12;
                        break;

                    default:
                        break;
                }

                SupplierPackageDTO Result = await _client.UpdateSupplierPackageAsync(_mapper.Map<SupplierPackageDTO>(model));

                Result.Id = model.Id;

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully",
                    data = new
                    {
                        Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.Name,
                        ID = Result.Id,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                        Price = Result.Price
                    }
                });
            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                    message = err.Message
                });
            }
            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });

        }
        public async Task<IActionResult> Edit(long Id)
        {
            var supplierPackage = _mapper.Map<IEnumerable<SupplierPackageViewModel>>(await _client.GetByIdAsync(Id));
            return View(supplierPackage.FirstOrDefault());
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _client.DeleteSupplierPackageAsync(Id);

                return Json(new
                {
                    success = true,
                    message = "Record Deleted Successfully"
                });
            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                    message = err.Message
                });
            }
        }
    }
}
