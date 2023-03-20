using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels.Survey
{
    public class SurveyQuestionViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Question { get; set; }
        public int Position { get; set; }
        public bool IsRequired { get; set; }
        public string Status { get; set; }

        public DateTime CreationDate { get; set; }
        public long? SurveyId { get; set; }
        public long RestaurantId { get; set; }
        public SurveyViewModel Survey { get; set; }
        public List<SurveyOptionViewModel> SurveyOptions { get; set; }
    }
}
