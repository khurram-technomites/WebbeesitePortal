using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using MimeKit;
using WebAPI.Interfaces.IServices.Domains;
using AutoMapper;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Emails;

namespace WebAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _adminEmailAddress;
        private readonly IIntegrationSettingService _integrationSettingService;
        private readonly IMapper _mapper;
        private readonly EmailSettings _emailSettings;
        private readonly ITemplateService _templateService;
        private readonly ILogger<EmailService> _logger;

        public EmailService(ITemplateService templateService, ILogger<EmailService> logger,
            IConfiguration _config, IIntegrationSettingService integrationSettingService, IMapper mapper)
        {
            _integrationSettingService = integrationSettingService;
            _mapper = mapper;
            _emailSettings = new EmailSettings();

            IEnumerable<IntegrationSettingDTO> integrationSettingDTOList = _mapper.Map<IEnumerable<IntegrationSettingDTO>>(_integrationSettingService.GetAllAsync().Result);
            IntegrationSettingDTO integrationSetting = integrationSettingDTOList.FirstOrDefault();

            if(integrationSetting != null)
            {
                _emailSettings.SenderName = integrationSetting.SenderName;
                _emailSettings.MailServer = integrationSetting.Host;
                _emailSettings.MailPort = integrationSetting.Port;
                _emailSettings.UserName = integrationSetting.EmailAddress;
                _emailSettings.Sender = integrationSetting.EmailAddress;
                _emailSettings.Password = integrationSetting.Password;
                _emailSettings.UseSSL = integrationSetting.EnableSSL;
                _emailSettings.AuthRequired = true;
            }          

            _templateService = templateService;
            _adminEmailAddress = _config.GetValue<string>("AdminEmail");

            _logger = logger;
        }


        public async Task<string> SendConfirmationEmail(ConfirmEmailDTO DataModel)
        {
            String ViewName = "ConfirmEmail";
            String Subject = "Verify your phone number";

            try
            {
                var Message = await _templateService.RenderTemplateAsync(ViewName, DataModel);

                await SendEmailAsync(DataModel.Email, Subject, Message);

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending confirmation email to {0}", DataModel.Email);

                return ex.Message;
            }
        }

        public async Task<string> SendResetPasswordEmail(ResetPasswordDTO DataModel)
        {
            String ViewName = "ResetPassword";
            String Subject = "Reset your password";

            try
            {
                var Message = await _templateService.RenderTemplateAsync(ViewName, DataModel);

                await SendEmailAsync(DataModel.Email, Subject, Message);

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending reset password email to {0}", DataModel.Email);

                return ex.Message;
            }
        }

        public async Task<string> SendTestEmail(string EmailAddress)
        {
            String ViewName = "TestEmail";
            String Subject = "Email to test health";

            try
            {
                var Message = await _templateService.RenderTemplateAsync(ViewName, "");

                await SendEmailAsync(EmailAddress, Subject, Message);

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending Test email to {0}", EmailAddress);

                return ex.Message;
            }
        }

        public async Task SendEmailAsync(string email, string subject, string message, string senderName = "", string senderAddress = "", string CC = "", string BCC = "")
        {
            try
            {
                var mimeMessage = new MimeMessage();

                if (!string.IsNullOrEmpty(senderName) && !string.IsNullOrEmpty(senderAddress))
                    mimeMessage.From.Add(new MailboxAddress(senderName, senderAddress));
                else
                    mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));

                mimeMessage.To.Add(MailboxAddress.Parse(email));

                if (!String.IsNullOrEmpty(CC))
                {
                    mimeMessage.Cc.Add(MailboxAddress.Parse(CC));
                }

                if (!String.IsNullOrEmpty(BCC))
                {
                    mimeMessage.Bcc.Add(MailboxAddress.Parse(BCC));
                }

                mimeMessage.Subject = subject;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, _emailSettings.UseSSL);

                    if (_emailSettings.AuthRequired)
                    {
                        // Note: since we don't have an OAuth2 token, disable
                        // the XOAUTH2 authentication mechanism.
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Authenticate(_emailSettings.UserName, _emailSettings.Password);
                    }

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<string> SendContactEmail(string senderAddress, string senderName, string email, string message)
        {
            String Subject = "Need help!";

            try
            {
                await SendEmailAsync(email, Subject, message, senderName, senderAddress);

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {0}", email);

                return ex.Message;
            }
        }
        public async Task<string> SendSubscriberEmail(string senderAddress, string email, string message)
        {
            String Subject = "ContactUs";

            try
            {
                await SendEmailAsync(email, Subject, message, senderAddress);

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {0}", email);

                return ex.Message;
            }
        }

        public async Task<string> SendOTPEmail(ConfirmEmailDTO DataModel)
        {
            String ViewName = "OtpVerify";
            String Subject = "Verify your email address";

            try
            {
                var Message = await _templateService.RenderTemplateAsync(ViewName, DataModel);

                await SendEmailAsync(DataModel.Email, Subject, Message);

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending confirmation email to {0}", DataModel.Email);

                return ex.Message;
            }
        }
        public async Task<string> SendWelcomeEmail(ConfirmEmailDTO DataModel)
        {
            String ViewName = "WelcomeEmailTemplate";
            String Subject = "Welcome to the webbeesite";

            try
            {
                var Message = await _templateService.RenderTemplateAsync(ViewName, DataModel);

                await SendEmailAsync(DataModel.Email, Subject, Message);

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending confirmation email to {0}", DataModel.Email);

                return ex.Message;
            }
        }
        public async Task<string> SendProfileApprovalEmail(ConfirmEmailDTO DataModel)
        {
            String ViewName = "ProfileApproval";
            String Subject = "Webbeesite - Vendor Account Approved";

            try
            {
                var Message = await _templateService.RenderTemplateAsync(ViewName, DataModel);

                await SendEmailAsync(DataModel.Email, Subject, Message);

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending confirmation email to {0}", DataModel.Email);

                return ex.Message;
            }
        }
        public async Task<string> SendOrderPlacementEmail(OrderPlacementEmailDTO DataModel)
        {
            String ViewName = "OrderPlacement";
            String Subject = "Your order has been confirmed";

            try
            {
                var Message = await _templateService.RenderTemplateAsync(ViewName, DataModel);

                await SendEmailAsync(DataModel.Email, Subject, Message);

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending confirmation email to {0}", DataModel.Email);

                return ex.Message;
            }
        }
        public async Task<string> SendDemoRequestEmail(GeneralEmailDTO DataModel, string Subject, string Email)
        {
            String ViewName = "RequestDemoEmailTemplate";

            try
            {
                var Message = await _templateService.RenderTemplateAsync(ViewName, DataModel);

                await SendEmailAsync(Email, Subject, Message, DataModel.Name, DataModel.Email);

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending confirmation email to {0}", Email);

                return ex.Message;
            }
        }

        public async Task<string> SendContactEmailByTemplate(GeneralEmailDTO DataModel, string Subject, string Email)
        {
            String ViewName = "ContactUs";

            try
            {
                var Message = await _templateService.RenderTemplateAsync(ViewName, DataModel);
                await SendEmailAsync(Email, Subject, Message, DataModel.Name, DataModel.Email);

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {0}", Email);

                return ex.Message;
            }
        }
        public async Task<string> SendGeneralEmail(GeneralEmailDTO DataModel, string Subject, string Email)
        {
            String ViewName = "GeneralEmailTemplate";

            try
            {
                var Message = await _templateService.RenderTemplateAsync(ViewName, DataModel);

                await SendEmailAsync(Email, Subject, Message);

                return "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending confirmation email to {0}", Email);

                return ex.Message;
            }
        }
    }
}
