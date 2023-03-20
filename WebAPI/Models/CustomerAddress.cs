using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class CustomerAddress
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Customer))]
        public long CustomerId { get; set; }
        [MaxLength(500, ErrorMessage = "Address length must be less than 500 characters")]
        public string Address { get; set; }
        public string CompleteAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [MaxLength(20, ErrorMessage = "Type length must be less than 20 characters")]
        public string Type { get; set; }
        public string Street { get; set; }
        public string Floor { get; set; }
        public string NoteToRider { get; set; }

        public Customer Customer { get; set; }
    }
}
