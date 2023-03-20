using System;

namespace WebApp.ViewModels
{
    public class ClientContentMediaViewModel
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string DocumentType { get; set; }

        public string DocumentPath { get; set; }
        public DateTime CreatedOn { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
