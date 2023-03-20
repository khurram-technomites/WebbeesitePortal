﻿using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Supplier
{
    public class SupplierOrderDTO
    {
        public SupplierOrderDTO()
        {
            SupplierOrderDetails = new List<SupplierOrderDetailDTO>();
        }

        public long Id { get; set; }
        public string OrderNo { get; set; }
        public long SupplierId { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string DeliveryAddress { get; set; }
        public string Status { get; set; }
        public bool IsPaid { get; set; }
        public string Currency { get; set; }
        public long RestaurantId { get; set; }
        public string RestauantContact { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantEmail { get; set; }
        public string RestaurantStreetAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal TotalAmount { get; set; }
        public string CouponCode { get; set; }
        public string NoteToRider { get; set; }
        public decimal CouponDiscount { get; set; }
        public DateTime OrderRequiredDate { get; set; }
        public DateTime OrderRequiredDate2 { get; set; }
        public DateTime CreationDate { get; set; }
        public TimeSpan? OrderRequiredTime { get; set; }
        public RestaurantDTO Restaurant { get; set; }
        public SupplierDTO Supplier { get; set; }
        public List<SupplierOrderDetailDTO> SupplierOrderDetails { get; set; }
        public RestaurantTransactionHistoryDTO RestaurantTransactionHistory { get; set; }
    }
}
