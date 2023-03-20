using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class IntegrationSetting  : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(50, ErrorMessage = "Email Address must be less than 50 characters" )]
        public string EmailAddress { get; set; }
        [MaxLength(50, ErrorMessage = "Contact Email must be less than 50 characters")]
        public string ContactEmail { get; set; }
        [MaxLength(225, ErrorMessage = "Password must be less than 225 characters")]
        public string Password { get; set; }
        [MaxLength(10, ErrorMessage = "Password must be less than 10 characters")]
        public int Port { get; set; }
        [MaxLength(50, ErrorMessage = "Host must be less than 50 characters")]
        public string Host { get; set; }
        [MaxLength(50, ErrorMessage = "SenderName must be less than 50 characters")]
        public string SenderName { get; set; }
        public bool EnableSSL { get; set; }
        [MaxLength(200, ErrorMessage = "SMSApiKey must be less than 200 characters")]
        public string SMSApiKey { get; set; }
        [MaxLength(100, ErrorMessage = "SMSSenderId must be less than 100 characters")]
        public string SMSSenderId { get; set; }
        [MaxLength(500, ErrorMessage = "SMSUrl must be less than 500 characters")]
        public string SMSUrl { get; set; }
        [MaxLength(500, ErrorMessage = "GoogleMapKey must be less than 500 characters")]
        public string GoogleMapKey { get; set; }
        [MaxLength(500, ErrorMessage = "FCMKey must be less than 500 characters")]
        public string FCMKey { get; set; }
        public string SquadFCMKey { get; set; }
        public string AutomobileFCMKey { get; set; }
        public string PartnerFCMKey { get; set; }
        public string DeliveryFCMKey { get; set; }
        public string SandboxEndpoint { get; set; }
        public string LiveEndpoint { get; set; }
        public bool GoLive { get; set; }
        public string FatoorahAPIKey { get; set; }
        public string SMSUsername { get; set; }
        public string SMSPassword { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public bool DLR { get; set; }

        public string SalesEmail { get; set; }
    }

}
