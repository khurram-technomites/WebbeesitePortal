using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class GarageProjectViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageViewModel Garage { get; set; }
        public List<GarageProjectImageViewModel> GarageProjectImages { get; set; } = new();
    }
}
