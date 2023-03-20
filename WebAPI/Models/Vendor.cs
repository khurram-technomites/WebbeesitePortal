using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Vendor : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string NameAsPerTradeLicense { get; set; }
        [MaxLength(250, ErrorMessage = "Address length must be less than 250 characters")]
        public string NameArAsPerTradeLicense { get; set; }
        [MaxLength(250, ErrorMessage = "Address length must be less than 250 characters")]

        public string TradeLicenseNo { get; set; }
        public string FullName { get; set; }
        public string ContactNumber1 { get; set; }
        [MaxLength(25, ErrorMessage = "Address length must be less than 25 characters")]
        public string ContactNumber2 { get; set; }
        [MaxLength(25, ErrorMessage = "Address length must be less than 25 characters")]
        public string Whatsapp { get; set; }
        [MaxLength(25, ErrorMessage = "Address length must be less than 25 characters")]
        public string OfficeAddress { get; set; }
        [MaxLength(4000, ErrorMessage = "Address length must be less than 4000 characters")]
        public string Website { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [ForeignKey(nameof(City))]
        public long? CityId { get; set; }

        [ForeignKey(nameof(Country))]
        public long? CountryId { get; set; }

        public string TrnNumber { get; set; }
      

        public string Email { get; set; }
        public string Status { get; set; }

        public string RejectionReason { get; set; }

        public AppUser User { get; set; }
        public City City{ get; set; }
        public Country Country { get; set; }
    }
}
