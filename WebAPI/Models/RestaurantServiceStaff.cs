﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class RestaurantServiceStaff : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(20, ErrorMessage = "FirstName length must be less than 20 characters")]
        public string FirstName { get; set; }
        [MaxLength(20, ErrorMessage = "LastName length must be less than 20 characters")]
        public string LastName { get; set; }
        [MaxLength(50, ErrorMessage = "Email length must be less than 50 characters")]
        public string Email { get; set; }
        [MaxLength(20, ErrorMessage = "PhoneNumber length must be less than 20 characters")]
        public string PhoneNumber { get; set; }
        public string Logo { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        
        public AppUser User { get; set; }

        [ForeignKey("Restaurant")]
        public long RestaurantId { get; set; }

        [MaxLength(100, ErrorMessage = "Status length must be less than 100 characters")]
        public string Status { get; set; }
        public Restaurant Restaurant { get; set; }

        [ForeignKey("RestaurantBranch")]
        public long RestaurantBranchId { get; set; }

        public RestaurantBranch RestaurantBranch { get; set; }
    }
}
