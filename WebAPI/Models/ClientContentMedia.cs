using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebAPI.Models
{
    public class ClientContentMedia
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey(nameof(Garage))]
        public long ClientId { get; set; }

        [MaxLength(100, ErrorMessage = "Domain length must be less than 100 characters")]
        public string DocumentType { get; set; }

        public string DocumentPath { get; set; }
        public DateTime CreatedOn { get; set; }
        public Garage Garage { get; set; }
    }
}
