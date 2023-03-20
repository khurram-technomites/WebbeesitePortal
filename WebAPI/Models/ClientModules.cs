using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebAPI.Models
{
    public class ClientModules:GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey(nameof(Garage))]
        public long ClientId { get; set; }

        [ForeignKey(nameof(Module))]
        public long ModuleId { get; set; }

        public long Quantity { get; set; }

        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; }

        public Garage Garage { get; set; }

        public Module Module { get; set; }

    }
}
