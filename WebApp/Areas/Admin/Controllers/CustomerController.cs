using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICustomerClient _client;

        [BindProperty]
        public CustomerViewModel Model { get; set; }

        public CustomerController(IMapper mapper, ICustomerClient client)
        {
            _mapper = mapper;
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var user = _mapper.Map<IEnumerable<CustomerViewModel>>(await _client.GetAllCustomersAsync());
            return View(user);
        }
        //public async Task<ActionResult> Create()
        //{
        //    return View(new CustomerViewModel());
        //}
        //[HttpPost]
        //public async Task<ActionResult> Create(CustomerViewModel model)
        //{
        //    string message = string.Empty;
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            CustomerDTO Result = await _client.AddCustomerAsync(_mapper.Map<CustomerDTO>(model));
        //            Result.IsActive = true;
        //            return Json(new
        //            {
        //                success = true,
        //                url = "/Admin/Customer/Index",
        //                message = "Customer Created Successfully",
        //                data = new
        //                {
        //                    CreationDate = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
        //                    CustomerName = Result.Name,
        //                    Email = Result.Email,
        //                    IsActive = Result.IsActive,
        //                    ID = Result.Id
        //                }
        //            });
        //        }
        //        catch (ApiException ex)
        //        {
        //            ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
        //            return Json(new
        //            {
        //                url = "/Admin/Customer/Index",
        //                success = false,
        //                message = err.Message
        //            });
        //        }
        //    }

        //    return Json(new
        //    {
        //        success = false,
        //        message = "Fill all required fields and submit the form again"
        //    });
        //}
        [HttpPost]
        public async Task<IActionResult> Edit(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    CustomerDTO Result = await _client.UpdateCustomerAsync(_mapper.Map<CustomerDTO>(model));

                    Result.Id = Model.Id;

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            CustomerName = Result.Name,
                            Email = Result.Email,
                            IsActive = Result.Status == "Active" ? true : false,
                            ID = Result.Id
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
            CustomerViewModel customerById = _mapper.Map<CustomerViewModel>(await _client.GetCustomerByIdAsync(Id));
            return View(customerById);
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _client.DeleteCustomerAsync(Id);

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
        public async Task<IActionResult> Details(long Id)
        {
            CustomerViewModel CustomerDetail = _mapper.Map<CustomerViewModel>(await _client.GetCustomerByIdAsync(Id));
            return View(CustomerDetail);
        }
        public async Task<ActionResult> ToggleActiveStatus(long Id)
        {
            try
            {
                CustomerViewModel Result = _mapper.Map<CustomerViewModel>(await _client.ToggleActiveStatus(Id));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        CustomerName = Result.Name,
                        Email = Result.Email,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                        ID = Result.Id
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
