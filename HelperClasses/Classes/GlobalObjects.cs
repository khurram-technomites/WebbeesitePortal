using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.Classes
{
    public enum Permissions : short
    {
        Admin = -1,
        User = 1,
        Supplier = 2
    }

    public enum ImageFormats
    {
        JPEG,
        JPG,
        PNG,
        WEBP,
        RAW,
        SVG,
        GIF
    }

    public enum BlogCategory
    {
        Garage,
        SparePart
    }
    public enum Sort
    {
        ASC,
        DES
    }

    public enum ResponseMessages
    {
        Success,
        Failure,
        Error
    }

    public enum Period
    {
        Today
    }

    public enum Client
    {
        Website,
        Mobile
    }

    public enum Status
    {
        Draft,
        Published,
        Unpublished,
        Active,
        Inactive,
        Processing,
        Approved,
        Rejected,
        Pending,
        Opened,
        Closed,
        Reserved,
        Completed,
    }
    public enum StatusforGarageRequest
    {
        empty,
        Active,
        Pending,
        Pickedup,
        Delivered,
        Cancelled
    }
    public enum GarageCustomerStatus
    {
        Paid,
        UnPaid,
    }
    public enum ClientPaymentStatus
    {
        Paid,
        UnPaid,
        Void,
        Pending,
    }

    public enum BalanceSheetStatus
	{
        Opened,
        Closed,
        Completed,
    }

    public enum CustomerAddressStatus
    {
        Home,
        Office,
        Other,
    }

    public enum TableReservationStatus
    {
        Inactive,
        Active,
        Reserved,
        Completed,
        Canceled,
        Merged,
    }
    public enum ReservationStatus
    {
        Reserved,
        Completed,
        Expired,
        Canceled,
    }

    public enum ItemStock
    {
        InStock,
        OutOfStock
    }

    public enum BannerType
    {
        Banner,
        PromotionBanner,
        MenuBanner
    }
    public enum BillingPeriod
    {
        Monthly,
        Quarterly,
        HalfYearly,
        Yearly
    }
    public enum Roles
    {
        Admin,
        Customer,
        ServiceStaff,
        DeliveryStaff,
        GarageOwner,
        SparePartDealer,
        RestaurantOwner,
        AdminUser,
        RestaurantServiceStaff,
        RestaurantDeliveryStaff,
        RestaurantCashierStaff,
        RestaurantKitchenManager,
        Supplier,
        B2BManager,
        AutomobileManager,
        RestaurantManager,
        Vendor
    }

    public enum Logins
    {
        Garage,
        SparePartDealer,
        ServiceStaff,
        DeliveryStaff,
        Admin,
        Restaurant,
        RestaurantServiceStaff,
        RestaurantDeliveryStaff,
		RestaurantCashierStaff,
        RestaurantKitchenManager,
        Customer,
        Supplier,
        Vendor
	}

    public enum OrderPaymentType
    {
        Card,
        COD,
        Both 
    }

    public enum UserLogStatus
    {
        Login,
        Logout,
    }

    public enum FileType
    {
        Image,
        Document,
        Video
    }

    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Preparing,
        FoodReady,
        OnTheWay,
        Delivered,
        NotDelivered,
        Canceled,
        //Completed, Dilivered
        All
    }
    public enum OrderPaidStatus
    {
        Paid,
        UnPaid
    }

    public enum OrderDetailStatus
    {
        New,
        Canceled,
        Updated,
    }

    public enum DeliveryType
    {
        PerKilometer,
        Fixed,
        Pickup,
        Delivery
    }

    public enum OrderType
    {
        Delivery,
        Pickup,
        Online,
        DineIn
    }

    public enum OrderOrigin
    {
        POS,
        Website,
        Customer,
    }

    public enum PaymentMethod
    {
        Pending,
        Cash,
        Card,
        Partial,
        Aggregator,
        Credit
    }
    public enum PaymentStatus //PaidTo
    {
        PaidToRestaurant, //Cash
        PaidOnline, //Card
    }

    public enum SparePartRequestStatus
    {
        Pending,
        Active,
        Inactive,
        Locked, //when user selects an offer
        PickedByRider,
        Delivered
    }

    public enum DiscountType
    {
        Percentage,
        FixedAmount
    }

    public enum PrinterType
    {
        Kitchen,
        Cashier,
        Packaging
    }

    public enum WebhookEvents
    {
        [Description("WebhookInvoiceStatusChange"), Localizable(true)]
        TransactionsStatusChanged = 1,
        [Description("WebhookRefundStatusChange"), Localizable(true)]
        RefundStatusChanged = 2,
        [Description("WebhookDeposit"), Localizable(true)]
        BalanceTransferred = 3,
        [Description("WebhookSupplierStatusChange"), Localizable(true)]
        SupplierStatusChanged = 4,
    }

    public static class GlobalObjects
    {
        public static string PasswordKey = "054986542AHSLD";
    }
}
