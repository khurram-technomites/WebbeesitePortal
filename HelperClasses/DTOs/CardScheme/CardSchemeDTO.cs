using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.CardScheme
{
    public class CardSchemeDTO
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
