using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageCareerDTO
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string FulName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Education { get; set; }
        public string Position { get; set; }
        public string Experience { get; set; }
        public string CVPath { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageDTO Garage { get; set; }
    }
}
