using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class RouteGroup
    {
        public int RouteGroupId { get; set; }
        public int GroupId { get; set; }
        public int RouteId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Route Route { get; set; }
    }
}
