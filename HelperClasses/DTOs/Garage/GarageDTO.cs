using HelperClasses.Classes;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.GarageCMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class GarageDTO
    {
        public GarageDTO()
        {
            GarageImages = new List<GarageImageDTO>();
            GarageRepairSpecifications = new List<GarageRepairSpecificationDTO>();
            GarageSchedules = new List<GarageScheduleDTO>();
            GarageRatings = new List<GarageRatingDTO>();
            GarageDocuments = new List<GarageDocumentDTO>();
        }
        public long Id { get; set; }
        public string Logo { get; set; }
        public string SecondaryLogo { get; set; }
        public string ThumbnailImage { get; set; }
        public string Video { get; set; }
        public string Slug { get; set; }
        public string NameAsPerTradeLicense { get; set; }
        public string NameArAsPerTradeLicense { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CompleteAddress { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonNumber { get; set; }
        public string ContactPersonNumber01 { get; set; }
        public string ContactPersonEmail { get; set; }
        public string Status { get; set; }
        public string ReferenceCode { get; set; }
        public string Bank { get; set; }
        public string BankAccountHolderName { get; set; }
        public string BankAccountNumber { get; set; }
        public string IBAN { get; set; }
        public string UserId { get; set; }
        public double ServingKilometers { get; set; }
        public double AvgRating { get; set; }
        public float RatingCount { get; set; }        
        public DateTime CreationDate { get; set; }
        public string Distance { get; set; }
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
        public string DateDifference
        {
            get
            {
                DateTime current = DateTime.UtcNow;
                double difference = (current - CreationDate).TotalDays;

                if (difference < 30)
                    return $"Since {Math.Floor(difference)} days";

                if (difference > 30 && difference < 365)
                    return $"Since {Math.Floor(difference / 30)} months";

                else
                    return $"Since {Math.Floor(difference / 365)} years";

            }
            set { }
        }
        public bool IsActive
        {
            get
            {
                if (Status == Enum.GetName(typeof(Status), HelperClasses.Classes.Status.Active))
                    return true;

                return false;
            }
            set { }
        }
        public CountryDTO Country { get; set; }
        public CityDTO City { get; set; }
        public List<GarageImageDTO> GarageImages { get; set; }
        public List<GarageRepairSpecificationDTO> GarageRepairSpecifications { get; set; }
        public List<GarageScheduleDTO> GarageSchedules { get; set; }
        public List<GarageRatingDTO> GarageRatings { get; set; }
        public AppUserDTO User { get; set; }
        public ClientTypesDTO ClientType { get; set; }
        public GarageBusinessSettingDTO GarageBusinessSetting { get; set; }
        public ClientSectionsDTO ClientSection { get; set; }
        public ClientIndustriesDTO ClientIndustry { get; set; }
        public GarageContentManagementDTO GarageContentManagement { get; set; }
        public List<GarageDocumentDTO> GarageDocuments { get; set; }
        public List<GarageBannerSettingDTO> GarageBannerSettings { get; set; }
    }
}
