using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;
using System;

namespace WebApp.ViewModels
{
    public class RestaurantManagerViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Contact2 { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }
        public RestaurantViewModel Restaurant { get; set; }
        public RestaurantBranchViewModel RestaurantBranch { get; set; }
        public string RestaurantBranchName { get; set; }
    }
}
