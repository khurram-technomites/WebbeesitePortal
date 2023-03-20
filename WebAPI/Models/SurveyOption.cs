using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class SurveyOption : GeneralSchema
    {
        public SurveyOption()
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Option { get; set; }
        public int Position { get; set; }

        [ForeignKey(nameof(SurveyQuestion))]
        public long QuestionId { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }

        ///*Foreign Keys*/
        //[ForeignKey(nameof(Survey))]
        //public long? SurveyId { get; set; }

        ///*Relationships*/
        //public Survey Survey { get; set; }

        /*ICollections*/
    }
}
