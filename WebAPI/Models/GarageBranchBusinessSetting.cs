using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageBranchBusinessSetting : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(500, ErrorMessage = "Title must be less than 500 characters")]

        [ForeignKey(nameof(GarageBusinessSetting))]
        public long GarageBusinessSettingId { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string CompleteAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string ContactPersonName { get; set; }
        public string Email { get; set; }
  
        public GarageBusinessSetting GarageBusinessSetting { get; set; }
    }
}
