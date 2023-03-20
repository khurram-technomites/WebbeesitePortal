using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartMenuManagement : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(SparePartsDealer))]
        public long SparePartDealerId { get; set; }
        [ForeignKey(nameof(SparePartMenu))]
        public long SparePartMenuId { get; set; }
        public int Position { get; set; }
        public string Status { get; set; }
        public SparePartsDealer SparePartsDealer { get; set; }
        public SparePartMenu SparePartMenu { get; set; }
    }
}
