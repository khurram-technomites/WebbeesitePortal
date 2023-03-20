using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Survey
{
    public class SurveyQuestionDTO
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
        public SurveyDTO Survey { get; set; }
        public List<SurveyOptionDTO> SurveyOptions { get; set; }
    }
}
