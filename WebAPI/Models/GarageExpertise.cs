using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageExpertise : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(GarageExpertiseManagement))]
        public long GarageExpertiseManagementId { get; set; }
        [ForeignKey(nameof(Expertise))]
        public long ExpertiseId { get; set; }
        public GarageExpertiseManagement GarageExpertiseManagement { get; set; }
        public Expertise Expertise { get; set; }
    }
}
