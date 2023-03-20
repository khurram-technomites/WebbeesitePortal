
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Areas : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(50, ErrorMessage = "Name length must be less than 50 characters")]
        public string Name { get; set; }
        [MaxLength(50, ErrorMessage = "NameAr length must be less than 50 characters")]
        public string NameAr { get; set; }

        //[ForeignKey("Country")]
        //public long CountryId { get; set; }
        //public Countries Country { get; set; }

        [ForeignKey(nameof(City))]
        public long CityId { get; set; }
        public City City { get; set; }
    }
}
