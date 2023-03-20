using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class SurveyQuestion : GeneralSchema
    {
        public SurveyQuestion()
        {
			SurveyOptions = new HashSet<SurveyOption>();
		}

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(255, ErrorMessage = "Name must be less than 255 characters")]
        public string Name { get; set; }
        [MaxLength(100, ErrorMessage = "Type must be less than 100 characters")]
        public string Type { get; set; }
        public string Question { get; set; }
        public int Position { get; set; }
        public bool IsRequired { get; set; }
        [MaxLength(100, ErrorMessage = "Status length must be less than 100 characters")]
        public string Status { get; set; }

        /*Foreign Keys*/
        [ForeignKey(nameof(Survey))]
        public long? SurveyId { get; set; }
        
        [ForeignKey(nameof(Restaurant))]
        public long? RestaurantId { get; set; }

        /*Relationships*/
        public Survey Survey { get; set; }
        public Restaurant Restaurant { get; set; }

		/*ICollections*/
		public ICollection<SurveyOption> SurveyOptions { get; set; }
	}
}
