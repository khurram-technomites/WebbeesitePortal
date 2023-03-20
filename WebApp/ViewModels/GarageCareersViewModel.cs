using Fingers10.ExcelExport.Attributes;
using HelperClasses.DTOs;
using System;

namespace WebApp.ViewModels
{
    public class GarageCareersViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        [IncludeInReport(Order = 2)]
        public string FulName { get; set; }
        [IncludeInReport(Order = 3)]
        public string Gender { get; set; }
        [IncludeInReport(Order = 4)]
        public DateTime DOB { get; set; }
        [IncludeInReport(Order = 5)]
        public string Education { get; set; }
        [IncludeInReport(Order = 6)]
        public string Position { get; set; }
        [IncludeInReport(Order = 7)]
        public string Experience { get; set; }
        [IncludeInReport(Order = 8)]
        public string CVPath { get; set; }
        [IncludeInReport(Order = 1)]
        public DateTime CreationDate { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
