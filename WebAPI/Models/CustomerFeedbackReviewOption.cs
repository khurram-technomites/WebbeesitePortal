using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class CustomerFeedbackReviewOption
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		
		[ForeignKey(nameof(SurveyOption))]
		public long SurveyOptionId { get; set; }

		[ForeignKey(nameof(CustomerFeedbackReview))]
		public long CustomerFeedbackReviewId { get; set; }

		/*Relationships*/
		public SurveyOption SurveyOption { get; set; }
		public CustomerFeedbackReview CustomerFeedbackReview { get; set; }
	}
}
