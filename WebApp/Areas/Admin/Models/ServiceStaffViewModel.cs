using Fingers10.ExcelExport.Attributes;
using System;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Models
{
    public class ServiceStaffViewModel
    {
        public string UserName { get; set; }
        public long Id { get; set; }
        [IncludeInReport(Order = 1)]
        public string FirstName { get; set; }
        [IncludeInReport(Order = 2)]
        public string LastName { get; set; }
        [IncludeInReport(Order = 3)]
        public string Email { get; set; }
        [IncludeInReport(Order = 4)]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string Logo { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public UserViewModel User { get; set; }
    }

  
}
