using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.ServiceAndDeliveryStaffDTO;
using HelperClasses.DTOs.ServiceStaff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Areas.Admin.Models;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class ServiceStaffController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServiceStaffClient _client;


        public ServiceStaffController(IMapper mapper, IServiceStaffClient client)
        {
            _mapper = mapper;
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            PagingParameters paging = new PagingParameters();
            paging.PageNumber = 1;
            paging.PageSize = 10;
            var Info = _mapper.Map<IEnumerable<ServiceStaffViewModel>>(await _client.GetAllServiceStaffsAsync(paging));
            return View(Info);
        }

        public async Task<IActionResult> Create()
        {
            return View(new ServiceStaffRegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceStaffRegisterViewModel model)
        {
            try
            {
                if (model.Email == "admin@fougito.com")
                    return Json(new { success = false, message = "Email cannot be admin@fougito.com" });

                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    ServiceAndDeliveryStaffRegisteredViewModel registerViewModel = new ServiceAndDeliveryStaffRegisteredViewModel();
                    registerViewModel.RegisteringFor = Enum.GetName(typeof(Roles), Roles.ServiceStaff);
                    ServiceStaffViewModel viewModel = new ServiceStaffViewModel();
                    viewModel.Email = model.Email;
                    viewModel.FirstName = model.FirstName;
                    viewModel.PhoneNumber = model.PhoneNumber;
                    viewModel.LastName = model.LastName;
                    viewModel.Logo = model.Logo;
                    viewModel.Status = "Active";
                    viewModel.CreationDate = DateTime.UtcNow;

                    model.ServiceStaff = viewModel;
                    registerViewModel.ServiceStaffRegister = model;
                    ServiceStaffRegisterDTO Result = await _client.AddServiceStaffAsync(_mapper.Map<ServiceAndDeliveryStaffRegisterDTO>(registerViewModel));


                    //var Parent = category.ParentCategoryID.HasValue ? _categoryService.GetCategory((long)category.ParentCategoryID) : null;
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/ServiceStaff/Index",
                        message = "Staff successfully created.. !",
                        data = new
                        {
                            ID = Result.ServiceStaff.Id,
                            Date = Result.ServiceStaff.CreationDate != null ? Result.ServiceStaff.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                            Name = Result.ServiceStaff.FirstName,
                            Lastname = Result.ServiceStaff.LastName,
                            //Parent = Parent != null ? (Parent.CategoryName) : "",
                            //IsParentCategoryDeleted = category.IsParentCategoryDeleted.HasValue ? category.IsParentCategoryDeleted.Value.ToString() : bool.FalseString,
                            Contact = Result.ServiceStaff.PhoneNumber
                        }
                    });


                }
                else
                {
                    message = "Please fill the form properly ...";
                }
                return Json(new { success = false, message = message });
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

        public async Task<IActionResult> Edit(long Id)
        {
            try
            {
                ServiceStaffRegisterViewModel registerModel = new ServiceStaffRegisterViewModel();
                ServiceStaffViewModel model = _mapper.Map<ServiceStaffViewModel>(await _client.GetServiceStaffByIdAsync(Id));
                registerModel.ServiceStaff = model;
                registerModel.Password = model.User.PasswordHash;
                registerModel.Email = model.Email;
                registerModel.PhoneNumber = model.PhoneNumber;
                registerModel.FirstName = model.FirstName;
                registerModel.LastName = model.LastName;
                registerModel.UserName = model.User.UserName;
                registerModel.Status = model.Status;
                registerModel.Logo = model.Logo;

                return View(registerModel);
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
        public async Task<IActionResult> Edit(ServiceStaffRegisterViewModel model, long id)
        {
            if (ModelState.IsValid)
            {
                if (model.Email == "admin@fougito.com")
                    return Json(new { success = false, message = "Email cannot be admin@fougito.com" });

                try
                {
                    ServiceAndDeliveryStaffRegisteredViewModel registerViewModel = new ServiceAndDeliveryStaffRegisteredViewModel();
                    registerViewModel.RegisteringFor = Enum.GetName(typeof(Roles), Roles.ServiceStaff);
                    ServiceStaffViewModel viewModel = new ServiceStaffViewModel();
                    viewModel.Id = model.Id;
                    viewModel.Email = model.Email;
                    viewModel.FirstName = model.FirstName;
                    viewModel.PhoneNumber = model.PhoneNumber;
                    viewModel.LastName = model.LastName;
                    viewModel.Logo = model.Logo;
                    viewModel.PasswordHash = model.Password;
                    viewModel.Status = model.Status;
                    model.ServiceStaff = viewModel;
                    registerViewModel.ServiceStaffRegister = model;
                    ServiceStaffRegisterViewModel Result = _mapper.Map<ServiceStaffRegisterViewModel>(await _client.UpdateServiceStaffAsync(_mapper.Map<ServiceAndDeliveryStaffRegisterDTO>(registerViewModel)));

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            ID = Result.ServiceStaff.Id,
                            Date = Result.ServiceStaff.CreationDate != null ? Result.ServiceStaff.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                            Name = Result.ServiceStaff.FirstName,
                            Lastname=Result.ServiceStaff.LastName,
                            //Parent = Parent != null ? (Parent.CategoryName) : "",
                            //IsParentCategoryDeleted = category.IsParentCategoryDeleted.HasValue ? category.IsParentCategoryDeleted.Value.ToString() : bool.FalseString,
                            Contact = Result.ServiceStaff.PhoneNumber

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _client.DeleteServiceStaffAsync(id);

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

        public async Task<IActionResult> Details(long id)
        {
            ServiceStaffRegisterViewModel registerModel = new ServiceStaffRegisterViewModel();
            ServiceStaffViewModel model = _mapper.Map<ServiceStaffViewModel>(await _client.GetServiceStaffByIdAsync(id));
            registerModel.ServiceStaff = model;
            registerModel.Password = model.User.Password;
            registerModel.Email = model.User.Email;
            registerModel.PhoneNumber = model.PhoneNumber;
            registerModel.FirstName = model.User.FirstName;
            registerModel.LastName = model.User.LastName;
            registerModel.UserName = model.User.UserName;
            registerModel.Logo = model.Logo;

            return View(registerModel);
        }

        public async Task<IActionResult> ServiceStaffReport()
        {
            PagingParameters pagging = new PagingParameters();
            pagging.PageSize = 10;
            pagging.PageNumber = 1;
            var Info = _mapper.Map<IEnumerable<ServiceStaffViewModel>>(await _client.GetAllServiceStaffsAsync(pagging));
            return new CSVResult<ServiceStaffViewModel>(Info, "ServiceStaff");
        }
        public async Task<ActionResult> ToggleActiveStatus(long Id)
        {
            try
            {
                ServiceStaffViewModel Result = _mapper.Map<ServiceStaffViewModel>(await _client.ToggleActiveStatus(Id));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.FirstName,
                        LastName = Result.LastName,
                        Contact = Result.PhoneNumber,
                        Status = Result.Status
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
