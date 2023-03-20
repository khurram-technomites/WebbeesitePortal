using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartExpertise : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(SparePartExpertiseManagement))]
        public long SparePartExpertiseManagementId { get; set; }
        [ForeignKey(nameof(Expertise))]
        public long ExpertiseId { get; set; }
        public SparePartExpertiseManagement SparePartExpertiseManagement { get; set; }
        public Expertise Expertise { get; set; }
    }
}
