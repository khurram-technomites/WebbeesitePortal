using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebApp.ViewModels
{
    public class GarageRatingViewModel
    {
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public string UserId { get; set; }
        public long GarageId { get; set; }
        public bool IsApproved { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
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
        public GarageDTO Garage { get; set; }
    }
}
