using HelperClasses.DTOs.GarageCMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartCMS
{
    public class SparePartExpertiseDTO
    {
        public long Id { get; set; }
        public long SparePartExpertiseManagementId { get; set; }
        public long ExpertiseId { get; set; }
        public SparePartExpertiseManagementDTO SparePartExpertiseManagement { get; set; }
        public ExpertiseDTO Expertise { get; set; }
    }
}
