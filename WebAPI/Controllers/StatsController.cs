using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.ResponseWrapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.RestaurantDashboard;
using HelperClasses.DTOs.Supplier;
using HelperClasses.DTOs.GarageDashboard;
using HelperClasses.DTOs.SparePartDashboard;
using HelperClasses.Classes;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantService _restaurant;
        private readonly IRestaurantBranchService _restaurantBranch;
        private readonly IUserService _user;
        private readonly ICustomerService _customer;
        private readonly IGarageService _garage;
        private readonly IGarageRatingService _garageRating;
        private readonly IGarageTestimonialsService _garageTestimonials;
        private readonly IGarageMenuManagementService _garageMenus;
        private readonly ISparePartTestimonialService _sparePartTestimonials;
        private readonly ISparePartMenuManagementService _sparePartMenus;
        private readonly ISparePartsDealerService _sparePartsDealerService;
        private readonly ISparePartPartnersManagementService _sparePartPartnersManagementService;
        private readonly IMenuService _menus;
        private readonly IServiceStaffService _serviceStaffService;
        private readonly IDeliveryStaffService _deliveryStaffService;
        private readonly IRestaurantCashierStaffService _cashierStaffService;
        private readonly IItemService _itemService;
        private readonly ICouponService _couponService;
        private readonly IOrderService _orderService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        private readonly ISupplierItemCategoryService _supplierCategory;
        private readonly ISupplierCouponService _supplierCouponService;
        private readonly ISupplierOrderService _supplierOrderService;
        private readonly ISupplierPackageService _supplierPackageService;
        private readonly ISupplierItemService _supplierItemService;
        private readonly IGarageProjectService _garageProjectService;
        private readonly IGarageAwardService _garageAwardService;
        private readonly IGaragePartnersManagementService _garagePartnersManagementService;
        private readonly ITicketService _ticketService;
        private readonly IClientModulePurchasesService _clientModulePurchasesService;
        public StatsController(IMapper mapper, IRestaurantService restaurant, IUserService user,
            ICustomerService customer, IGarageService garage, ISparePartsDealerService sparePartsDealerService
            , IServiceStaffService serviceStaffService, IDeliveryStaffService deliveryStaffService, IRestaurantCashierStaffService cashierStaffService, IMenuService menus
            , IItemService itemService, ICouponService couponService, IOrderService orderService,
            IRestaurantBranchService restaurantBranch, ICategoryService categoryService,
            ISupplierService supplierService, ISupplierItemCategoryService supplierCategory,
            ISupplierCouponService supplierCouponService, ISupplierOrderService supplierOrderService,
            ISupplierPackageService supplierPackageService, ISupplierItemService supplierItemService, IGarageProjectService garageProjectService, IGarageAwardService garageAwardService, IGarageRatingService garageRating, IGarageTestimonialsService garageTestimonials, IGarageMenuManagementService garageMenus,
            IGaragePartnersManagementService garagePartnersManagementService, ISparePartPartnersManagementService sparePartPartnersManagementService 
            , ISparePartTestimonialService sparePartTestimonials, ISparePartMenuManagementService sparePartMenus,
            ITicketService ticketService, IClientModulePurchasesService clientModulePurchasesService)
        {
            _restaurant = restaurant;
            _mapper = mapper;
            _user = user;
            _customer = customer;
            _garage = garage;
            _sparePartsDealerService = sparePartsDealerService;
            _sparePartTestimonials = sparePartTestimonials;
            _sparePartMenus = sparePartMenus;
            _sparePartPartnersManagementService = sparePartPartnersManagementService;
            _serviceStaffService = serviceStaffService;
            _deliveryStaffService = deliveryStaffService;
            _cashierStaffService = cashierStaffService;
            _menus = menus;
            _itemService = itemService;
            _couponService = couponService;
            _orderService = orderService;
            _restaurantBranch = restaurantBranch;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _supplierCategory = supplierCategory;
            _supplierCouponService = supplierCouponService;
            _supplierOrderService = supplierOrderService;
            _supplierPackageService = supplierPackageService;
            _supplierItemService = supplierItemService;
            _garageProjectService = garageProjectService;
            _garageAwardService = garageAwardService;
            _garagePartnersManagementService = garagePartnersManagementService;
            _garageRating = garageRating;
            _garageTestimonials = garageTestimonials;
            _garageMenus = garageMenus;
            _ticketService = ticketService;
            _clientModulePurchasesService = clientModulePurchasesService;
        }


        [HttpGet("StatsCount")]
        public async Task<IActionResult> StatsCount()
        {
            AdminDashboardStatsDTO stats = new()
            {
                RestaurantCount = await _restaurant.GetAllRestaurantsCountAsync(),
                UserCount = await _user.GetAllUsersCountAsync(),
                CustomerCount = await _customer.GetAllCustomersCountAsync(),
                GarageCount = await _garage.GetAllGaragesCountAsync(),
                SparePartCount = await _sparePartsDealerService.GetAllSparePartsDealersCountAsync(),
                ServiceStaffCount = await _serviceStaffService.GetAllServiceStaffsCountAsync(),
                DeliveryStaffCount = await _deliveryStaffService.GetAllDeliveryStaffsCountAsync(),
                TicketCount = await _ticketService.GetAllTicketsCountAsync()
            };
            return Ok(stats);
        }
        [HttpGet("VendorStatsCount/{VendorId}")]
        public async Task<IActionResult> VendorStatsCount(long VendorId)
        {
            var purchases = await _clientModulePurchasesService.GetEarning(VendorId);
            var Garages = await _garage.GetGarageByVendorAsync(VendorId);
            VendorDashboardStatsDTO stats = new()
            {

                GarageCount = await _garage.GetAllGaragesCountByUserIdAsync(VendorId),
                Earning = purchases.Sum(x => x.SubTotal),
                PendingGarageCount = Garages.Count(x=>x.Status == Enum.GetName(typeof(Status), Status.Processing)),

            };
            return Ok(stats);
        }
        [HttpGet("Dashboard/RestaurantStatsCount/{RestaurantId}")]
        public async Task<IActionResult> RestaurantStatsCount(long RestaurantId)
        {
            /*var OrderIdForCustomerId = _orderService.Get(RestaurantId);
             var customerID = OrderIdForCustomerId;*/
            var Customer = await _customer.GetCustomerByRestaurantIdAsync(RestaurantId);
            long countCustomer = Customer.Where(x => x.Customer != null).Count();
            RestaurantDashboardStatsDTO stats = new()
            {
                RestaurantBranchCount = await _restaurantBranch.GetAllResaturantBranchesCountAsync(RestaurantId),
                UserCount = await _user.GetAllUsersCountAsync(),
                CustomerCount = countCustomer,
                ItemsCount = await _itemService.GetAllItemsCountByRestaurantIDAsync(RestaurantId),
                MenusCount = await _menus.GetAllMenusCountAsync(RestaurantId),
                CategoriesCount = await _categoryService.GetAllCategoriesCountAsync(RestaurantId),
                CouponsCount = await _couponService.GetAllCouponsCountAsync(RestaurantId),
                OrdersCount = await _orderService.GetAllOrdersByRestaurantIDCountAsync(RestaurantId),
                DeliveryStaffCount = await _deliveryStaffService.GetAllRestaurantDeliveryStaffsCountAsync(RestaurantId),
                ServiceStaffCount = await _serviceStaffService.GetAllServiceStaffsCountByRestaurantIdAsync(RestaurantId),
                CashierStaffCount = await _cashierStaffService.GetAllRestaurantCashierStaffsCountAsync(RestaurantId),
                CanceledCount = await _orderService.GetAllOrdersCancelledStausCountAsync(RestaurantId),
                ConfirmedCount = await _orderService.GetAllOrdersConfirmedStausCountAsync(RestaurantId),
                PendingCount = await _orderService.GetAllOrdersPendingStausCountAsync(RestaurantId),
                OnTheWayCount = await _orderService.GetAllOrdersOnTheWayStausCountAsync(RestaurantId),
                PreparingCount = await _orderService.GetAllOrdersPreparingStausCountAsync(RestaurantId),
                DeliveredCount = await _orderService.GetAllOrdersDeliveredStausCountAsync(RestaurantId),
                FoodReadyCount = await _orderService.GetAllOrdersFoodReadyStausCountAsync(RestaurantId),

            };
            return Ok(stats);
        }
        [HttpGet("Dashboard/SupplierStatusCount/{SupplierId}")]
        public async Task<IActionResult> SupplierStatusCount(long SupplierId)
        {
            SupplierDashboardDTO stats = new()
            {
                UserCount = await _supplierService.GetAllSuppliersCountAsync(),
                ItemsCount = await _supplierItemService.GetAllItemsCountBySupplierIdAsync(SupplierId),
                CategoriesCount = await _supplierCategory.GetAllSupplierCategoriesCountAsync(),
                CouponsCount = await _supplierCouponService.GetAllCouponsCountAsync(SupplierId),
                OrdersCount = await _supplierOrderService.GetAllOrdersBySupplierIDCountAsync(SupplierId),
                CanceledCount = await _supplierOrderService.GetAllOrdersCancelledStausCountAsync(SupplierId),
                ConfirmedCount = await _supplierOrderService.GetAllOrdersConfirmedStausCountAsync(SupplierId),
                PendingCount = await _supplierOrderService.GetAllOrdersPendingStausCountAsync(SupplierId),
                OnTheWayCount = await _supplierOrderService.GetAllOrdersOnTheWayStausCountAsync(SupplierId),
                PreparingCount = await _supplierOrderService.GetAllOrdersPreparingStausCountAsync(SupplierId),
                DeliveredCount = await _supplierOrderService.GetAllOrdersDeliveredStausCountAsync(SupplierId),
                FoodReadyCount = await _supplierOrderService.GetAllOrdersFoodReadyStausCountAsync(SupplierId),

            };
            return Ok(stats);
        }

        [HttpGet("Dashboard/Garage/{GarageId}")]
        public async Task<IActionResult> GarageStatusCount(long GarageId)
        {
            var rating = _garageRating.GetGarageRatingByIdAsync(GarageId).Result;
            GarageDashboardStatsDTO stats = new()
            {
                TotalProjects = await _garageProjectService.GetAllProjectsByGarageIdAsync(GarageId),
                TotalAwards = await _garageAwardService.GetAllAwardByGarageIdAsync(GarageId),
                TotalPartners = await _garagePartnersManagementService.GetAllPartnersByGarageIdAsync(GarageId),
                AverageRating = rating.Count() > 0 ? rating.Average(x => x.Rating) : 0,
                TotalMenus = await _garageMenus.GetCountByGarageIdAsync(GarageId),
                TotalTestimonials = await _garageTestimonials.GetCountByGarageIdAsync(GarageId),


            };
            return Ok(stats);
        }

        [HttpGet("Dashboard/SparePart/{SparePartId}")]
        public async Task<IActionResult> SparePartStatusCount(long SparePartId)
        {
            SparePartDashboardStatsDTO stats = new()
            {
                TotalPartners = await _sparePartPartnersManagementService.GetAllPartnersBySparePartDealerIdAsync(SparePartId),
                TotalMenus = await _sparePartMenus.GetCountBySparePartIdAsync(SparePartId),
                TotalTestimonials = await _sparePartTestimonials.GetCountBySparePartDealerIdAsync(SparePartId),
            };
            return Ok(stats);
        }
    }
}
