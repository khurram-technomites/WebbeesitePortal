using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class GarageProjectImageViewModel
    {
        public long Id { get; set; }
        public long GarageProjectId { get; set; }
        public string ImagePath { get; set; }
        public GarageProjectViewModel GarageProject { get; set; }

    }
}
