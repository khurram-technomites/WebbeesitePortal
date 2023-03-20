using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class BlogCategory : GeneralSchema
    {
        public BlogCategory()
        {
            GarageBlogs = new HashSet<GarageBlog>();
            SparePartBlogs = new HashSet<SparePartBlog>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Module { get; set; }

        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }  
        public IEnumerable<GarageBlog> GarageBlogs { get; set; }
        public IEnumerable<SparePartBlog> SparePartBlogs { get; set; }
    }
}
