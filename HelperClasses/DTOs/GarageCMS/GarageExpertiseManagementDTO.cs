using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageExpertiseManagementDTO
    {
        public GarageExpertiseManagementDTO()
        {
            GarageExpertise = new List<GarageExpertiseDTO>();
        }
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageDTO Garage { get; set; }
        public List<GarageExpertiseDTO> GarageExpertise { get; set; }
    }
}
