using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;

namespace WebApp.ViewModels
{
    public class SparePartExpertiseViewModel
    {
        public long Id { get; set; }
        public long SparePartExpertiseManagementId { get; set; }
        public long ExpertiseId { get; set; }
        public SparePartExpertiseManagementViewModel SparePartExpertiseManagement { get; set; }
        public ExpertiseViewModel Expertise { get; set; }
    }
}
