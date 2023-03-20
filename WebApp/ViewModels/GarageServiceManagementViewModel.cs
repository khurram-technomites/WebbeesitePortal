using HelperClasses.DTOs;
using System;

namespace WebApp.ViewModels
{
    public class GarageServiceManagementViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string BannerImagePath { get; set; }
        public string Thumbnail { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Slug { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
