using Fingers10.ExcelExport.Attributes;
using HelperClasses.Classes;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SparePartsDealerViewModel
    {
        public long Id { get; set; }
        public string Logo { get; set; }
        [IncludeInReport(Order = 2)]
        [Display(Name = "NameAsPerTradeLicense")]
        [DisplayName("NameAsPerTradeLicense")]
        public string NameAsPerTradeLicense { get; set; }
        public string NameArAsPerTradeLicense { get; set; }
        [IncludeInReport(Order = 3)]
        [Display(Name = "Address")]
        [DisplayName("Address")]
        public string Address { get; set; }
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
        public string Bank { get; set; }
        public string BankAccountHolderName { get; set; }
        public string BankAccountNumber { get; set; }
        public string IBAN { get; set; }
        public string Video { get; set; }
        public string CompleteAddress { get; set; }
        public string RejectionReason { get; set; }
        public bool IsServicesAllowed { get; set; }
        public bool IsBlogsAllowed { get; set; }
        public bool IsAppoinmnetsAllowed { get; set; }
        public bool IsTeamsAllowed { get; set; }
        public bool IsFeedbackAllowed { get; set; }
        public bool IsCareersAllowed { get; set; }
        public string ThemeColor { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Whatsapp { get; set; }
        public string Website { get; set; }
        public string SecondaryLogo { get; set; }
  
        public string UserId { get; set; }
        [IncludeInReport(Order = 12)]
        [Display(Name = "CreationDate")]
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get; set; }
        public UserViewModel User { get; set; }
        public ICollection<SparePartsDealerSpecificationsDTO> DealerInventorySpecifications { get; set; }
        public IEnumerable<CarMakeViewModel> CarMake { get; set; }
        public ICollection<SparePartDealerImagesDTO> DealerImages { get; set; }
        public ICollection<SparePartsDealerScheduleDTO> DealerSchedules { get; set; }
        public List<SparePartsDealerDocumentDTO> SparePartsDealerDocuments { get; set; }
    }
}
