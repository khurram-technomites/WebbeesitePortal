using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageExpertiseDTO
    {
        public long Id { get; set; }
        public long GarageExpertiseManagementId { get; set; }
        public long ExpertiseId { get; set; }
        public GarageExpertiseManagementDTO GarageExpertiseManagement { get; set; }
        public ExpertiseDTO Expertise { get; set; }
    }
}
