using HelperClasses.DTOs;
using System;

namespace WebApp.ViewModels
{
    public class GarageBlogViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int EstimatedReadingMinutes { get; set; }
        public string Status { get; set; }
        public string Slug { get; set; }
        public long BlogCategoryId { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
