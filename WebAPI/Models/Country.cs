using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Country : GeneralSchema
    {
        public Country()
        {
            Cities = new HashSet<City>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(50, ErrorMessage = "Name length must be less than 50 characters")]
        public string Name { get; set; }
        [MaxLength(50, ErrorMessage = "NameAr length must be less than 50 characters")]
        public string NameAr { get; set; }
        public string Status { get; set; }
        public ICollection<City> Cities { get; set; }

    }
}
