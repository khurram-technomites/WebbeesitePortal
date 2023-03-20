using HelperClasses.DTOs;
using HelperClasses.DTOs.Fatoorah;
using HelperClasses.DTOs.Fatoorah.WebHook;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices
{
    public interface IFatoorahService
    {
        Task<string> InitiatePayment(OrderDTO order, string restaurantPath, string supplierCode);
        Task<string> InitiateGaragePayment(GarageCustomerInvoiceDTO order, string garagePath);
        Task<PaymentInquiryResponseDTO> GetPaymentResponse(string PaymentId);
        Task<string> InitiatePayment(SupplierOrder order, string restaurantPath, string supplierCode);
        Task<string> InitiatePayment(ClientModulePurchasesDTO order, string clientPath);
        bool ValidateSignature<T>(WebHookResponseDTO<T> webHookResponse, string headerSignature);
    }
}
