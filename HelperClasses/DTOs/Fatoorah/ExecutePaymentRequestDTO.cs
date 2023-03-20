using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Fatoorah
{
    public class ExecutePaymentRequestDTO
    {
		public int PaymentMethodId { get; set; }
		public string CustomerName { get; set; }
		public string DisplayCurrencyIso { get; set; }
		public string MobileCountryCode { get; set; }
		public string CustomerMobile { get; set; }
		public string CustomerEmail { get; set; }
		public decimal InvoiceValue { get; set; }
		public string CallBackUrl { get; set; }
		public string ErrorUrl { get; set; }
		public string Language { get; set; }
		public string UserDefinedField { get; set; }

		public CustomerAddressDTO CustomerAddress { get; set; }
		public List<InvoiceItemDTO> InvoiceItems { get; set; }
        public List<InvoiceSupplierDTO> Suppliers { get; set; }
	}

	public class CustomerAddressDTO
	{
		public string Street { get; set; }
	}

	public class InvoiceItemDTO
	{
		public string ItemName { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
	}

	public class InvoiceSupplierDTO
    {
        public string SupplierCode { get; set; }
        public decimal? ProposedShare { get; set; }
        public decimal InvoiceShare { get; set; }
    }
}
