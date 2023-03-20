using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartTransactionHistory : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(SparePartsDealer))]
        public long SparePartsDealerId { get; set; }
        public string Type { get; set; }
        public decimal Wallet { get; set; }
        public string TransactionStatus { get; set; }
        public decimal Amount { get; set; }

        [ForeignKey(nameof(SparePartRequestQuote))]
        public long SparePartRequestQuoteId { get; set; }
        public SparePartRequestQuote SparePartRequestQuote { get; set; }
        public SparePartsDealer SparePartsDealer { get; set; }
    }
}
