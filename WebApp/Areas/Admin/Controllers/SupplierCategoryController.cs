using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, B2B Manager")]
    public class SupplierCategoryController : Controller
    {
        private readonly ISupplierItemCategoryClient _Service;
        private readonly IMapper _mapper;
        [BindProperty]
        public SupplierItemCategoryViewModel Model { get; set; }
        public SupplierCategoryController(ISupplierItemCategoryClient Service, IMapper mapper)
        {
            _mapper = mapper;
            _Service = Service;
        }
        public async Task<IActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<SupplierItemCategoryViewModel>>(await _Service.GetCategories());
            return View(info);
        }
        public async Task<IActionResult> Details(long Id)
        {
            IEnumerable<SupplierItemCategoryViewModel> supplierCategoryModel = _mapper.Map<IEnumerable<SupplierItemCategoryViewModel>>(await _Service.GetCategoryByID(Id));
            return View(supplierCategoryModel.FirstOrDefault());
        }
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(SupplierItemCategoryViewModel model)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    SupplierItemCategoryDTO Result = await _Service.Create(_mapper.Map<SupplierItemCategoryDTO>(model));

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/SupplierCategory/Index",
                        message = "Supplier Category Created Successfully",
                        data = new
                        {
                            CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.Name,
                            ID = Result.Id,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                        }
                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Admin/SupplierCategory/Index",
                        success = false,
                        message = err.Message
                    });
                }
            }

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SupplierItemCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    SupplierItemCategoryDTO Result = await _Service.Edit(_mapper.Map<SupplierItemCategoryDTO>(model));

                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.Name,
                            ID = Result.Id,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
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

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });

        }
        public async Task<IActionResult> Edit(long Id)
        {
            IEnumerable<SupplierItemCategoryViewModel> supplierCategoryModel = _mapper.Map<IEnumerable<SupplierItemCategoryViewModel>>(await _Service.GetCategoryByID(Id));
            return View(supplierCategoryModel.FirstOrDefault());
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _Service.Delete(Id);

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
        public async Task<ActionResult> ToggleActiveStatus(long Id)
        {
            try
            {
                SupplierItemCategoryViewModel Result = _mapper.Map<SupplierItemCategoryViewModel>(await _Service.ToggleActiveStatus(Id));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.Name,
                        ID = Result.Id,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
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
    }
}
