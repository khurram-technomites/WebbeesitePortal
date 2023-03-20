using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class PaymentMethod  : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(20,ErrorMessage ="Method must be less than 20 charachter")]
        public string Method { get; set; }
        [MaxLength(20, ErrorMessage = "MethodAr must be less than 20 charachter")]
        public string MethodAr { get; set; }
    }
}
