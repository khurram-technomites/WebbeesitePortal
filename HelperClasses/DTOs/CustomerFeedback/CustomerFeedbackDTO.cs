using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.CustomerFeedback
{
	public class CustomerFeedbackDTO
	{
		public long Id { get; set; }

		public string Name { get; set; }
		public int Position { get; set; }
		public string AudioSound { get; set; }
		public string Status { get; set; }
		public long? UserDetailId { get; set; }
		public string UserType { get; set; }
		public DateTime CreationDate { get; set; }
        public string ActiveStatus { get; set; }


        /*Foreign Keys*/
        public string UserId { get; set; }
		public long? CustomerId { get; set; }
		public long? SurveyId { get; set; }
		public long? RestaurantId { get; set; }
		public long? RestaurantBranchId { get; set; }

		/*Relationships*/
		public CustomerDTO Customer { get; set; }
		public SurveyDTO Survey { get; set; }
		public RestaurantBranchDTO RestaurantBranch { get; set; }
		public RestaurantDTO Restaurant { get; set; }
		public AppUserDTO User { get; set; }

		/*ICollections*/
		public IList<CustomerFeedbackReviewDTO> CustomerFeedbackReviews { get; set; }
	}
}
