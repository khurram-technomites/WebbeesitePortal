
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebAPI.Models
{
    public class City : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(50, ErrorMessage = "Name length must be less than 50 characters")]
        public string Name { get; set; }
        [MaxLength(50, ErrorMessage = "NameAr length must be less than 50 characters")]
        public string NameAr { get; set; }
        public string Logo { get; set; }
        [MaxLength(10, ErrorMessage = "Status length must be less than or equals to 10 characters")]
        public string Status { get; set; }


        [ForeignKey(nameof(Country))]
        public long CountryId { get; set; }
        public Country Country { get; set; }

    }
}
