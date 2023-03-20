using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
   
    public class GarageAwardDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreationDate { get; set; }
        public long GarageId { get; set; }
        public GarageDTO Garage { get; set; }
    }
}
