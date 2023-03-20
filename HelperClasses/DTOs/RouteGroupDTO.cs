using System;
using System.Collections.Generic;

#nullable disable

namespace HelperClasses.DTOs
{
    public partial class RouteGroupDTO
    {
        public int RouteGroupId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int RouteId { get; set; }
        public string RoutePath { get; set; }
    }
}
