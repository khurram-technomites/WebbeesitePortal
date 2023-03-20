using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Survey
{
    public class SurveyDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public long? RestaurantId { get; set; }
        public long? RestaurantBranchId { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public RestaurantBranchDTO RestaurantBranch { get; set; }
        public RestaurantDTO Restaurant { get; set; }
        public List<SurveyQuestionDTO> SurveyQuestions { get; set; }
    }
}
