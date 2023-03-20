using Fingers10.ExcelExport.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models;

namespace WebApp.ViewModels
{
    public class GarageSubscribersViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        [IncludeInReport(Order = 1)]
        [Display(Name = "Subscriber Email")]
        [DisplayName("Subscriber Email")]
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
