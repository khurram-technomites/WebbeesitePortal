using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels.Survey;

namespace WebApp.ViewModels
{
    public class CustomerFeedbackViewModel
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
		public CustomerViewModel Customer { get; set; }
		public SurveyViewModel Survey { get; set; }
		public RestaurantBranchViewModel RestaurantBranch { get; set; }
		public RestaurantViewModel Restaurant { get; set; }
		public AppUserViewModel User { get; set; }

		/*ICollections*/
		public IList<CustomerFeedbackReviewViewModel> CustomerFeedbackReviews { get; set; }
	}
}
