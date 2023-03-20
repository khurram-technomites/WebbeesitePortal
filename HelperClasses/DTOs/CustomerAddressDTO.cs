using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class CustomerAddressDTO
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string Address { get; set; }
        public string CompleteAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Type { get; set; }
        public string Street { get; set; }
        public string Floor { get; set; }
        public string NoteToRider { get; set; }
        public double Distance { get; set; }
    }
}
