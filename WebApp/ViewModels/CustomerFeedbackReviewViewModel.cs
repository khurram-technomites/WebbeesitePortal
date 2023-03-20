using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels.Survey;

namespace WebApp.ViewModels
{
    public class CustomerFeedbackReviewViewModel
    {
		public long Id { get; set; }
		public string Comment { get; set; }
		public int? RatingValue { get; set; }
		public DateTime ReviewDateTime { get; set; }

		/*Foreign Keys*/
		public long CustomerFeedbackId { get; set; }
		public long SurveyQuestionId { get; set; }
		public long? EmojiId { get; set; }
		public long? SurveyOptionId { get; set; }

		/*Relationships*/
		public CustomerFeedbackViewModel CustomerFeedback { get; set; }
		public SurveyQuestionViewModel SurveyQuestion { get; set; }
		public EmojiViewModel Emoji { get; set; }
		public SurveyOptionViewModel SurveyOption { get; set; }
	}
}
