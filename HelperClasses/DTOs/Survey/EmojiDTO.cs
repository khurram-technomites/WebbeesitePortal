using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Survey
{
	public class EmojiDTO
	{
		public long Id { get; set; }
		public string Text { get; set; }
		public string Image { get; set; }
		public int Position { get; set; }
	}
}
