using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
	public class RestaurantPrinterSettingDTO
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string IP { get; set; }
		public int Port { get; set; }
		public bool IsDefault { get; set; }
		public string DeviceID { get; set; }
		public DateTime CreationDate { get; set; }
		public string Status { get; set; }

		public long RestaurantId { get; set; }
		public long RestaurantBranchId { get; set; }

		/*Relationships*/
		public RestaurantDTO Restaurant { get; set; }
		public RestaurantBranchDTO RestaurantBranch { get; set; }
	}
}
