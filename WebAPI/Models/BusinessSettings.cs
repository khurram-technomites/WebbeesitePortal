using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class BusinessSettings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(500 , ErrorMessage = "Title must be less than 500 characters")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "TitleAr must be less than 500 characters")]
        public string TitleAr { get; set; }
        [MaxLength(20, ErrorMessage = "WhatsApp must be less than 20 characters")]
        public string WhatsApp { get; set; }
        [MaxLength(225, ErrorMessage = "FirstMessage must be less than 225 characters")]
        public string FirstMessage { get; set; }
        [MaxLength(225, ErrorMessage = "FirstMessageAr must be less than 225 characters")]
        public string FirstMessageAr { get; set; }
        [MaxLength(1000, ErrorMessage = "MapIframe must be less than 1000 characters")]
        public string MapIframe { get; set; }
        [MaxLength(500, ErrorMessage = "StreetAddress must be less than 500 characters")]
        public string StreetAddress { get; set; }
        [MaxLength(500, ErrorMessage = "StreetAddressAr must be less than 500 characters")]
        public string StreetAddressAr { get; set; }
        [MaxLength(6, ErrorMessage = "PhoneCode must be less than 6 characters")]
        public string PhoneCode { get; set; }
        [MaxLength(6, ErrorMessage = "PhoneCode2 must be less than 6 characters")]
        public string PhoneCode2 { get; set; }
        [MaxLength(20, ErrorMessage = "Contact must be less than 20 characters")]
        public string Contact { get; set; }
        [MaxLength(20, ErrorMessage = "Contact2 must be less than 20 characters")]
        public string Contact2 { get; set; }
        [MaxLength(20, ErrorMessage = "Fax must be less than 20 characters")]
        public string Fax { get; set; }
        [MaxLength(50, ErrorMessage = "Email must be less than 50 characters")]
        public string Email { get; set; }
        [MaxLength(50, ErrorMessage = "Email2 must be less than 50 characters")]
        public string Email2 { get; set; }
        [MaxLength(50, ErrorMessage = "WorkingDays must be less than 50 characters")]
        public string WorkingDays{ get; set; }
        [MaxLength(50, ErrorMessage = "WorkingTime must be less than 50 characters")]
        public string WorkingTime { get; set; }
        [MaxLength(2000, ErrorMessage = "AndroidAppUrl must be less than 50 characters")]
        public string AndroidAppUrl { get; set; }
        [MaxLength(2000, ErrorMessage = "IosAppUrl must be less than 50 characters")]
        public string IosAppUrl { get; set; }
        [MaxLength(2000, ErrorMessage = "Facebook must be less than 50 characters")]
        public string Facebook { get; set; }
        [MaxLength(2000, ErrorMessage = "Instagram must be less than 50 characters")]
        public string Instagram { get; set; }
        [MaxLength(2000, ErrorMessage = "Youtube must be less than 50 characters")]
        public string Youtube { get; set; }
        [MaxLength(2000, ErrorMessage = "Twitter must be less than 50 characters")]
        public string Twitter { get; set; }
        [MaxLength(2000, ErrorMessage = "Snapchat must be less than 50 characters")]
        public string Snapchat { get; set; }
        public string PrivacyPolicy { get; set; }
        public string DeliveryPolicy { get; set; }
        public string ReturnPolicy { get; set; }
        public string TermsAndConditions { get; set; }
    }
}
