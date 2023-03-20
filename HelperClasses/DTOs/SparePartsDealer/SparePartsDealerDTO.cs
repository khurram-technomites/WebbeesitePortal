using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartsDealer
{
    public class SparePartsDealerDTO
    {
        public long Id { get; set; }
        public string NameAsPerTradeLicense { get; set; }
        public string NameArAsPerTradeLicense { get; set; }
        public string Logo { get; set; }
        public string Video { get; set; }
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
        public string UserId { get; set; }
        public string Bank { get; set; }
        public string BankAccountHolderName { get; set; }
        public string BankAccountNumber { get; set; }
        public string IBAN { get; set; }
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
        public DateTime CreationDate { get; set; }

        public ICollection<SparePartsDealerSpecificationsDTO> DealerInventorySpecifications { get; set; }
        public ICollection<SparePartDealerImagesDTO> DealerImages { get; set; }
        public ICollection<SparePartsDealerScheduleDTO> DealerSchedules { get; set; }
        public AppUserDTO User { get; set; }
        public List<SparePartsDealerDocumentDTO> SparePartsDealerDocuments { get; set; }
    }
}
