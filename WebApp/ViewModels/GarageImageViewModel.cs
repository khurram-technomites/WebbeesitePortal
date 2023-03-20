using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class GarageImageViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string Image { get; set; }
    }
}
