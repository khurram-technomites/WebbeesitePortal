using DocumentFormat.OpenXml.Drawing.Charts;
using Fingers10.ExcelExport.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class ClientTypesViewModel
    {
        public long Id { get; set; }
        [IncludeInReport(Order = 1)]
        [Display(Name = "Client Type Name")]
        [DisplayName("Client Type Name")]
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
