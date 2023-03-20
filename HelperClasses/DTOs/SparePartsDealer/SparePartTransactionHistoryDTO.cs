using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartsDealer
{
    public class SparePartTransactionHistoryDTO
    {
        public long Id { get; set; }
        public long SparePartsDealerId { get; set; }
        public string Type { get; set; }
        public string TransactionStatus { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreationDate { get; set; }
        public long SparePartRequestQuoteId { get; set; }
        public SparePartRequestQuoteDTO SparePartRequestQuote { get; set; }
        public SparePartsDealerDTO SparePartsDealer { get; set; }
    }
}
