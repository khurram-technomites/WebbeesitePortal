﻿using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class RestaurantServiceStaffViewModel
    {
		public long Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string Logo { get; set; }
		public long RestaurantId { get; set; }
		public long RestaurantBranchId { get; set; }
		public string BranchName { get; set; }

		public string UserName { get; set; }
		public string Password { get; set; }
		public DateTime CreationDate { get; set; }
		public string UserId { get; set; }
		public string Status { get; set; }
		public AppUserDTO User { get; set; }
	}
}
