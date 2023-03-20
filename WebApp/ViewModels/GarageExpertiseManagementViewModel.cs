using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs;
using System.Collections.Generic;
using System;

namespace WebApp.ViewModels
{
    public class GarageExpertiseManagementViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageViewModel Garage { get; set; }
        public List<GarageExpertiseViewModel> GarageExpertise { get; set; }
    }
}
