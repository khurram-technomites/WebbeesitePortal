using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Garage : GeneralSchema
    {
        public Garage()
        {
            GarageRepairSpecifications = new HashSet<GarageRepairSpecification>();
            GarageImages = new HashSet<GarageImage>();
            GarageSchedules = new HashSet<GarageSchedule>();
            GarageRatings = new HashSet<GarageRating>();
            GarageDocuments = new HashSet<GarageDocument>();
            GarageBannerSettings = new HashSet<GarageBannerSetting>();
            GarageMenuManagement = new HashSet<GarageMenuManagement>();
            GarageServiceManagement = new HashSet<GarageServiceManagement>();
            GarageTeamManagement = new HashSet<GarageTeamManagement>();
            GarageTestimonials = new HashSet<GarageTestimonials>();
            GarageBlogs = new HashSet<GarageBlog>();
            GarageSubscribers = new HashSet<GarageSubscribers>();
            GaragePartnersManagement = new HashSet<GaragePartnersManagement>();
            GarageCustomerAppointment = new HashSet<GarageCustomerAppointment>();
            GarageCareers = new HashSet<GarageCareers>();
            GarageCustomerFeedbacks = new HashSet<GarageCustomerFeedback>();
            GarageExpertiseManagements = new HashSet<GarageExpertiseManagement>();
            GarageAwards = new HashSet<GarageAward>();
            GarageFAQs = new HashSet<GarageFAQ>();
            GarageKeyBenefits = new HashSet<GarageKeyBenefit>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Slug { get; set; }
        public string Logo { get; set; }
        public string SecondaryLogo { get; set; }
        public string ThumbnailImage { get; set; }
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
        public bool IsExpertisAllowed { get; set; }
        public bool IsPartnerAllowed { get; set; }
        public bool IsProjectAllowed { get; set; }
        public bool IsTestimonialAllowed { get; set; }
        public bool IsServicesAllowed { get; set; }
        public bool IsBlogsAllowed { get; set; }
        public bool IsAppoinmnetsAllowed { get; set; }
        public bool IsTeamsAllowed { get; set; }
        public bool IsFeedbackAllowed { get; set; }
        public bool IsCareersAllowed { get; set; }
        public bool IsMenusAllowed { get; set; }

        public bool IsCustomerAppoinmentAllowed { get; set; }
        public bool IsAwardAllowed { get; set; }
        public string ThemeColor { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Whatsapp { get; set; }
        public string Website { get; set; }
        [ForeignKey(nameof(ClientType))]
        public long? ClientTypeId { get; set; }
        [ForeignKey(nameof(ClientSection))]
        public long? ClientSectionId { get; set; }
        [ForeignKey(nameof(ClientIndustry))]
        public long? ClientIndustryId { get; set; }
        [ForeignKey(nameof(Vendor))]
        public long? VendorId { get; set; }
        [ForeignKey(nameof(Country))]
        public long? CountryId { get; set; }
        [ForeignKey(nameof(City))]
        public long? CityId { get; set; }

        public bool IsDomainRequired { get; set; }
        public string DomainProvider { get; set; }
        public string DomainUserId { get; set; }
        public string DomainPassword { get; set; }

        public AppUser User { get; set; }
        public ClientTypes ClientType { get; set; }
        public ClientSections ClientSection { get; set; }
        public ClientIndustries ClientIndustry { get; set; }
        public Vendor Vendor { get; set; }
        public Country Country { get; set; }
        public City City { get; set; }
        public GarageBusinessSetting GarageBusinessSetting { get; set; }
        public GarageContentManagement GarageContentManagement { get; set; }
        public GarageAppointmentManagement GarageAppointmentManagement { get; set; }

       //Collections

        public ICollection<GarageExpertiseManagement> GarageExpertiseManagements { get; set; }
        public ICollection<GarageRepairSpecification> GarageRepairSpecifications { get; set; }
        public ICollection<GarageImage> GarageImages { get; set; }
        public ICollection<GarageSchedule> GarageSchedules { get; set; }
        public ICollection<GarageRating> GarageRatings { get; set; }
        public ICollection<GarageDocument> GarageDocuments { get; set; }
        public ICollection<GarageBannerSetting> GarageBannerSettings { get; set; }
        public ICollection<GarageMenuManagement> GarageMenuManagement { get; set; }
        public ICollection<GarageServiceManagement> GarageServiceManagement { get; set; }
        public ICollection<GarageTeamManagement> GarageTeamManagement { get; set; }
        public ICollection<GarageTestimonials> GarageTestimonials { get; set; }
        public ICollection<GarageBlog> GarageBlogs { get; set; }
        public ICollection<GarageSubscribers> GarageSubscribers { get; set; }
        public ICollection<GaragePartnersManagement> GaragePartnersManagement { get; set; }
        public ICollection<GarageCustomerAppointment> GarageCustomerAppointment { get; set; }
        public ICollection<GarageCareers> GarageCareers { get; set; }
        public ICollection<GarageAward> GarageAwards { get; set; }
        public ICollection<GarageCustomerFeedback> GarageCustomerFeedbacks { get; set; }
        public ICollection<GarageFAQ> GarageFAQs { get; set; }
        public ICollection<GarageKeyBenefit> GarageKeyBenefits { get; set; }
        
    }
}
