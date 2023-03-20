using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartBusinessSetting
    {
        public SparePartBusinessSetting()
        {

            SparePartBranchBusinessSettings = new HashSet<SparePartBranchBusinessSetting>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(500, ErrorMessage = "Title must be less than 500 characters")]
        public string Title { get; set; }
        public string Whatsapp { get; set; }
        [ForeignKey(nameof(SparePart))]
        public long SparePartId { get; set; }
        public string FirstMessage { get; set; }
        public string StreetAddress { get; set; }
        public string Contact01 { get; set; }
        public string Contact02 { get; set; }
        public string ContactPersonName { get; set; }
        public string CompleteAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public string Twitter { get; set; }
        public string Snapchat { get; set; }
        public string LinkedIn { get; set; }
        public string Behnace { get; set; }
        public string Pinterest { get; set; }
        public SparePartsDealer SparePart { get; set; }
        public ICollection<SparePartBranchBusinessSetting> SparePartBranchBusinessSettings { get; set; }
    }
}
