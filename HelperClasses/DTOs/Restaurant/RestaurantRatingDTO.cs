using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantRatingDTO
    {
        public DateTime CreatedDate { get; set; }
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public string UserId { get; set; }
        public long? OrderId { get; set; }
        public long RestaurantId { get; set; }
        public string Status { get; set; }
        public bool ShowOnWebsite { get; set; }
        public DateTime? PublishedDatetime { get; set; }
        public string DateDifference
        {
            get
            {
                if (PublishedDatetime.HasValue)
                {
                    DateTime current = DateTime.UtcNow;
                    double difference = (current - PublishedDatetime.Value).TotalMinutes;

                    if (difference < 60)
                        return $"Published {Math.Floor(difference)} minutes ago";

                    if (difference > 60 && difference < 1440)
                        return $"Published {Math.Floor(difference / 60)} hours ago";

                    else
                        return $"Published {Math.Floor(difference / 1440)} days ago";
                }

                return "";
            }
            set { }
        }
        public AppUserDTO User { get; set; }
        public RestaurantDTO Restaurant { get; set; }
    }
}
