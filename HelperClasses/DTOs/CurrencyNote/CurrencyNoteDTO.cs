using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.CurrencyNote
{
    public class CurrencyNoteDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Quantity { get; set; } = 1;
    }
}
