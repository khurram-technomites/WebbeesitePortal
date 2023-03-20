using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public partial class Emoji
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(50, ErrorMessage = "Text must be less than 50 characters")]
        public string Text { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
        [MaxLength(100, ErrorMessage = "Status length must be less than 100 characters")]
        public string Status { get; set; }
    }
}
