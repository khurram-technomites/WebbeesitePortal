using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class ShopController : Controller
    {
        private readonly ISupplierClient _client;
        private readonly ISupplierCouponRedemptionClient _redemptionclient;
        private readonly IRestaurantCouponClient _restaurantCouponClient;
        private readonly ISupplierCouponClient _couponClient;
        private readonly ISupplierOrderClient _orderClient;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IMapper _mapper;
        public ShopController(ISupplierClient client, IMapper mapper, ISupplierCouponRedemptionClient redemptionClient, IUserSessionManager userSessionManager, ISupplierOrderClient orderClient, ISupplierCouponClient couponClient , IRestaurantCouponClient restaurantCouponClient)
        {
            _client = client;
            _redemptionclient = redemptionClient;
            _restaurantCouponClient = restaurantCouponClient;
            _couponClient = couponClient;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _orderClient = orderClient;


        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<SupplierCardViewModel> suppliers = _mapper.Map<IEnumerable<SupplierCardViewModel>>(await _client.GetAllSuppliersAsync());

            return View(suppliers);
        }

        public async Task<IActionResult> ProductsByShop(long id)
        {


            SupplierViewModel supplier = _mapper.Map<IEnumerable<SupplierViewModel>>(await _client.GetSupplierById(id)).FirstOrDefault();
            ViewBag.SupplierId = supplier.Id;
            TempData["SupplierId"] = ViewBag.SupplierId;
            ViewBag.SupplierName = supplier.NameAsPerTradeLicense;
            PagingParameters paging = new PagingParameters();
            paging.PageNumber = 1;
            paging.PageSize = 20;


            IEnumerable<SupplierItemViewModel> items = _mapper.Map<IEnumerable<SupplierItemViewModel>>(await _client.GetAllSupplierItems(id, paging));
            IEnumerable<SupplierItemCategoryViewModel> categories = _mapper.Map<IEnumerable<SupplierItemCategoryViewModel>>(await _client.GetAllCategory(id));
            SupplierItemsAndCategoriesViewModel model = new SupplierItemsAndCategoriesViewModel();
            model.Items = items;
            model.Categories = categories;

            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> Pagination(long id, long categoryId = 0, int pgno = 1)
        {
            PagingParameters paging = new PagingParameters();
            paging.PageNumber = pgno;
            paging.PageSize = 20;

            if (categoryId == 0)
            {
                IEnumerable<SupplierItemViewModel> items = _mapper.Map<IEnumerable<SupplierItemViewModel>>(await _client.GetAllSupplierItems(id, paging));
                return Json(new
                {

                    success = true,
                    items = items.Select(i => new
                    {

                        id = i.Id,
                        name = i.Title,
                        packaging = i.Packaging != null ? i.Packaging : "-",
                        image = string.IsNullOrEmpty(i.Thumbnail) ? "/Images/Icons/product_default.png" : i.Thumbnail,
                        categoryId = i.CategoryId,
                        regularPrice = i.RegularPrice,
                        salePrice = i.SalePrice,
                        discount = i.DiscountPercentage,

                    })

                });
            }

            else
            {
                IEnumerable<SupplierItemViewModel> AllItems = _mapper.Map<IEnumerable<SupplierItemViewModel>>(await _client.GetAllSupplierItemsByCategory(id , categoryId, paging));
                return Json(new
                {
                    success = true,
                    items = AllItems.Select(i => new
                    {
                        id = i.Id,
                        name = i.Title,
                        packaging = i.Packaging != null ? i.Packaging : "-",
                        image = string.IsNullOrEmpty(i.Thumbnail) ? "/Images/Icons/product_default.png" : i.Thumbnail,
                        categoryId = i.CategoryId,
                        regularPrice = i.RegularPrice,
                        salePrice = i.SalePrice,
                        discount = i.DiscountPercentage,
                    })

                });

            }
        }

        public async Task<IActionResult> GetProductByCategory(long SupplierId, long CategoryId)
        {
            PagingParameters paging = new PagingParameters();
            paging.PageNumber = 1;
            paging.PageSize = 20;
            if (CategoryId == 0)
            {
                IEnumerable<SupplierItemViewModel> AllItems = _mapper.Map<IEnumerable<SupplierItemViewModel>>(await _client.GetAllSupplierItems(SupplierId, paging));
                if (AllItems.Any())
                {
                    return Json(new
                    {

                        success = true,
                        items = AllItems.Select(i => new
                        {

                            id = i.Id,
                            name = i.Title,
                            packaging = i.Packaging != null ? i.Packaging : "-",
                            image = string.IsNullOrEmpty(i.Thumbnail) ? "/Images/Icons/product_default.png" : i.Thumbnail,
                            categoryId = i.CategoryId,
                            regularPrice = i.RegularPrice,
                            salePrice = i.SalePrice,
                            discount = i.DiscountPercentage,

                        })

                    });
                }

            }

            IEnumerable<SupplierItemViewModel> items = _mapper.Map<IEnumerable<SupplierItemViewModel>>(await _client.GetAllSupplierItemsByCategory(SupplierId, CategoryId, paging));
            if (items.Any())
            {
                return Json(new
                {

                    success = true,
                    items = items.Select(i => new
                    {

                        id = i.Id,
                        name = i.Title,
                        packaging = i.Packaging != null ? i.Packaging : "-",
                        categoryId = i.CategoryId,
                        regularPrice = i.RegularPrice,
                        image = string.IsNullOrEmpty(i.Thumbnail) ? "/Images/Icons/product_default.png" : i.Thumbnail,
                        salePrice = i.SalePrice,
                        discount = i.DiscountPercentage,

                    })

                });

            }

            else
            {
                return Json(new
                {

                    success = "false",
                    message = "Item not found!",

                });
            }
        }

        public async Task<IActionResult> AddToCart(long id)
        {

            SupplierItemViewModel item = _mapper.Map<SupplierItemViewModel>(await _client.GetItemById(id));
            return View(item);
        }

        public async Task<IActionResult> Basket(long id)
        {

            IEnumerable<SupplierViewModel> suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _client.GetSupplierById(id));
            SupplierViewModel supplier = suppliers.FirstOrDefault();
            return View(supplier);
        }

        public async Task<IActionResult> Coupon()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            IEnumerable<SupplierCouponViewModel> coupons = _mapper.Map<IEnumerable<SupplierCouponViewModel>>(await _restaurantCouponClient.GetAllRestaurantCoupons(restaurantId));
            return View(coupons);
        }

        public async Task<IActionResult> CouponValidate(long id)
        {
            string userId = _userSessionManager.GetUserStore().UserId;
            SupplierCouponViewModel supplierCoupon = _mapper.Map<SupplierCouponViewModel>(await _couponClient.GetCouponByIdAsync(id));
            if (supplierCoupon != null)
            {
                if (supplierCoupon.IsOpenToAll == true)
                {
                    IEnumerable<SupplierCouponRedemptionViewModel> allCoupons = _mapper.Map<IEnumerable<SupplierCouponRedemptionViewModel>>(await _redemptionclient.GetAllSupplierCouponRedemptions(id));

                    if (allCoupons.Count() < supplierCoupon.MaxUsage)
                    {
                        

                        return Json(new
                        {

                            success = true,
                            percent = supplierCoupon.Type == "Percentage" ? true : false,
                            amount = supplierCoupon.Value,
                            couponCode = supplierCoupon.CouponCode,
                            maxAmount = supplierCoupon.MaxAmount,
                            message = "Coupon successfully redeemed!"

                        });
                    }
                    else
                    {
                        return Json(new
                        {

                            success = false,
                            message = "Coupon is not valid now!"

                        });

                    }
                }

                else
                {
                    IEnumerable<SupplierCouponRedemptionViewModel> allCoupons = _mapper.Map<IEnumerable<SupplierCouponRedemptionViewModel>>(await _redemptionclient.GetSupplierCouponRedemptionsByRestaurant(userId, id));
                    if (allCoupons.Count() < supplierCoupon.Frequency)
                    {
                        
                        return Json(new
                        {

                            success = true,
                            percent = supplierCoupon.Type == "Percentage" ? true : false,
                            amount = supplierCoupon.Value,
                            couponCode = supplierCoupon.CouponCode,
                            maxAmount = supplierCoupon.MaxAmount,
                            message = "Coupon successfully redeemed!",

                        });
                    }
                    else
                    {
                        return Json(new
                        {

                            success = false,
                            message = "Coupon limit has exceeded!"

                        });
                    }

                }

            }

            else
            {
                return Json(new
                {

                    success = false,
                    message = "Oops something went wrong!"

                });
            }
        }

        public IActionResult OrderNow()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OrderNow(SupplierOrderPlacementViewModel order)
        {
            string result = await _orderClient.PlaceOrder(_mapper.Map<SupplierOrderPlacementDTO>(order));
            if (!string.IsNullOrEmpty(result))
            {
                return Json(new
                {
                    success = true,
                    url = result,

                });
            }

            return Json(new
            {
                sucess = false,

            });
        }

        [AllowAnonymous]
        public async Task<ActionResult> Paid(long id, [FromQuery(Name = "paymentId")] string PaymentId)
        {
            string message = string.Empty;
            int response = await _orderClient.Paid(PaymentId, id);
            if (response == 1)
            {
                message = "success";
            }
            else if (response == 2)
            {

                message = "card captured";
            }
            else
            {
                message = "failed";
            }

            TempData["Message"] = message;
            return View();
        }

        public IActionResult ScheduleOrder()
        {
            return View();
        }
    }
}
