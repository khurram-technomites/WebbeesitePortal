using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageTeamManagementDTO
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string ImagePath { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageDTO Garage { get; set; }
    }
}
