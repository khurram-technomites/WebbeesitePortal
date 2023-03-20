﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class DeliveryStaff : GeneralSchema
    {
        public DeliveryStaff()
        {
            SparePartDeliveries = new HashSet<SparePartDeliveries>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(20, ErrorMessage = "FirstName length must be 20 less than characters")]
        public string FirstName { get; set; }
        [MaxLength(20, ErrorMessage = "LastName length must be 20 less than characters")]
        public string LastName { get; set; }
        [MaxLength(50, ErrorMessage = "Email length must be 50 less than characters")]
        public string Email { get; set; }
        [MaxLength(20, ErrorMessage = "Phone Number length must be 20 less than characters")]
        public string PhoneNumber { get; set; }
        public string Logo { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public string Status { get; set; }
        public AppUser User { get; set; }
        public ICollection<SparePartDeliveries> SparePartDeliveries { get; set; }
    }
}