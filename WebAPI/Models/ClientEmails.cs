using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebAPI.Models
{
    public class ClientEmails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Garage))]
        public long ClientId { get; set; }

        [MaxLength(150, ErrorMessage = "Email length must be less than 150 characters")]
        public string Email { get; set; }


        public Garage Garage { get; set; }
    }
}
