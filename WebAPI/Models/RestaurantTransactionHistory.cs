using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class RestaurantTransactionHistory : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string NameOnCard { get; set; }
        public string MaskCardNo { get; set; }
        public string TransactionStatus { get; set; }
        public decimal Amount { get; set; }
        [ForeignKey(nameof(SupplierOrder))]
        public long SupplierOrderId { get; set; }
        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }

        public SupplierOrder SupplierOrder { get; set; }
        public Restaurant Restaurant { get; set; }

    }
}
