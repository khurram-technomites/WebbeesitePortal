using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public partial class RestaurantCashDenominationDetail
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		public int NoteCount { get; set; }
		public decimal Amount { get; set; }

		/*Foreign Keys*/
		[ForeignKey(nameof(CurrencyNote))]
		public long CurrencyNoteId { get; set; }
		[ForeignKey(nameof(RestaurantCashDenomination))]
		public long RestaurantCashDenominationId { get; set; }

		/*Relationships*/
		public CurrencyNote CurrencyNote { get; set; }
		public RestaurantCashDenomination RestaurantCashDenomination { get; set; }
	}
}
