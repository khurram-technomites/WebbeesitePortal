using Fingers10.ExcelExport.Attributes;
using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class GarageViewModel
    {
        public long Id { get; set; }
        public string Logo { get; set; }
        public string SecondaryLogo { get; set; }
        public string ThumbnailImage { get; set; }
        public string PhoneNumber { get; set; }
        public string Slug { get; set; }
        [IncludeInReport(Order = 2)]
        [Display(Name = "NameAsPerTradeLicense")]
        [DisplayName("NameAsPerTradeLicense")]
        public string NameAsPerTradeLicense { get; set; }
        public string NameArAsPerTradeLicense { get; set; }
        [IncludeInReport(Order = 3)]
        [Display(Name = "Address")]
        [DisplayName("Address")]
        public string Address { get; set; }
        public string CompleteAddress { get; set; }
        [IncludeInReport(Order = 4)]
        [Display(Name = "Description")]
        [DisplayName("Description")]
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        [IncludeInReport(Order = 5)]
        [Display(Name = "Latitude")]
        [DisplayName("Latitude")]
        public decimal Latitude { get; set; }
        [IncludeInReport(Order = 6)]
        [Display(Name = "Longitude")]
        [DisplayName("Longitude")]
        public decimal Longitude { get; set; }
        [IncludeInReport(Order = 7)]
        [Display(Name = "ContactPersonName")]
        [DisplayName("ContactPersonName")]
        public string ContactPersonName { get; set; }
        [IncludeInReport(Order = 8)]
        [Display(Name = "ContactPersonNumber")]
        [DisplayName("ContactPersonNumber")]
        public string ContactPersonNumber { get; set; }
        public string ContactPersonNumber01 { get; set; }
        [IncludeInReport(Order = 9)]
        [Display(Name = "ContactPersonEmail")]
        [DisplayName("ContactPersonEmail")]
        public string ContactPersonEmail { get; set; }
        [IncludeInReport(Order = 10)]
        [Display(Name = "Status")]
        [DisplayName("Status")]
        public string Status { get; set; }
        [IncludeInReport(Order = 11)]
        [Display(Name = "ReferenceCode")]
        [DisplayName("ReferenceCode")]
        public string ReferenceCode { get; set; }
        [IncludeInReport(Order = 12)]
        [Display(Name = "Bank")]
        [DisplayName("Bank")]
        public string Bank { get; set; }
        [IncludeInReport(Order = 13)]
        [Display(Name = "BankAccountHolderName")]
        [DisplayName("BankAccountHolderName")]
        public string BankAccountHolderName { get; set; }
        [IncludeInReport(Order = 14)]
        [Display(Name = "BankAccountNumber")]
        [DisplayName("BankAccountNumber")]
        public string BankAccountNumber { get; set; }
        [IncludeInReport(Order = 15)]
        [Display(Name = "IBAN")]
        [DisplayName("IBAN")]
        public string IBAN { get; set; }
        public string UserId { get; set; }
        [IncludeInReport(Order = 16)]
        [Display(Name = "ServingKilometers")]
        [DisplayName("ServingKilometers")]
        public double ServingKilometers { get; set; }
        [IncludeInReport(Order = 17)]
        [Display(Name = "AvgRating")]
        [DisplayName("AvgRating")]
        public double AvgRating { get; set; }
        [IncludeInReport(Order = 18)]
        [Display(Name = "RatingCount")]
        [DisplayName("RatingCount")]
        public float RatingCount { get; set; }
        [IncludeInReport(Order = 19)]
        [Display(Name = "CreationDate")]
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get; set; }
        public string Video { get; set; }
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
        public long? ClientTypeId { get; set; }
        public long? ClientSectionId { get; set; }
        public long? ClientIndustryId { get; set; }
        public long? VendorId { get; set; }
        public long? CountryId { get; set; }
        public long? CityId { get; set; }
        public bool IsDomainRequired { get; set; }
        public string DomainProvider { get; set; }
        public string DomainUserId { get; set; }
        public string DomainPassword { get; set; }
        public UserViewModel User { get; set; }
        public List<ClientContentMediaViewModel> ClientContentMediaViewModels { get; set; } = new List<ClientContentMediaViewModel>();
        public List<ClientDomainSuggestionsViewModel> ClientDomainSuggestionsViewModels { get; set; } = new List<ClientDomainSuggestionsViewModel>();
        public List<ClientEmailsViewModel> ClientEmailsViewModels { get; set; } = new List<ClientEmailsViewModel>();
        public CountryViewModel Country { get; set; }
        public CityViewModel City { get; set; }
        public ClientTypesViewModel ClientType { get; set; }
        public ClientSectionsViewModel ClientSection { get; set; }
        public ClientIndustriesViewModel ClientIndustry { get; set; }
        public GarageBusinessSettingViewModel GarageBusinessSetting { get; set; }
        public GarageContentManagementViewModel GarageContentManagement { get; set; } = new GarageContentManagementViewModel();
        public List<GarageRepairSpecificationDTO> GarageRepairSpecifications { get; set; } = new List<GarageRepairSpecificationDTO>();
        public List<GarageImageDTO> GarageImages { get; set; } = new List<GarageImageDTO>();
        public List<GarageScheduleDTO> GarageSchedules { get; set; } = new List<GarageScheduleDTO>();
        public List<GarageRatingDTO> GarageRatings { get; set; } = new List<GarageRatingDTO>();
        public List<GarageDocumentViewModel> GarageDocuments { get; set; } = new List<GarageDocumentViewModel>();
        public ClientModulePurchasesViewModel ClientModulePurchases { get; set; } = new ClientModulePurchasesViewModel();
        public List<ModulePurchaseDetailsViewModel> ModulePurchaseDetails { get; set; } = new List<ModulePurchaseDetailsViewModel>();
    }

}
