using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartsDealer : GeneralSchema
    {
        public SparePartsDealer()
        {
            DealerInventorySpecifications = new HashSet<DealerInventorySpecification>();
            DealerImages = new HashSet<DealerImage>();
            DealerSchedules = new HashSet<DealerSchedule>();
            SparePartMenuManagement = new HashSet<SparePartMenuManagement>();
            SparePartBannerSettings = new HashSet<SparePartBannerSetting>();
            SparePartsDealerDocuments = new HashSet<SparePartsDealerDocument>();
            SparePartMenuManagement = new HashSet<SparePartMenuManagement>();
            SparePartServiceManagement = new HashSet<SparePartServiceManagement>();
            SparePartTeamManagement = new HashSet<SparePartTeamManagement>();
            SparePartExpertiseManagements = new HashSet<SparePartExpertiseManagement>();
            SparePartTestimonials = new HashSet<SparePartTestimonial>();
            SparePartBlogs = new HashSet<SparePartBlog>();
            SparePartSubscribers = new HashSet<SparePartSubscriber>();
            SparePartPartnersManagement = new HashSet<SparePartPartnersManagement>();
            SparePartCustomerAppointment = new HashSet<SparePartCustomerAppointment>();
            SparePartCareers = new HashSet<SparePartCareer>();
            SparePartCustomerFeedbacks = new HashSet<SparePartCustomerFeedback>();
            SparePartFAQ = new HashSet<SparePartFAQ>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Slug { get; set; }
        public string Logo { get; set; }
        public string Video { get; set; }
        [MaxLength(200, ErrorMessage = "NameAsPerTradeLicense length must be less than 200 characters")]
        public string NameAsPerTradeLicense { get; set; }
        [MaxLength(200, ErrorMessage = "NameArAsPerTradeLicense length must be less than 200 characters")]
        public string NameArAsPerTradeLicense { get; set; }
        [MaxLength(250, ErrorMessage = "Address length must be less than 250 characters")]
        public string Address { get; set; }
        public string CompleteAddress { get; set; }
        [MaxLength(4000, ErrorMessage = "Description length must be less than 4000 characters")]
        public string Description { get; set; }
        [MaxLength(4000, ErrorMessage = "DescriptionAr length must be less than 4000 characters")]
        public string DescriptionAr { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string ContactPersonName { get; set; }
        [MaxLength(20, ErrorMessage = "ContactPersonNumber length must be less than 20 characters")]
        public string ContactPersonNumber { get; set; }
        [MaxLength(20, ErrorMessage = "ContactPersonNumber01 length must be less than 20 characters")]
        public string ContactPersonNumber01 { get; set; }
        public string ContactPersonEmail { get; set; }
        [MaxLength(20, ErrorMessage = "Status length must be less than 10 characters")]
        public string Status { get; set; }
        public string ReferenceCode { get; set; }
        public string Bank { get; set; }
        public string BankAccountHolderName { get; set; }
        public string BankAccountNumber { get; set; }
        public string IBAN { get; set; }
        public string Remarks { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public string RejectionReason { get; set; }
        public bool IsServicesAllowed { get; set; }
        public bool IsBlogsAllowed { get; set; }
        public bool IsAppoinmnetsAllowed { get; set; }
        public bool IsTeamsAllowed { get; set; }
        public bool IsFeedbackAllowed { get; set; }
        public bool IsCareersAllowed { get; set; }
        public string SecondaryLogo { get; set; }
        public string ThemeColor { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Whatsapp { get; set; }
        public string Website { get; set; }

        public AppUser User { get; set; }
        public SparePartContentManagement SparePartContentManagement { get; set; }
        public SparePartAppointmentManagement SparePartAppointmentManagement { get; set; }
        public SparePartBusinessSetting SparePartBusinessSetting { get; set; }

        //Collections 

        public ICollection<DealerInventorySpecification> DealerInventorySpecifications { get; set; }
        public ICollection<DealerImage> DealerImages { get; set; }
        public ICollection<DealerSchedule> DealerSchedules { get; set; }
        public ICollection<SparePartsDealerDocument> SparePartsDealerDocuments { get; set; }
        public ICollection<SparePartBannerSetting> SparePartBannerSettings { get; set; }
        public ICollection<SparePartServiceManagement> SparePartServiceManagement { get; set; }
        public ICollection<SparePartMenuManagement> SparePartMenuManagement { get; set; }
        public ICollection<SparePartTeamManagement> SparePartTeamManagement { get; set; }
        public ICollection<SparePartExpertiseManagement> SparePartExpertiseManagements { get; set; }
        public ICollection<SparePartTestimonial> SparePartTestimonials { get; set; }
        public ICollection<SparePartBlog> SparePartBlogs { get; set; }
        public ICollection<SparePartSubscriber> SparePartSubscribers { get; set; }
        public ICollection<SparePartPartnersManagement> SparePartPartnersManagement { get; set; }
        public ICollection<SparePartCustomerAppointment> SparePartCustomerAppointment { get; set; }
        public ICollection<SparePartCareer> SparePartCareers { get; set; }
        public ICollection<SparePartCustomerFeedback> SparePartCustomerFeedbacks { get; set; }
        public ICollection<SparePartFAQ> SparePartFAQ { get; set; }

    }
}
