using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
	public class RestaurantCardResponseDTO
	{
		public long Id { get; set; }
		public long RestaurantId { get; set; }
		public int DeliveryTime { get; set; }
		public string BranchDeliveryType { get; set; }
		public decimal BranchDeliveryCharges { get; set; }
		public decimal ServiceDistance { get; set; }
		public string NameAsPerTradeLicense { get; set; }
		public string Logo { get; set; }
        public string ThumbnailImage { get; set; }
        public string Address { get; set; }
		public string OpeningTime { get; set; }
		public string ClosingTime { get; set; }
		public string Distance { get; set; }
		public double AvgRating { get; set; }
		public float RatingCount { get; set; }
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		public string Slug { get; set; }
		public bool IsClose { get; set; }
		public decimal MinOrderPrice { get; set; } = 60M;

		public TimeSpan? ClosingTimeSpan { get; set; }

		public string DiscountBadge
		{
			get
			{
				if (RestaurantId == 9 || RestaurantId == 10)
					return null;

				else
				{
					return "Special discount: 13%";
				}

			}
			set { }
		}
		public string Timing
		{
			get
			{
				if (string.IsNullOrEmpty(OpeningTime) && string.IsNullOrEmpty(ClosingTime))
					return null;

				return OpeningTime + " - " + ClosingTime;
			}
		}
		public bool IsRestaurantCloseToday
		{
			get
			{
				return string.IsNullOrEmpty(Timing);
			}
		}
	}
}
