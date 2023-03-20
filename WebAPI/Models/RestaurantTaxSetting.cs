using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class RestaurantTaxSetting : GeneralSchema
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		[MaxLength(50, ErrorMessage = "Name length must be less than 50 characters")]
		public string Name { get; set; }
		public decimal TAXPercent { get; set; }
		public string Description { get; set; }
        public DateTime CreationDate { get; set; }


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
