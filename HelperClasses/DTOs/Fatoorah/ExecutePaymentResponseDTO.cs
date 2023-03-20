using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Fatoorah
{
    public class ExecutePaymentResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ValidationErrorDTO> ValidationErrors { get; set; }
        public DataDTO Data { get; set; }
    }

	public class ValidationErrorDTO
	{
		public string Name { get; set; }
		public string Error { get; set; }
	}

	public class DataDTO
	{
		public int InvoiceId { get; set; }
		public bool IsDirectPayment { get; set; }
		public string PaymentURL { get; set; }
		public string CustomerReference { get; set; }
		public string UserDefinedField { get; set; }
		public string RecurringId { get; set; }
	}
}
