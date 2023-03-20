using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageRepairSpecification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey("Garage")]
        public long GarageId { get; set; }
        [ForeignKey(nameof(CarMake))]
        public long CarMakeId { get; set; }
        [ForeignKey(nameof(CarModel))]
        public long? CarModelId { get; set; }

        public Garage Garage { get; set; }
        public CarMake CarMake { get; set; }
        public CarModel CarModel { get; set; }
    }
}
