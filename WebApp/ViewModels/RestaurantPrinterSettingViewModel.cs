using System;

namespace WebApp.ViewModels
{
	public class RestaurantPrinterSettingViewModel
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string IP { get; set; }
		public int Port { get; set; }
		public bool IsDefault { get; set; }
		public string DeviceID { get; set; }
		public string Status { get; set; }
		public long RestaurantId { get; set; }
		public DateTime CreationDate { get; set; }
		public long RestaurantBranchId { get; set; }
		public RestaurantViewModel Restaurant { get; set; }
		public RestaurantBranchViewModel RestaurantBranch { get; set; }
	}
}
