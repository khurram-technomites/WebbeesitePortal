using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Blog
{
    public class BlogCategoryDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Module { get; set; }
        public long GarageId { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
