using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels.Survey
{
    public class SurveyViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public long? RestaurantId { get; set; }
        public long? RestaurantBranchId { get; set; }
        public string Status { get; set; }

        public DateTime CreationDate { get; set; }
        public RestaurantBranchViewModel RestaurantBranch { get; set; }
        public RestaurantViewModel Restaurant { get; set; }
        public List<SurveyQuestionViewModel> SurveyQuestions { get; set; }
    }
}
