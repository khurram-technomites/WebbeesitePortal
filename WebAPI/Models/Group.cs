using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class Group
    {
        public Group()
        {
            RouteGroups = new HashSet<RouteGroup>();
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<RouteGroup> RouteGroups { get; set; }
    }
}
