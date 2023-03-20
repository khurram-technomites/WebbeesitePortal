using HelperClasses.DTOs;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models;

namespace WebApp.ViewModels
{
    public class GarageBannerSettingViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string ImagePath { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public string BannerType { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
