using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Fatoorah
{
    public class PaymentInquiryResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object ValidationErrors { get; set; }
        public PaymentInquiryDataDTO Data { get; set; }
    }

	public class PaymentInquiryDataDTO
	{
		public int InvoiceId { get; set; }
		public string InvoiceStatus { get; set; }
		public string InvoiceReference { get; set; }
		public object CustomerReference { get; set; }
		public DateTime CreatedDate { get; set; }
		public string ExpiryDate { get; set; }
		public double InvoiceValue { get; set; }
		public object Comments { get; set; }
		public string CustomerName { get; set; }
		public string CustomerMobile { get; set; }
		public string CustomerEmail { get; set; }
		public object UserDefinedField { get; set; }
		public string InvoiceDisplayValue { get; set; }
		public List<PaymentInvoiceItemDTO> InvoiceItems { get; set; }
		public List<InvoiceTransactionDTO> InvoiceTransactions { get; set; }
		public List<object> Suppliers { get; set; }
	}

	public class InvoiceTransactionDTO
	{
		public DateTime TransactionDate { get; set; }
		public string PaymentGateway { get; set; }
		public string ReferenceId { get; set; }
		public string TrackId { get; set; }
		public string TransactionId { get; set; }
		public string PaymentId { get; set; }
		public string AuthorizationId { get; set; }
		public string TransactionStatus { get; set; }
		public string TransationValue { get; set; }
		public string CustomerServiceCharge { get; set; }
		public string DueValue { get; set; }
		public string PaidCurrency { get; set; }
		public string PaidCurrencyValue { get; set; }
		public string Currency { get; set; }
		public object Error { get; set; }
		public string CardNumber { get; set; }
	}
	public class PaymentInvoiceItemDTO
	{
		public string ItemName { get; set; }
		public int Quantity { get; set; }
		public double UnitPrice { get; set; }
		public object Weight { get; set; }
		public object Width { get; set; }
		public object Height { get; set; }
		public object Depth { get; set; }
	}
}
