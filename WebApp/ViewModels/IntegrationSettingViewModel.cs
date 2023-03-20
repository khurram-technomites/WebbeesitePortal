using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class IntegrationSettingViewModel
    {
        public long Id { get; set; }
        public string EmailAddress { get; set; }
        public string ContactEmail { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string Host { get; set; }
        public bool EnableSSL { get; set; }
        public string SMSApiKey { get; set; }
        public string SMSSenderId { get; set; }
        public string SMSUrl { get; set; }
        public string GoogleMapKey { get; set; }
        public string FCMKey { get; set; }
        public string SquadFCMKey { get; set; }
        public string PartnerFCMKey { get; set; }
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
