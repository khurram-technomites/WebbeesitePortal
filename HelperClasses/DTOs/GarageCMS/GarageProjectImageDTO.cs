using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageProjectImageDTO
    {
        public long Id { get; set; }
        public long GarageProjectId { get; set; }
        public string ImagePath { get; set; }
    }
}
