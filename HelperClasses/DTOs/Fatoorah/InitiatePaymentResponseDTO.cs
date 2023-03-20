using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Fatoorah
{
    public class InitiatePaymentResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object ValidationErrors { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public List<PaymentMethodDTO> PaymentMethods { get; set; }
    }
}
