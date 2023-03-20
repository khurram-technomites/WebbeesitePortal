using HelperClasses.DTOs.GarageCMS;

namespace WebApp.ViewModels
{
    public class GarageExpertiseViewModel
    {
        public long Id { get; set; }
        public long GarageExpertiseManagementId { get; set; }
        public long ExpertiseId { get; set; }
        public GarageExpertiseManagementViewModel GarageExpertiseManagement { get; set; }
        public ExpertiseViewModel Expertise { get; set; }
    }
}
