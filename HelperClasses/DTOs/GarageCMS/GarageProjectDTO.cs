using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageProjectDTO
    {
        public GarageProjectDTO()
        {
            GarageProjectImages = new List<GarageProjectImageDTO>();
        }
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageDTO Garage { get; set; }
        public List<GarageProjectImageDTO> GarageProjectImages { get; set; } = new();

    }
}
