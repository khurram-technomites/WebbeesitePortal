using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class EmojiViewModel
    {
		public long Id { get; set; }
		public string Text { get; set; }
		public string Image { get; set; }
		public int Position { get; set; }
	}
}
