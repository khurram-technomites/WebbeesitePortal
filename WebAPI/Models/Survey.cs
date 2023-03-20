using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Survey : GeneralSchema
    {
        public Survey()
        {
			SurveyQuestions = new HashSet<SurveyQuestion>();
		}

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(255, ErrorMessage = "Name must be less than 255 characters")]
        public string Name { get; set; }
        public int Position { get; set; }
        [MaxLength(100, ErrorMessage = "Status length must be less than 100 characters")]
        public string Status { get; set; }

        /*Foreign Keys*/
        [ForeignKey("Restaurant")]
        public long? RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantBranch))]
        public long? RestaurantBranchId { get; set; }

        /*Relationships*/
        public RestaurantBranch RestaurantBranch { get; set; }
        public Restaurant Restaurant { get; set; }

		/*ICollections*/
		public ICollection<SurveyQuestion> SurveyQuestions { get; set; }
	}
}
