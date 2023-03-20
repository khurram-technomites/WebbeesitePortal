using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ItemCategory : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(200, ErrorMessage = "Name length must be less than 200 characters")]
        public string Name { get; set; }
        [MaxLength(200, ErrorMessage = "NameAr length must be less than 200 characters")]
        public string NameAr { get; set; }
        [MaxLength(4000, ErrorMessage = "Description length must be less than 4000 characters")]
        public string Description { get; set; }
        [MaxLength(4000, ErrorMessage = "DescriptionAr length must be less than 4000 characters")]
        public string DescriptionAr { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
