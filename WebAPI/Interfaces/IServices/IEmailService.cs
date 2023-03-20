using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Emails;
using HelperClasses.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Interfaces.IServices
{
    public interface IEmailService
    {
        Task<string> SendTestEmail(string EmailAddress);
        Task<string> SendConfirmationEmail(ConfirmEmailDTO DataModel);
        Task<string> SendOTPEmail(ConfirmEmailDTO DataModel);
        Task<string> SendWelcomeEmail(ConfirmEmailDTO DataModel);
        Task<string> SendProfileApprovalEmail(ConfirmEmailDTO DataModel);
        Task<string> SendOrderPlacementEmail(OrderPlacementEmailDTO DataModel);
        Task<string> SendResetPasswordEmail(ResetPasswordDTO DataModel);
        Task<string> SendGeneralEmail(GeneralEmailDTO DataModel, string Subject, string Email);
        Task<string> SendContactEmail(string senderAddress, string Subject, string Email, string message);
        Task<string> SendContactEmailByTemplate(GeneralEmailDTO DataModel, string Subject, string Email);
        Task<string> SendDemoRequestEmail(GeneralEmailDTO DataModel, string Subject, string Email);
        Task<string> SendSubscriberEmail(string senderAddress, string email, string message);
    }
}
