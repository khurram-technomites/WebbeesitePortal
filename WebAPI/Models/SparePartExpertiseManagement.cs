using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartExpertiseManagement : GeneralSchema
    {
        public SparePartExpertiseManagement()
        {
            SparePartExpertise = new HashSet<SparePartExpertise>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(SparePartDealer))]
        public long SparePartDealerId { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public SparePartsDealer SparePartDealer { get; set; }
        public ICollection<SparePartExpertise> SparePartExpertise { get; set; }
    }
}
