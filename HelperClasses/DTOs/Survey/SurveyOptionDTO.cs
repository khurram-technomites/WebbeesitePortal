using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Survey
{
    public class SurveyOptionDTO
    {
        public long Id { get; set; }
        public string Option { get; set; }
        public int Position { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsRequired { get; set; }
        public long? QuestionId { get; set; }
        public SurveyQuestionDTO SurveyQuestion { get; set; }
    }
}
