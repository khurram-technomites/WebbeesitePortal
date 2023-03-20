using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;
using System;

namespace WebApp.ViewModels
{
    public class RestaurantTableViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string ActiveStatus { get; set; }
        public int Serving { get; set; }
        public DateTime CreationDate { get; set; }
        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }
        public RestaurantViewModel Restaurant { get; set; }
        public RestaurantBranchViewModel RestaurantBranch { get; set; }

        //ICollections
        //public List<RestaurantTableReservationViewModel> RestaurantTableReservations { get; set; }
    }
}
