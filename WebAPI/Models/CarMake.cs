using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class CarMake : GeneralSchema
    {
        public CarMake()
        {
            CarModels = new HashSet<CarModel>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(50, ErrorMessage = "Name length must be less than 50 characters")]
        public string Name { get; set; }
        [MaxLength(50, ErrorMessage = "NameAR length must be less than 50 characters")]
        public string NameAR { get; set; }
        [MaxLength(10, ErrorMessage = "Status length must be less than 10 characters")]
        public string Status { get; set; }
        public string Logo { get; set; }
        public ICollection<CarModel> CarModels { get; set; }
    }
}
