using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartCMS
{
    public class SparePartExpertiseManagementDTO
    {
        public SparePartExpertiseManagementDTO()
        {
            SparePartExpertise = new List<SparePartExpertiseDTO> { };
        }
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate  { get; set; }
        public string ImagePath { get; set; }
        public SparePartsDealerDTO SparePartDealer { get; set; }
        public List<SparePartExpertiseDTO> SparePartExpertise { get; set; }
    }
}
