using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
	public class NumberRange
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		public string Name { get; set; }
		public string Prefix { get; set; }
		public int Padding { get; set; }
		public int CurrentCount { get; set; }
	}
}
