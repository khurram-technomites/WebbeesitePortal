using HelperClasses.DTOs.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.CustomerFeedback
{
    public class CustomerFeedbackReviewDTO
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
        public CustomerFeedbackDTO CustomerFeedback { get; set; }
        public SurveyQuestionDTO SurveyQuestion { get; set; }
        public EmojiDTO Emoji { get; set; }
        public SurveyOptionDTO SurveyOption { get; set; }

        /*ICollections*/
        public IList<CustomerFeedbackReviewOptionDTO> customerFeedbackReviewOptions { get; set; }
    }
}
