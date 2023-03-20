using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class CurrencyNote
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		[MaxLength(50, ErrorMessage = "Name length must be less than 50 characters")]
		public string Name { get; set; }
		public decimal Value { get; set; }
	}
}
