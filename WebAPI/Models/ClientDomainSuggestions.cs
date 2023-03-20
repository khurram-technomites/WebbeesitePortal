using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class ClientDomainSuggestions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Garage))]
        public long ClientId { get; set; }

        [MaxLength(100, ErrorMessage = "Domain length must be less than 100 characters")]
        public string Domain { get;set; }

        public int Position { get; set; }

        public Garage Garage { get; set; }


    }
}
