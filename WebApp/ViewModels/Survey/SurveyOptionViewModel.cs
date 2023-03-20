using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels.Survey
{
    public class SurveyOptionViewModel
    {
        public long Id { get; set; }
        public string Option { get; set; }
        public int Position { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsRequired { get; set; }
        public long? QuestionId { get; set; }
        public SurveyQuestionViewModel SurveyQuestion { get; set; }
    }
}
