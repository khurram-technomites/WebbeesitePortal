using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using WebAPI.Configurations;
using WebAPI.Helpers;

#nullable disable

namespace WebAPI.Models
{
    public partial class FougitoContext : IdentityDbContext<AppUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FougitoContext()
        {
        }

        public FougitoContext(DbContextOptions<FougitoContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual DbSet<Aggregator> Aggregators { get; set; }
        public virtual DbSet<CurrencyNote> CurrencyNotes { get; set; }
        public virtual DbSet<CardScheme> CardSchemes { get; set; }
        public virtual DbSet<ServiceStaff> ServiceStaffs { get; set; }
        public virtual DbSet<Areas> Areas { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<RestaurantCustomer> RestaurantCustomers { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<BusinessSettings> BusinessSettings { get; set; }
        public virtual DbSet<Blogs> Blogs { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<IntegrationSetting> IntegrationSettings { get; set; }
        public virtual DbSet<MyFatoorahPaymentGatewaySetting> MyFatoorahPaymentGatewaySettings { get; set; }
        public virtual DbSet<NotificationReceiver> NotificationReceivers { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<DeliveryStaff> DeliveryStaffs { get; set; }
        public virtual DbSet<Garage> Garages { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CarMake> CarMakes { get; set; }
        public virtual DbSet<CarModel> CarModels { get; set; }
        public virtual DbSet<GarageRepairSpecification> GarageRepairSpecifications { get; set; }
        public virtual DbSet<GarageImage> GarageImages { get; set; }
        public virtual DbSet<GarageDocument> GarageDocuments { get; set; }
        public virtual DbSet<GarageSchedule> GarageSchedules { get; set; }
        public virtual DbSet<SparePartsDealer> SparePartsDealers { get; set; }
        public virtual DbSet<DealerInventorySpecification> DealerInventorySpecifications { get; set; }
        public virtual DbSet<DealerImage> DealerImages { get; set; }
        public virtual DbSet<DealerSchedule> DealerSchedules { get; set; }
        public virtual DbSet<SparePartsDealerDocument> SparePartsDealerDocuments { get; set; }
        public virtual DbSet<SparePartRequest> SparePartRequests { get; set; }
        public virtual DbSet<SparePartRequestImage> SparePartRequestImages { get; set; }
        public virtual DbSet<SparePartRequestQuote> SparePartRequestQuotes { get; set; }
        public virtual DbSet<SparePartRequestQuoteImage> SparePartRequestQuotesImages { get; set; }
        public virtual DbSet<SparePartDeliveries> SparePartDeliveries { get; set; }
        public virtual DbSet<SparePartBannerSetting> SparePartBannerSettings { get; set; }
        public virtual DbSet<SparePartMenu> SparePartMenus { get; set; }
        public virtual DbSet<SparePartMenuManagement> SparePartMenusManagement { get; set; }
        public virtual DbSet<SparePartContentManagement> SparePartContentsManagement { get; set; }
        public virtual DbSet<SparePartServiceManagement> SparePartServicesManagement { get; set; }
        public virtual DbSet<SparePartTeamManagement> SparePartTeamsManagement { get; set; }
        public virtual DbSet<SparePartExpertiseManagement> SparePartExpertiseManagement { get; set; }
        public virtual DbSet<SparePartPartnersManagement> SparePartPartnersManagement { get; set; }
        public virtual DbSet<SparePartAppointmentManagement> SparePartAppointmentsManagement { get; set; }
        public virtual DbSet<SparePartExpertise> SparePartExpertise { get; set; }
        public virtual DbSet<SparePartTestimonial> SparePartTestimonials { get; set; }
        public virtual DbSet<SparePartBlog> SparePartBlogs { get; set; }
        public virtual DbSet<SparePartSubscriber> SparePartSubscribers { get; set; }
        public virtual DbSet<SparePartCustomerAppointment> SparePartCustomerAppointments { get; set; }
        public virtual DbSet<SparePartCareer> SparePartCareers { get; set; }
        public virtual DbSet<SparePartFAQ> SparePartFAQs{ get; set; }
        public virtual DbSet<SparePartBusinessSetting> SparePartBusinessSettings { get; set; }
        public virtual DbSet<SparePartBranchBusinessSetting> SparePartBranchBusinessSettings { get; set; }
        public virtual DbSet<SparePartCustomerFeedback> SparePartCustomerFeedbacks { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<RouteGroup> RouteGroups { get; set; }
        public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<GarageBusinessSetting> GarageBusinessSettings { get; set; }
        public virtual DbSet<GarageBranchBusinessSetting> GarageBranchBusinessSettings { get; set; }
        public virtual DbSet<RestaurantBranch> RestaurantBranches { get; set; }
        public virtual DbSet<RestaurantBranchSchedule> RestaurantBranchSchedules { get; set; }
        public virtual DbSet<ItemCategory> ItemCategories { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemOption> ItemOptions { get; set; }
        public virtual DbSet<ItemOptionValue> ItemOptionValues { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<MenuItemOptionValue> MenuItemOptionValues { get; set; }
        public virtual DbSet<MenuItemOption> MenuItemOptions { get; set; }
        public virtual DbSet<OrderDetailOptionValue> OrderDetailOptionValues { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<NumberRange> NumberRanges { get; set; }
        public virtual DbSet<RestaurantDeliveryStaff> RestaurantDeliveryStaffs { get; set; }
        public virtual DbSet<RestaurantCashierStaff> RestaurantCashierStaffs { get; set; }
        public virtual DbSet<RestaurantRating> RestaurantRatings { get; set; }
        public virtual DbSet<GarageAward> GarageAwards { get; set; }
        public virtual DbSet<RestaurantImages> RestaurantImages { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<CouponCategory> CouponCategories { get; set; }
        public virtual DbSet<CustomerCoupon> CustomerCoupon { get; set; }
        public virtual DbSet<CouponRedemption> CouponRedemptions { get; set; }
        public virtual DbSet<RestaurantBannerSetting> RestaurantBannerSettings { get; set; }
        public virtual DbSet<RestaurantServiceStaff> RestaurantServiceStaffs { get; set; }
        public virtual DbSet<PopularCategories> PopularCategories { get; set; }
        public virtual DbSet<TrendingItem> TrendingItems { get; set; }
        public virtual DbSet<RestaurantContentManagement> RestaurantContentManagement { get; set; }
        public virtual DbSet<FCMUserSession> FCMUserSessions { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TicketDocument> TicketDocument { get; set; }
        public virtual DbSet<TicketMessages> TicketMessages { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<SupplierDocument> SupplierDocument { get; set; }
        public virtual DbSet<SupplierItem> SupplierItem { get; set; }
        public virtual DbSet<SupplierItemCategory> SupplierItemCategory { get; set; }
        public virtual DbSet<SupplierItemImage> SupplierItemImage { get; set; }
        public virtual DbSet<SupplierOrder> SupplierOrder { get; set; }
        public virtual DbSet<SupplierCoupon> SupplierCoupon { get; set; }
        public virtual DbSet<SupplierCouponCategory> SupplierCouponCategories { get; set; }
        public virtual DbSet<SupplierCouponCategory> SupplierCouponRedemption { get; set; }
        public virtual DbSet<RestaurantCoupon> RestaurantCoupons { get; set; }
        public virtual DbSet<SupplierOTPVerification> SupplierOTPVerification { get; set; }
        public virtual DbSet<RestaurantTransactionHistory> RestaurantTransactionHistory { get; set; }
        public virtual DbSet<SupplierPackage> SupplierPackage { get; set; }
        public virtual DbSet<RestaurantPrinterSetting> RestaurantPrinterSettings { get; set; }
        public virtual DbSet<RestaurantCardScheme> RestaurantCardSchemes { get; set; }
        public virtual DbSet<RestaurantTaxSetting> RestaurantTaxSettings { get; set; }
        public virtual DbSet<RestaurantAggregator> RestaurantAggregators { get; set; }
        public virtual DbSet<RestaurantUserLogManagement> RestaurantUserLogManagement { get; set; }
        public virtual DbSet<RestaurantManager> RestaurantManagers { get; set; }
        public virtual DbSet<RestaurantWaiter> RestaurantWaiters { get; set; }
        public virtual DbSet<RestaurantKitchenManager> RestaurantKitchenManagers { get; set; }
        public virtual DbSet<RestaurantTable> RestaurantTables { get; set; }
        public virtual DbSet<RestaurantTableReservation> RestaurantTableReservations { get; set; }
        public virtual DbSet<RestaurantReservation> RestaurantReservations { get; set; }
        public virtual DbSet<RestaurantBalanceSheet> RestaurantBalanceSheets { get; set; }
        public virtual DbSet<RestaurantCashDenomination> RestaurantCashDenominations { get; set; }
        public virtual DbSet<RestaurantCashDenominationDetail> RestaurantCashDenominationDetails { get; set; }
        public virtual DbSet<RestaurantAggregatorWiseSale> RestaurantAggregatorWiseSales { get; set; }
        public virtual DbSet<RestaurantProductWiseSale> RestaurantProductWiseSales { get; set; }
        public virtual DbSet<RestaurantCategoryWiseSale> RestaurantCategoryWiseSales { get; set; }
        public virtual DbSet<CustomerFavouriteBranches> CustomerFavouriteBranches { get; set; }
        public virtual DbSet<SparePartAvailableRequests> SparePartAvailableRequests { get; set; }
        public virtual DbSet<GarageCustomerInvoice> GarageCustomerInvoice { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public virtual DbSet<SurveyOption> SurveyOptions { get; set; }
        public virtual DbSet<Emoji> Emojis { get; set; }
        public virtual DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }
        public virtual DbSet<GarageBannerSetting> GarageBannerSetting { get; set; }
        public virtual DbSet<GarageMenu> GarageMenu { get; set; }
        public virtual DbSet<GarageMenuManagement> GarageMenuManagement { get; set; }
        public virtual DbSet<GarageContentManagement> GarageContentManagement { get; set; }
        public virtual DbSet<GarageServiceManagement> GarageServiceManagement { get; set; }
        public virtual DbSet<GarageTeamManagement> GarageTeamManagement { get; set; }
        public virtual DbSet<Expertise> Expertise { get; set; }
        public virtual DbSet<GarageExpertiseManagement> GarageExpertiseManagement { get; set; }
        public virtual DbSet<GarageTestimonials> GarageTestimonials { get; set; }
        public virtual DbSet<GarageBlog> GarageBlog { get; set; }
        public virtual DbSet<GarageSubscribers> GarageSubscribers { get; set; }
        public virtual DbSet<GaragePartnersManagement> GaragePartnersManagement { get; set; }
        public virtual DbSet<GarageCustomerAppointment> GarageCustomerAppointment { get; set; }
        public virtual DbSet<GarageAppointmentManagement> GarageAppointmentManagement { get; set; }
        public virtual DbSet<GarageCareers> GarageCareers { get; set; }
        public virtual DbSet<GarageCustomerFeedback> GarageCustomerFeedback { get; set; }
        public virtual DbSet<CustomerFeedbackReview> CustomerFeedbackReviews { get; set; }
        public virtual DbSet<CustomerFeedbackReviewOption> CustomerFeedbackReviewOptions { get; set; }
        public virtual DbSet<SparePartTransactionHistory> SparePartTransactionHistory { get; set; }
        public virtual DbSet<GarageProject> GarageProject { get; set; }
        public virtual DbSet<GarageProjectImages> GarageProjectImages { get; set; }
        public virtual DbSet<ClientIndustries> ClientIndustries { get; set; }
        public virtual DbSet<ClientSections> ClientSections { get; set; }
        public virtual DbSet<ClientTypes> ClientTypes { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<VendorDocument> VendorDocument { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<ClientModulePurchases> ClientModulePurchases { get; set; }
        public virtual DbSet<ModulePurchaseDetails> ModulePurchaseDetails { get; set; }
        public virtual DbSet<ClientDomainSuggestions> ClientDomainSuggestions { get; set; }
        public virtual DbSet<ClientContentMedia> ClientContentMedias { get; set; }
        public virtual DbSet<ClientEmails> ClientEmails { get; set; }
        public virtual DbSet<ClientModules> ClientModules { get; set; }
        public virtual DbSet<ClientModulePurchaseTransactions> ClientModulePurchaseTransactions { get; set; }
        public virtual DbSet<VendorOTPVerification> VendorOTPVerification { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Precision
            builder.Entity<Garage>().Property(e => e.Latitude).HasPrecision(9, 6);
            builder.Entity<Garage>().Property(e => e.Longitude).HasPrecision(9, 6);
            builder.Entity<GarageBusinessSetting>().Property(e => e.Latitude).HasPrecision(9, 6);
            builder.Entity<GarageBusinessSetting>().Property(e => e.Longitude).HasPrecision(9, 6);

            builder.Entity<SparePartsDealer>().Property(e => e.Latitude).HasPrecision(9, 6);
            builder.Entity<SparePartsDealer>().Property(e => e.Longitude).HasPrecision(9, 6);

            builder.Entity<SparePartRequestQuote>().Property(e => e.OriginalPrice).HasPrecision(10, 2);
            builder.Entity<SparePartRequestQuote>().Property(e => e.TejariPrice).HasPrecision(10, 2);
            builder.Entity<SparePartRequestQuote>().Property(e => e.FougitoCommision).HasPrecision(10, 2);
            builder.Entity<SparePartRequestQuote>().Property(e => e.TotalPrice).HasPrecision(10, 2);
            builder.Entity<SparePartRequestQuote>().Property(e => e.DeliveryCharges).HasPrecision(10, 2);

            builder.Entity<SparePartRequest>().Property(e => e.Price).HasPrecision(10, 2);
            builder.Entity<SparePartRequest>().Property(e => e.FougitoCommision).HasPrecision(10, 2);
            builder.Entity<SparePartRequest>().Property(e => e.TotalPrice).HasPrecision(10, 2);
            builder.Entity<SparePartRequest>().Property(e => e.DeliveryCharges).HasPrecision(10, 2);
            builder.Entity<SparePartRequest>().Property(e => e.Latitude).HasPrecision(9, 6);
            builder.Entity<SparePartRequest>().Property(e => e.Longitude).HasPrecision(9, 6);

            builder.Entity<SparePartDeliveries>().Property(e => e.PickupLatitude).HasPrecision(9, 6);
            builder.Entity<SparePartDeliveries>().Property(e => e.PickupLongitude).HasPrecision(9, 6);
            builder.Entity<SparePartDeliveries>().Property(e => e.DropLatitude).HasPrecision(9, 6);
            builder.Entity<SparePartDeliveries>().Property(e => e.DropLongitude).HasPrecision(9, 6);

            builder.Entity<RestaurantBranch>().Property(e => e.Latitude).HasPrecision(9, 6);
            builder.Entity<RestaurantBranch>().Property(e => e.Longitude).HasPrecision(9, 6);

            builder.Entity<Item>().Property(e => e.Price).HasPrecision(10, 2);

            builder.Entity<CustomerAddress>().Property(e => e.Latitude).HasPrecision(9, 6);
            builder.Entity<CustomerAddress>().Property(e => e.Longitude).HasPrecision(9, 6);

            builder.Entity<SupplierItem>().Property(e => e.RegularPrice).HasPrecision(10, 2);
            builder.Entity<SupplierItem>().Property(e => e.SalePrice).HasPrecision(10, 2);

            builder.Entity<RestaurantTaxSetting>().Property(e => e.TAXPercent).HasPrecision(10, 2);

            builder.Entity<Aggregator>().Property(e => e.TAXPercent).HasPrecision(10, 2);

            builder.Entity<CurrencyNote>().Property(e => e.Value).HasPrecision(10, 2);

            builder.Entity<RestaurantBalanceSheet>().Property(e => e.OpeningBalance).HasPrecision(10, 2);
            builder.Entity<RestaurantBalanceSheet>().Property(e => e.ClosingBalance).HasPrecision(10, 2);
            builder.Entity<RestaurantBalanceSheet>().Property(e => e.TotalCashSale).HasPrecision(10, 2);
            builder.Entity<RestaurantBalanceSheet>().Property(e => e.TotalCardSale).HasPrecision(10, 2);
            builder.Entity<RestaurantBalanceSheet>().Property(e => e.AggregatorCashSale).HasPrecision(10, 2);
            builder.Entity<RestaurantBalanceSheet>().Property(e => e.AggregatorOnlineSale).HasPrecision(10, 2);
            builder.Entity<RestaurantBalanceSheet>().Property(e => e.NetTotal).HasPrecision(10, 2);
            builder.Entity<RestaurantBalanceSheet>().Property(e => e.GrandTotal).HasPrecision(10, 2);
            builder.Entity<RestaurantBalanceSheet>().Property(e => e.TotalTax).HasPrecision(10, 2);
            builder.Entity<RestaurantBalanceSheet>().Property(e => e.TotalCreditSale).HasPrecision(10, 2);
            builder.Entity<RestaurantBalanceSheet>().Property(e => e.DeliveryCharges).HasPrecision(10, 2);
            builder.Entity<RestaurantBalanceSheet>().Property(e => e.TotalDiscount).HasPrecision(10, 2);
            //builder.Entity<RestaurantBalanceSheet>().Property(e => e.TotalSaleWithoutTax).HasPrecision(10, 2);
            //builder.Entity<RestaurantBalanceSheet>().Property(e => e.GrossSaleWithTax).HasPrecision(10, 2);

            builder.Entity<RestaurantCashDenomination>().Property(e => e.TotalAmount).HasPrecision(10, 2);
            builder.Entity<RestaurantCashDenomination>().Property(e => e.PositiveVariance).HasPrecision(10, 2);
            builder.Entity<RestaurantCashDenomination>().Property(e => e.NegativeVariance).HasPrecision(10, 2);

            builder.Entity<RestaurantAggregatorWiseSale>().Property(e => e.Amount).HasPrecision(10, 2);
            builder.Entity<RestaurantProductWiseSale>().Property(e => e.Amount).HasPrecision(10, 2);
            builder.Entity<RestaurantCategoryWiseSale>().Property(e => e.Amount).HasPrecision(10, 2);

            builder.Entity<Order>().Property(e => e.CashReceived).HasPrecision(10, 2);
            builder.Entity<Order>().Property(e => e.Change).HasPrecision(10, 2);
            builder.Entity<Order>().Property(e => e.CardAmount).HasPrecision(10, 2);
            builder.Entity<Order>().Property(e => e.Latitude).HasPrecision(9, 6);
            builder.Entity<Order>().Property(e => e.Longitude).HasPrecision(9, 6);
            builder.Entity<Order>().Property(e => e.Amount).HasPrecision(10, 2);
            builder.Entity<Order>().Property(e => e.TaxPercent).HasPrecision(10, 2);
            builder.Entity<Order>().Property(e => e.TaxAmount).HasPrecision(10, 2);
            builder.Entity<Order>().Property(e => e.DiscountPercent).HasPrecision(10, 2);
            builder.Entity<Order>().Property(e => e.DiscountAmount).HasPrecision(10, 2);
            builder.Entity<Order>().Property(e => e.DeliveryCharges).HasPrecision(10, 2);
            builder.Entity<Order>().Property(e => e.RedeemAmount).HasPrecision(10, 2);
            builder.Entity<Order>().Property(e => e.TotalAmount).HasPrecision(10, 2);
            builder.Entity<Order>().Property(e => e.CouponDiscount).HasPrecision(10, 2);
            builder.Entity<Order>().Property(e => e.DeliveryStaffCash).HasPrecision(10, 2);
            builder.Entity<Order>().Property(e => e.EditCount).HasDefaultValue(1); // 1 equal to new order
            builder.Entity<Order>().Property(e => e.IsOnline).HasDefaultValue(false); // False by default

            builder.Entity<OrderDetail>().Property(e => e.EditCount).HasDefaultValue(1); // 1 equal to new detail

            builder.Entity<ClientModulePurchases>().Property(e => e.CouponDiscountAmount).HasPrecision(10, 2);
            builder.Entity<ClientModulePurchases>().Property(e => e.CouponDiscountPercentage).HasPrecision(10, 2);
            builder.Entity<ClientModulePurchases>().Property(e => e.DiscountPercentage).HasPrecision(10, 2);
            builder.Entity<ClientModulePurchases>().Property(e => e.DiscountAmount).HasPrecision(10, 2);
            builder.Entity<ClientModulePurchases>().Property(e => e.TaxPercentage).HasPrecision(10, 2);
            builder.Entity<ClientModulePurchases>().Property(e => e.TaxAmount).HasPrecision(10, 2);
            builder.Entity<ClientModulePurchases>().Property(e => e.Total).HasPrecision(10, 2);
            builder.Entity<ClientModulePurchases>().Property(e => e.SubTotal).HasPrecision(10, 2);

            builder.Entity<ModulePurchaseDetails>().Property(e => e.TotalPrice).HasPrecision(10, 2);

            //Global query filtering 
            builder.Entity<Areas>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageAward>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantWaiter>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Item>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Menu>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<ItemOption>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<ItemOptionValue>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<CarMake>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<CarModel>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Garage>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartsDealer>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<ServiceStaff>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Country>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<City>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartRequest>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<DeliveryStaff>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Blogs>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantDeliveryStaff>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantCashierStaff>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Coupon>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Category>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Customer>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Subscriber>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantBranch>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Restaurant>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageRating>().HasQueryFilter(x => x.ArchivedDate == null && x.Status != Enum.GetName(typeof(Status), Status.Rejected));
            builder.Entity<RestaurantRating>().HasQueryFilter(x => x.ArchivedDate == null && x.Status != Enum.GetName(typeof(Status), Status.Rejected));
            builder.Entity<MenuItem>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantServiceStaff>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Order>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<AppUser>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Supplier>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SupplierCoupon>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SupplierItemCategory>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SupplierItem>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SupplierPackage>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SupplierOrder>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SupplierOrderDetail>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SupplierItemImage>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantPrinterSetting>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantTaxSetting>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantUserLogManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantManager>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantKitchenManager>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantTable>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantTableReservation>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantReservation>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<RestaurantBalanceSheet>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Ticket>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<TicketDocument>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<TicketMessages>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartRequestQuote>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Survey>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SurveyQuestion>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageBranchBusinessSetting>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SurveyOption>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<CustomerFeedback>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Aggregator>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<CardScheme>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageBannerSetting>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageMenu>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageMenuManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageContentManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageServiceManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageTeamManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<Expertise>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageExpertiseManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageExpertise>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageTestimonials>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageBlog>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageAppointmentManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GaragePartnersManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageCustomerAppointment>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageCareers>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageCustomerFeedback>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageCustomerInvoice>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageProject>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartTransactionHistory>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartBannerSetting>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartMenu>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartMenuManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartContentManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartServiceManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartTeamManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartExpertiseManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartExpertise>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartTestimonial>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartBlog>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartAppointmentManagement>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartCustomerAppointment>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartCareer>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartCustomerFeedback>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<GarageProject>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<BlogCategory>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<SparePartFAQ>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<ClientIndustries>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<ClientSections>().HasQueryFilter(x => x.ArchivedDate == null);
            builder.Entity<ClientTypes>().HasQueryFilter(x => x.ArchivedDate == null);

            //View mapping
            builder
            .Entity<PopularCategories>(e =>
                {
                    e.HasNoKey();
                    e.ToView("vwPopularCategories");
                });

            builder
                .Entity<TrendingItem>(e =>
                {
                    e.HasNoKey();
                    e.ToView("vwTrendingItems");
                });

            builder
                .Entity<SparePartAvailableRequests>(e =>
                {
                    e.HasNoKey();
                    e.ToView("vwSparePartAvailableRequests");
                });

            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RolesConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (EntityEntry Entry in ChangeTracker.Entries())
                SetAuditValues(Entry);

            return await base.SaveChangesAsync();
        }

        private void SetAuditValues(EntityEntry Entity)
        {
            String UserID = "system";

            // Hang Fire jobs may not have a context
            if (_httpContextAccessor.HttpContext != null)
            {
                Claim UserClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid);

                // it will be null if annonymous access
                if (UserClaim != null)
                    UserID = UserClaim.Value;
            }

            MemberEntry CreatedBy = Entity.Members.Where(x => x.Metadata.Name == "CreatedBy")
                                                    .FirstOrDefault();
            MemberEntry CreationDate = Entity.Members.Where(x => x.Metadata.Name == "CreationDate")
                                                        .FirstOrDefault();

            if (Entity.State == EntityState.Added)
            {
                if (CreatedBy != null) CreatedBy.CurrentValue = UserID;
                if (CreationDate != null) CreationDate.CurrentValue = DateTime.UtcNow.ToDubaiDateTime();
            }
            else
            {
                MemberEntry ModifiedBy = Entity.Members.Where(x => x.Metadata.Name == "ModifiedBy")
                                                       .FirstOrDefault();
                MemberEntry ModifiedDate = Entity.Members.Where(x => x.Metadata.Name == "ModificationDate")
                                                         .FirstOrDefault();

                MemberEntry ArchivedDate = Entity.Members.Where(x => x.Metadata.Name == "ArchivedDate")
                                                        .FirstOrDefault();
                MemberEntry ArchivedBy = Entity.Members.Where(x => x.Metadata.Name == "ArchivedBy")
                                                        .FirstOrDefault();

                if (ArchivedDate != null)
                {
                    if (ArchivedDate.IsModified && (ArchivedBy.CurrentValue == null))
                        ArchivedBy.CurrentValue = UserID;
                }

                if (ModifiedBy != null) ModifiedBy.CurrentValue = UserID;
                if (ModifiedDate != null) ModifiedDate.CurrentValue = DateTime.UtcNow.ToDubaiDateTime();

                if (CreatedBy != null) CreatedBy.IsModified = false;
                if (CreationDate != null) CreationDate.IsModified = false;
            }
        }
    }
}
