
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
	public class Restaurant : GeneralSchema
	{
		public Restaurant()
		{
			RestaurantServiceStaffs = new HashSet<RestaurantServiceStaff>();
			RestaurantBranches = new HashSet<RestaurantBranch>();
			ItemCategories = new HashSet<ItemCategory>();
			ItemOptions = new HashSet<ItemOption>();
			Items = new HashSet<Item>();
			RestaurantRatings = new HashSet<RestaurantRating>();
			RestaurantDeliveryStaffs = new HashSet<RestaurantDeliveryStaff>();
			Categories = new HashSet<Category>();
			RestaurantImages = new HashSet<RestaurantImages>();
			Coupons = new HashSet<Coupon>();
			RestaurantCustomers = new HashSet<RestaurantCustomer>();
			RestaurantBannerSettings = new HashSet<RestaurantBannerSetting>();
			RestaurantDocuments = new HashSet<RestaurantDocument>();
			Orders = new HashSet<Order>();
			Menus = new HashSet<Menu>();
			RestaurantSubscribers = new HashSet<RestaurantSubscriber>();
			SupplierOrders = new HashSet<SupplierOrder>();

			RestaurantCashierStaffs = new HashSet<RestaurantCashierStaff>();
			RestaurantPrinterSettings = new HashSet<RestaurantPrinterSetting>();
			RestaurantCardSchemes = new HashSet<RestaurantCardScheme>();
			RestaurantTaxSettings = new HashSet<RestaurantTaxSetting>();
			RestaurantAggregators = new HashSet<RestaurantAggregator>();
			RestaurantUserLogManagements = new HashSet<RestaurantUserLogManagement>();
			RestaurantKitchenManagers = new HashSet<RestaurantKitchenManager>();
			RestaurantManagers = new HashSet<RestaurantManager>();
			RestaurantTables = new HashSet<RestaurantTable>();
			RestaurantBalanceSheets = new HashSet<RestaurantBalanceSheet>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		[MaxLength(200, ErrorMessage = "NameAsPerTradeLicense length must be less than 200 characters")]
		public string NameAsPerTradeLicense { get; set; }
		[MaxLength(200, ErrorMessage = "NameArAsPerTradeLicense length must be less than 200 characters")]
		public string NameArAsPerTradeLicense { get; set; }
		public string Logo { get; set; }
		public string SecondaryLogo { get; set; }
        public string ThumbnailImage { get; set; }
        [MaxLength(500, ErrorMessage = "Website length must be less than 500 characters")]
		public string Website { get; set; }
		[MaxLength(100, ErrorMessage = "Email length must be less than 100 characters")]
		public string Email { get; set; }
		[MaxLength(20, ErrorMessage = "PhoneNumber length must be less than 20 characters")]
		public string PhoneNumber { get; set; }
		[MaxLength(20, ErrorMessage = "WhatsappNumber length must be less than 20 characters")]
		public string WhatsappNumber { get; set; }
		[MaxLength(500, ErrorMessage = "Facebook length must be less than 500 characters")]
		public string Facebook { get; set; }
		[MaxLength(500, ErrorMessage = "Twitter length must be less than 500 characters")]
		public string Twitter { get; set; }
		[MaxLength(500, ErrorMessage = "Status length must be less than 20 characters")]
		public string Instagram { get; set; }
		public string Linkedin { get; set; }
		public string Status { get; set; }
		public string ReferenceCode { get; set; }
		public decimal TaxPercent { get; set; }
		public string UserId { get; set; }
		public string Slug { get; set; }
		public string Description { get; set; }
		public string DescriptionImage { get; set; }
		public string UniqueKey { get; set; }
		public string Origin { get; set; }
		public string ThemeColor { get; set; }
		public string Bank { get; set; }
		public string Favicon { get; set; }
		public string BankAccountHolderName { get; set; }
		public string BankAccountNumber { get; set; }
		public string IBAN { get; set; }
		public string VATRegistrationNumber { get; set; }
        public bool IsCashierAllowed { get; set; }
        public bool IsPartnerAllowed { get; set; }
        public bool IsKitchenManagerAllowed { get; set; }
        public bool IsWaiterAllowed { get; set; }
        public string OrderPaymentType { get; set; }

        public string SupplierCode { get; set; }
		public AppUser User { get; set; }
		public ICollection<RestaurantBranch> RestaurantBranches { get; set; }
		public ICollection<Menu> Menus { get; set; }
		public ICollection<ItemCategory> ItemCategories { get; set; }
		public ICollection<Item> Items { get; set; }
		public ICollection<ItemOption> ItemOptions { get; set; }
		public ICollection<RestaurantRating> RestaurantRatings { get; set; }
		public ICollection<RestaurantDeliveryStaff> RestaurantDeliveryStaffs { get; set; }
		public ICollection<Category> Categories { get; set; }
		public ICollection<Coupon> Coupons { get; set; }
		public ICollection<RestaurantCustomer> RestaurantCustomers { get; set; }
		public ICollection<Order> Orders { get; set; }
		public ICollection<RestaurantImages> RestaurantImages { get; set; }
		public ICollection<RestaurantBannerSetting> RestaurantBannerSettings { get; set; }
		public ICollection<RestaurantServiceStaff> RestaurantServiceStaffs { get; set; }
		public ICollection<RestaurantDocument> RestaurantDocuments { get; set; }
		public RestaurantContentManagement RestaurantContentManagement { get; set; }
		public ICollection<RestaurantSubscriber> RestaurantSubscribers { get; set; }
		public ICollection<SupplierOrder> SupplierOrders { get; set; }
		public ICollection<RestaurantTransactionHistory> RestaurantTransactionHistories { get; set; }

		public ICollection<RestaurantCashierStaff> RestaurantCashierStaffs { get; set; }
		public ICollection<RestaurantPrinterSetting> RestaurantPrinterSettings { get; set; }
		public ICollection<RestaurantCardScheme> RestaurantCardSchemes { get; set; }
		public ICollection<RestaurantTaxSetting> RestaurantTaxSettings { get; set; }
		public ICollection<RestaurantAggregator> RestaurantAggregators { get; set; }
		public ICollection<RestaurantUserLogManagement> RestaurantUserLogManagements { get; set; }
		public ICollection<RestaurantKitchenManager> RestaurantKitchenManagers { get; set; }
		public ICollection<RestaurantManager> RestaurantManagers { get; set; }
		public ICollection<RestaurantTable> RestaurantTables { get; set; }
		public ICollection<RestaurantBalanceSheet> RestaurantBalanceSheets { get; set; }

	}
}
