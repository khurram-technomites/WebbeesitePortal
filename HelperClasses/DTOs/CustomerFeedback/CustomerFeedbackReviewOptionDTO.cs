using HelperClasses.DTOs.Survey;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.CustomerFeedback
{
    public class CustomerFeedbackReviewOptionDTO
    {
        public long Id { get; set; }

        public long SurveyOptionId { get; set; }

        public long CustomerFeedbackReviewId { get; set; }

        /*Relationships*/
        public SurveyOptionDTO SurveyOption { get; set; }
        public CustomerFeedbackReviewDTO CustomerFeedbackReview { get; set; }
    }
}
