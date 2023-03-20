using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Supplier;
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

namespace WebApp.Areas.Supplier.Controllers
{
    [Area("Supplier")]
    [Authorize(Roles = "Supplier , Restaurant , Admin")]
    public class SupplierCouponController : Controller
    {
		private readonly ISupplierCouponClient _client;
		private readonly ICustomerClient _customerClient;
		private readonly IRestaurantCouponClient _customerCouponClient;
		private readonly IMapper _mapper;
		private readonly IUserSessionManager _userSessionManager;
		private readonly IFileUpload _fileUpload;
		public SupplierCouponController(ISupplierCouponClient client, IMapper mapper, IUserSessionManager userSessionManager, IFileUpload fileUpload , ICustomerClient customerClient , IRestaurantCouponClient customerCouponClient)
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
            long supplierId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<SupplierCouponViewModel>>(await _client.GetAllCouponsAsync(supplierId));
            return View(Info);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SupplierCouponViewModel model)
        {
            try
            {
                long supplierId = _userSessionManager.GetUserStore().Id;
                model.Status = Enum.GetName(typeof(Status), Status.Active);
                model.SupplierId = supplierId;
                model.Module = "Supplier";
                string message = string.Empty;
                string notificationmessage = string.Empty;
                if (model.Expiry.HasValue)
                {
                    model.Expiry.Value.ToString("dd MMM yyyy, hh:mm tt");
                }

                SupplierCouponDTO Result = await _client.AddCouponAsync(_mapper.Map<SupplierCouponDTO>(model));

                if (model.IsOpenToAll == true)
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Supplier/SupplierCoupon/Index",
                        message = "Record Added Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                            CouponCode = Result.CouponCode,
                            Name = Result.Name,
                            //Discount = Result.DiscountPercentage,
                            Type = Result.Type,
                            Value = Result.Value,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
                        }
                    });
                }

                return Json(new
                {
                    success = true,
                    url = "/Supplier/SupplierCoupon/Index",
                    message = "Record Added Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                        CouponCode = Result.CouponCode,
                        Name = Result.Name,
                        //Discount = Result.DiscountPercentage,
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
            SupplierCouponViewModel CouponDTO = _mapper.Map<SupplierCouponViewModel>(await _client.GetCouponByIdAsync(id));
            return View(CouponDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SupplierCouponViewModel model)
        {

            try
            {
                model.SupplierId = _userSessionManager.GetUserStore().Id;
                SupplierCouponDTO Result = await _client.UpdateCouponAsync(_mapper.Map<SupplierCouponDTO>(model));

                Result.Id = model.Id;

                if (model.IsOpenToAll == true)
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Supplier/SupplierCoupon/Index",
                        message = "Coupon Updated Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                            CouponCode = Result.CouponCode,
                            Name = Result.Name,
                            //Discount = Result.DiscountPercentage,
                            Type = Result.Type,
                            Value = Result.Value,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
                        }
                    });
                }

                return Json(new
                {
                    success = true,
                    url = "/Supplier/SupplierCoupon/Index",
                    message = "Coupon Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                        CouponCode = Result.CouponCode,
                        Name = Result.Name,
                        //Discount = Result.DiscountPercentage,
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
                SupplierCouponViewModel Result = _mapper.Map<SupplierCouponViewModel>(await _client.ToggleActiveStatus(id));

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
                        //Discount = Result.DiscountPercentage,
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
            //long supplierId = _userSessionManager.GetUserStore().Id;
            var customers = await _customerClient.GetAllSupplierCustomersAsync();
            ViewBag.Customers = customers.OrderBy(x => x.NameAsPerTradeLicense);
            ViewBag.CouponId = id;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SetCouponsByCouponID(long CouponsID)
        {
            try
            {
                var coupons = await _customerCouponClient.GetRestaurantCouponsByCoupon(CouponsID);
                return Json(new
                {
                    success = true,
                    data = coupons.Select(x => new
                    {
                        id = x.Restaurant.Id,
                        name = x.Restaurant.NameAsPerTradeLicense
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
        public async Task<IActionResult> SaveCustomerCoupons(RestaurantCouponViewModel model , long CouponsID)
        {
            try
            {
                RestaurantCouponDTO customerCoupon = await _customerCouponClient.GetRestaurantCoupon(model.RestaurantId, model.SupplierCouponId);


                if (customerCoupon == null)
                {

                    RestaurantCouponDTO Result = await _customerCouponClient.AddRestaurantCouponAsync(_mapper.Map<RestaurantCouponDTO>(model));
                    return Json(new
                    {
                        success = true,
                        message = "Coupon assigned to customer successfully ..."
                    });

                }

                else
                {
                    RestaurantCouponDTO Result = await _customerCouponClient.UpdateRestaurantCouponAsync(_mapper.Map<RestaurantCouponDTO>(model));
                    return Json(new
                    {
                        success = false,
                        message = "Coupon assigned to customer successfully ..."
                    });

                }
                return Json(new
                {
                    success = false,
                    message = "Coupon assigned to customer successfully ..."
                });

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
        public async Task<ActionResult> DeleteCouponCustomers(long RestaurantId, long CouponsID)
        {
            try
            {
                RestaurantCouponDTO customerCoupon = await _customerCouponClient.GetRestaurantCoupon(RestaurantId, CouponsID);

                await _customerCouponClient.DeleteRestaurantCouponAsync(customerCoupon.Id);

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
            return View(_mapper.Map<SupplierCouponViewModel>(await _client.GetCouponByIdAsync(id)));
        }

        public async Task<IActionResult> CouponReport()
        {
            long supplierId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<SupplierCouponViewModel>>(await _client.GetAllCouponsAsync(supplierId));
            return new CSVResult<SupplierCouponViewModel>(Info, "Coupon");
        }
    }
}
