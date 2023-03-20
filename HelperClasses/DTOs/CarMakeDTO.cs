using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class CarMakeDTO 
    {
        public CarMakeDTO()
        {
            CarModels = new List<CarModelDTO>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public string NameAR { get; set; }
        public string Status { get; set; }
        public string Logo { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal ModelsCount { get { return CarModels.Count; } set { } }
        public List<CarModelDTO> CarModels { get; set; }
    }
}
