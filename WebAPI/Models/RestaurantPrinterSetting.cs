using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class RestaurantPrinterSetting : GeneralSchema
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		[MaxLength(50, ErrorMessage = "Name length must be less than 50 characters")]
		public string Name { get; set; }
		[MaxLength(50, ErrorMessage = "Type length must be less than 50 characters")]
		public string Type { get; set; }
		[MaxLength(50, ErrorMessage = "IP length must be less than 50 characters")]
		public string IP { get; set; }
		public int Port { get; set; }
		public bool IsDefault { get; set; }
		[MaxLength(255, ErrorMessage = "DeviceID length must be less than 255 characters")]
		public string DeviceID { get; set; }
		[MaxLength(100, ErrorMessage = "Status length must be less than 100 characters")]
		public string Status { get; set; }

		/*Foreign Keys*/
		[ForeignKey("Restaurant")]
		public long RestaurantId { get; set; }
		[ForeignKey(nameof(RestaurantBranch))]
		public long RestaurantBranchId { get; set; }

		/*Relationships*/
		public Restaurant Restaurant { get; set; }
		public RestaurantBranch RestaurantBranch { get; set; }
	}
}
