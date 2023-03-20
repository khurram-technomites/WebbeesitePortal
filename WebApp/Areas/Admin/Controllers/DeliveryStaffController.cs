using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.DeliveryStaff;
using HelperClasses.DTOs.ServiceAndDeliveryStaffDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Areas.Admin.Models;
using WebApp.Areas.Admin.Models.DeliveryStaff;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class DeliveryStaffController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDeliveryStaffClient _client;

        public DeliveryStaffController(IMapper mapper, IDeliveryStaffClient client)
        {
            _mapper = mapper;
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            PagingParameters paging = new PagingParameters();
            paging.PageNumber = 1;
            paging.PageSize = 10;
            var Info = _mapper.Map<IEnumerable<DeliveryStaffViewModel>>(await _client.GetAllDeliveryStaffsAsync(paging));
            return View(Info);
        }

        public async Task<IActionResult> Create()
        {
            return View(new DeliveryStaffRegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(DeliveryStaffRegisterViewModel model)
        {
            try
            {
                if (model.Email == "admin@fougito.com")
                    return Json(new { success = false, message = "Email cannot be admin@fougito.com" });

                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    ServiceAndDeliveryStaffRegisteredViewModel registerViewModel = new ServiceAndDeliveryStaffRegisteredViewModel();
                    registerViewModel.RegisteringFor = Enum.GetName(typeof(Roles), Roles.DeliveryStaff);
                    DeliveryStaffViewModel viewModel = new DeliveryStaffViewModel();
                    viewModel.Email = model.Email;
                    viewModel.FirstName = model.FirstName;
                    viewModel.PhoneNumber = model.PhoneNumber;
                    viewModel.LastName = model.LastName;
                    viewModel.Logo = model.Logo;
                    viewModel.CreationDate = DateTime.UtcNow;
                    viewModel.Status = "Active";
                    model.DeliveryStaff = viewModel;
                    registerViewModel.DeliveryStaffRegister = model;
                    DeliveryStaffRegisterDTO Result = await _client.AddDeliveryStaffAsync(_mapper.Map<ServiceAndDeliveryStaffRegisterDTO>(registerViewModel));


                    //var Parent = category.ParentCategoryID.HasValue ? _categoryService.GetCategory((long)category.ParentCategoryID) : null;
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/DeliveryStaff/Index",
                        message = "Delivery staff successfully created.. !",
                        data = new
                        {
                            ID = Result.DeliveryStaff.Id,
                            Date = Result.DeliveryStaff.CreationDate != null ? Result.DeliveryStaff.CreationDate.ToString("dd MMM yyyy") : "-",
                            Name = Result.DeliveryStaff.FirstName,
                            Lastname = Result.DeliveryStaff.LastName,
                            //Parent = Parent != null ? (Parent.CategoryName) : "",
                            //IsParentCategoryDeleted = category.IsParentCategoryDeleted.HasValue ? category.IsParentCategoryDeleted.Value.ToString() : bool.FalseString,
                            Contact = Result.DeliveryStaff.PhoneNumber
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

        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                DeliveryStaffRegisterViewModel registerModel = new DeliveryStaffRegisterViewModel();
                DeliveryStaffViewModel model = _mapper.Map<DeliveryStaffViewModel>(await _client.GetDeliveryStaffByIdAsync(id));
                registerModel.DeliveryStaff = model;
                registerModel.Password = model.User.PasswordHash;
                registerModel.Email = model.User.Email;
                registerModel.PhoneNumber = model.PhoneNumber;
                registerModel.FirstName = model.User.FirstName;
                registerModel.LastName = model.User.LastName;
                registerModel.UserName = model.User.UserName;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DeliveryStaffRegisterViewModel model, long Id)
        {
            if (ModelState.IsValid)
            {
                if (model.Email == "admin@fougito.com")
                    return Json(new { success = false, message = "Email cannot be admin@fougito.com" });

                try
                {
                    ServiceAndDeliveryStaffRegisteredViewModel registerViewModel = new ServiceAndDeliveryStaffRegisteredViewModel();
                    registerViewModel.RegisteringFor = Enum.GetName(typeof(Roles), Roles.DeliveryStaff);
                    DeliveryStaffViewModel viewModel = new DeliveryStaffViewModel();
                    viewModel.Id = model.Id;
                    viewModel.Email = model.Email;
                    viewModel.FirstName = model.FirstName;
                    /*viewModel.User.UserName = model.UserName;*/
                    viewModel.PhoneNumber = model.PhoneNumber;
                    viewModel.LastName = model.LastName;
                    viewModel.Logo = model.Logo;
                    viewModel.Password = model.Password;
                    model.DeliveryStaff = viewModel;
                    registerViewModel.DeliveryStaffRegister = model;

                    DeliveryStaffRegisterViewModel Result = _mapper.Map<DeliveryStaffRegisterViewModel>(await _client.UpdateDeliveryStaffAsync(_mapper.Map<ServiceAndDeliveryStaffRegisterDTO>(registerViewModel)));

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            ID = Result.DeliveryStaff.Id,
                            Date = Result.DeliveryStaff.CreationDate != null ? Result.DeliveryStaff.CreationDate.ToString("dd MMM yyyy") : "-",
                            Name = Result.DeliveryStaff.FirstName,
                            Lastname = Result.DeliveryStaff.LastName,
                            //Parent = Parent != null ? (Parent.CategoryName) : "",
                            //IsParentCategoryDeleted = category.IsParentCategoryDeleted.HasValue ? category.IsParentCategoryDeleted.Value.ToString() : bool.FalseString,
                            Contact = Result.DeliveryStaff.PhoneNumber

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
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _client.DeleteDeliveryStaffAsync(id);

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
            DeliveryStaffRegisterViewModel registerModel = new DeliveryStaffRegisterViewModel();
            DeliveryStaffViewModel model = _mapper.Map<DeliveryStaffViewModel>(await _client.GetDeliveryStaffByIdAsync(id));
            registerModel.DeliveryStaff = model;
            registerModel.Password = model.User.Password;
            registerModel.Email = model.User.Email;
            registerModel.PhoneNumber = model.PhoneNumber;
            registerModel.FirstName = model.User.FirstName;
            registerModel.LastName = model.User.LastName;
            registerModel.UserName = model.User.UserName;
            registerModel.Logo = model.Logo;

            return View(registerModel);
        }

        public async Task<IActionResult> DeliveryStaffReport()
        {
            PagingParameters pagging = new PagingParameters();
            pagging.PageSize = 10;
            pagging.PageNumber = 1;
            var Info = _mapper.Map<IEnumerable<DeliveryStaffViewModel>>(await _client.GetAllDeliveryStaffsAsync(pagging));
            return new CSVResult<DeliveryStaffViewModel>(Info, "DeliveryStaff");
        }
        public async Task<ActionResult> ToggleActiveStatus(long Id)
        {
            try
            {
                DeliveryStaffViewModel Result = _mapper.Map<DeliveryStaffViewModel>(await _client.ToggleActiveStatus(Id));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.FirstName,
                        Lastname = Result.LastName,
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
