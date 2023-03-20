using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageMenuManagementDTO
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public long GarageMenuId { get; set; }
        public int Position { get; set; }
        public string Status { get; set; }
        public GarageDTO Garage { get; set; }
        public GarageMenuDTO GarageMenu { get; set; }
    }
}
