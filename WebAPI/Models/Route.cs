using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class Route
    {
        public Route()
        {
            RouteGroups = new HashSet<RouteGroup>();
        }

        public int RouteId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string RoutePath { get; set; }

        public virtual ICollection<RouteGroup> RouteGroups { get; set; }
    }
}
