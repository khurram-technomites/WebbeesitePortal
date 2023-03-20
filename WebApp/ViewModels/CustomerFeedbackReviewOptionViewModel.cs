using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models;
using WebApp.ViewModels.Survey;

namespace WebApp.ViewModels
{
    public class CustomerFeedbackReviewOptionViewModel
    {
        public long Id { get; set; }

        public long SurveyOptionId { get; set; }

        public long CustomerFeedbackReviewId { get; set; }

        /*Relationships*/
        public SurveyOptionViewModel SurveyOption { get; set; }
        public CustomerFeedbackReviewViewModel CustomerFeedbackReview { get; set; }
    }
}
