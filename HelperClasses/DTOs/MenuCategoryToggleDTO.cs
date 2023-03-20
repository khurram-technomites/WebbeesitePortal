using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class MenuCategoryToggleDTO
    {
        public long MenuItemId { get; set; }
        public long MenuId { get; set; }
        public long CategoryId { get; set; }
        public Status StatusId { get; set; }
    }
}
