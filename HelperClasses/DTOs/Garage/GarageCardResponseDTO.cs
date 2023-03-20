using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Garage
{
    public class GarageCardResponseDTO
    {
        public long Id { get; set; }
        public string NameAsPerTradeLicense { get; set; }
        public string Logo { get; set; }
        public string ThumbnailImage { get; set; }
        public string Address { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
        public string Distance { get; set; }
        public double AvgRating { get; set; }
        public float RatingCount { get; set; }
        public string Slug { get; set; }
        public string Timing
        {
            get
            {
                if (string.IsNullOrEmpty(OpeningTime) && string.IsNullOrEmpty(ClosingTime))
                    return null;

                return OpeningTime + " - " + ClosingTime;
            }
        }
        public bool IsGarageCloseToday
        {
            get
            {
                return string.IsNullOrEmpty(Timing);
            }
        }
    }
}
