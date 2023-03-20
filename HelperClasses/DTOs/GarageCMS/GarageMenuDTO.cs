using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageMenuDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Route { get; set; }
        public long? ModuleID { get; set; }
        public ModuleDTO Modules { get; set; }
    }
}
