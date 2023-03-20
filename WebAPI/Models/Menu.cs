using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Menu : GeneralSchema
    {
        public Menu()
        {
            MenuItem = new HashSet<MenuItem>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(100, ErrorMessage = "Name must be less than 100 characters")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "NameAr must be less than 100 characters")]
        public string NameAr { get; set; }

        [MaxLength(4000, ErrorMessage = "Description must be less than 4000 characters")]
        public string Description { get; set; }

        [MaxLength(4000, ErrorMessage = "DescriptionAr must be less than 4000 characters")]
        public string DescriptionAr { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public DateTime? BreakFastStartTime { get; set; }
        public DateTime? BreakFastEndTime { get; set; }

        public DateTime? LunchStartTime { get; set; }
        public DateTime? LunchEndTime { get; set; }

        public DateTime? DinnerStartTime { get; set; }
        public DateTime? DinnerEndTime { get; set; }

        [MaxLength(100, ErrorMessage = "Status must be less than 100 characters")]
        public string Status { get; set; }
        public string Image { get; set; }
        public bool IsPeriodic { get; set; }
        public int Position { get; set; }

        [ForeignKey(nameof(RestaurantBranch))]
        public long? RestaurantBranchId { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public long? RestaurantId { get; set; }
        public RestaurantBranch RestaurantBranch { get; set; }
        public Restaurant Restaurant { get; set; }
        public ICollection<MenuItem> MenuItem { get; set; }
    }
}
