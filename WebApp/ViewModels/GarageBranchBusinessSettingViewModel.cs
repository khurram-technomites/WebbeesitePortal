using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class GarageBranchBusinessSettingViewModel
    {
        public long Id { get; set; }
        public long GarageBusinessSettingId { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string CompleteAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string ContactPersonName { get; set; }
        public string Email { get; set; }
        public DateTime? CreationDate { get; set; }

        public GarageBusinessSettingViewModel GarageBusinessSetting { get; set; }
    }
}
