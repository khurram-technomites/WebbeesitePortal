using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperClasses.DTOs.CurrencyNote;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantCashDenominationDetailDTO
    {
        public long Id { get; set; }
        public int NoteCount { get; set; }
        public decimal Amount { get; set; }
        public long CurrencyNoteId { get; set; }
        public long CurrencyNoteName { get; set; }
        public long RestaurantCashDenominationId { get; set; }
        //public CurrencyNoteDTO CurrencyNote { get; set; }
        public RestaurantCashDenominationDTO RestaurantCashDenomination { get; set; }
    }
}
