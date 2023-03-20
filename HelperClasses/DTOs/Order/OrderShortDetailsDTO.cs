using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Order
{
    public class OrderShortDetailsDTO
    {

        public long Id { get; set; }
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public bool IsPaid { get; set; }
        public string Currency { get; set; }
        public DateTime? Date { get; set; }
        public string FormattedDate
        {
            get
            {
                if (Date.HasValue)
                    return Date.Value.ToString("F");

                return "";
            }
            set { }
        }
    }
}
