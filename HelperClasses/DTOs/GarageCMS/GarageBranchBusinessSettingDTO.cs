using HelperClasses.DTOs.SparePartCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageBranchBusinessSettingDTO
    {
        public long Id { get; set; }
        public long GarageBusinessSettingId { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string CompleteAddress { get; set; }
        public DateTime? CreationDate { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string ContactPersonName { get; set; }
        public string Email { get; set; }

        public SparePartBusinessSettingDTO SparePartBusinessSetting { get; set; }
    }
}
