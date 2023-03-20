using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Fatoorah.WebHook
{
    public class WebHookDepositeDTO
    {
        public string DepositReference { get; set; }
        public decimal DepositedAmount { get; set; }
        public int NumberOfTransactions { get; set; }
    }
}
