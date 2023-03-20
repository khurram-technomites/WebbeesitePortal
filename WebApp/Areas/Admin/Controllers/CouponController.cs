using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CouponController : Controller
    {
        private readonly ICouponClient _client;
        private readonly ICustomerClient _customerClient;
        private readonly ICustomerCouponClient _customerCouponClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IFileUpload _fileUpload;
        public CouponController(ICouponClient client, IMapper mapper, IUserSessionManager userSessionManager, IFileUpload fileUpload, ICustomerClient customerClient, ICustomerCouponClient customerCouponClient)
        {
            _client = client;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _fileUpload = fileUpload;
            _customerClient = customerClient;
            _customerCouponClient = customerCouponClient;
        }
        public async Task<IActionResult> Index()
        {
            var Info = _mapper.Map<IEnumerable<CouponViewModel>>(await _client.GetAllAdminCouponsAsync());
            return View(Info);
        }

        public async Task<IActionResult> Create()
        {
            //var Info = _mapper.Map<IEnumerable<CouponViewModel>>(await _client.GetAllAdminCouponsAsync());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CouponViewModel model)
        {
            try
            {
                model.Status = Enum.GetName(typeof(Status), Status.Active);
                model.Module = "Admin";
                string message = string.Empty;
                string notificationmessage = string.Empty;
                if (model.Expiry.HasValue)
                {
                    model.Expiry.Value.ToString("MM/dd/yyyy");
                }

                CouponDTO Result = await _client.AddCouponAsync(_mapper.Map<CouponDTO>(model));

                if (model.IsOpenToAll == true)
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Coupon/Index",
                        message = "Record Added Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                            CouponCode = Result.CouponCode,
                            Name = Result.Name,
                            Discount = Result.DiscountPercentage,
                            Type = Result.Type,
                            Value = Result.Value,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
                        }
                    });
                }

                return Json(new
                {
                    success = true,
                    url = "/Admin/Coupon/Index",
                    message = "Record Added Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                        CouponCode = Result.CouponCode,
                        Name = Result.Name,
                        Discount = Result.DiscountPercentage,
                        Type = Result.Type,
                        Value = Result.Value,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
                    }
                });
                //var Parent = Coupon.ParentCouponID.HasValue ? _CouponService.GetCoupon((long)Coupon.ParentCouponID) : null;



            }
            catch (ApiException ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                });
            }
        }

        public async Task<IActionResult> Edit(long id)
        {
            CouponViewModel CouponDTO = _mapper.Map<CouponViewModel>(await _client.GetCouponByIdAsync(id));
            return View(CouponDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CouponViewModel model)
        {

            try
            {

                CouponDTO Result = await _client.UpdateCouponAsync(_mapper.Map<CouponDTO>(model));

                Result.Id = model.Id;

                if (model.IsOpenToAll == true)
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Coupon/Index",
                        message = "Record Added Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy") : "-",
                            CouponCode = Result.CouponCode,
                            Name = Result.Name,
                            Discount = Result.DiscountPercentage,
                            Type = Result.Type,
                            Value = Result.Value,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
                        }
                    });
                }

                return Json(new
                {
                    success = true,
                    url = "/Admin/Coupon/Index",
                    message = "Record Added Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                        CouponCode = Result.CouponCode,
                        Name = Result.Name,
                        Discount = Result.DiscountPercentage,
                        Type = Result.Type,
                        Value = Result.Value,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
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

        public async Task<ActionResult> ToggleActiveStatus(long id)
        {
            try
            {
                CouponViewModel Result = _mapper.Map<CouponViewModel>(await _client.ToggleActiveStatus(id));

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                        CouponCode = Result.CouponCode,
                        Name = Result.Name,
                        Discount = Result.DiscountPercentage,
                        Type = Result.Type,
                        Value = Result.Value,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
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

        public async Task<IActionResult> GetCustomers(long id)
        {
            var customers = await _customerClient.GetAllCustomerDropDownByAdminAsync();
            ViewBag.Customers = new SelectList(customers, "Id", "Name");
            ViewBag.CouponId = id;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SetCouponsByCouponID(long CouponsID)
        {
            try
            {
                var coupons = await _customerCouponClient.GetCustomerCouponsByCoupon(CouponsID);
                return Json(new
                {
                    success = true,
                    data = coupons.Select(x => new
                    {
                        id = x.CustomerId,
                        name = x.Customer.Name
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveCustomerCoupons(CustomerCouponViewModel model)
        {
            try
            {
                CustomerCouponDTO customerCoupon = await _customerCouponClient.GetCustomerCoupon(model.CustomerId, model.CouponId);


                if (customerCoupon == null)
                {

                    CustomerCouponDTO Result = await _customerCouponClient.AddCustomerCouponAsync(_mapper.Map<CustomerCouponDTO>(model));
                    return Json(new
                    {
                        success = true,
                        message = "Coupon assigned to customer successfully ..."
                    });

                }

                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Coupon is already assigned to customer"
                    });

                }

            }

            catch (ApiException ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                });
            }


        }

        [HttpPost]
        public async Task<ActionResult> DeleteCouponCustomers(long CustomerId, long CouponId)
        {
            try
            {
                CustomerCouponDTO customerCoupon = await _customerCouponClient.GetCustomerCoupon(CustomerId, CouponId);

                await _customerCouponClient.DeleteCustomerCouponAsync(customerCoupon.Id);

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

        [HttpPost]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _client.DeleteCouponAsync(id);

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
            return View(_mapper.Map<CouponViewModel>(await _client.GetCouponByIdAsync(id)));
        }

        public async Task<IActionResult> CouponReport()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<CouponViewModel>>(await _client.GetAllCouponsAsync(restaurantId));
            return new CSVResult<CouponViewModel>(Info, "Coupon");
        }
    }
}
