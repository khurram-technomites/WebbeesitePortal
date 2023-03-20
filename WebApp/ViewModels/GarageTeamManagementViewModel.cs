using HelperClasses.DTOs;
using System;

namespace WebApp.ViewModels
{
    public class GarageTeamManagementViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string ImagePath { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
