using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class CustomerFeedbackReview
    {
        public CustomerFeedbackReview()
        {
            CustomerFeedbackReviewOptions = new HashSet<CustomerFeedbackReviewOption>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Comment { get; set; }
        public int? RatingValue { get; set; }
        public DateTime ReviewDateTime { get; set; }

        /*Foreign Keys*/

        [ForeignKey(nameof(CustomerFeedback))]
        public long CustomerFeedbackId { get; set; }

        [ForeignKey(nameof(SurveyQuestion))]
        public long SurveyQuestionId { get; set; }

        [ForeignKey(nameof(Emoji))]
        public long? EmojiId { get; set; }

        [ForeignKey(nameof(SurveyOption))]
        public long? SurveyOptionId { get; set; }

        /*Relationships*/
        public CustomerFeedback CustomerFeedback { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
        public Emoji Emoji { get; set; }
        public SurveyOption SurveyOption { get; set; }

        /*ICollections*/
        public ICollection<CustomerFeedbackReviewOption> CustomerFeedbackReviewOptions { get; set; }
    }
}
