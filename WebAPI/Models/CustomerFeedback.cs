using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class CustomerFeedback : GeneralSchema
	{
		public CustomerFeedback()
		{
			CustomerFeedbackReviews = new HashSet<CustomerFeedbackReview>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		[MaxLength(255, ErrorMessage = "Name must be less than 255 characters")]
		public string Name { get; set; }
		public int Position { get; set; }
		public string AudioSound { get; set; }
		[MaxLength(20, ErrorMessage = "Status must be less than 20 characters")]
		public string Status { get; set; }
		public long? UserDetailId { get; set; }
		[MaxLength(255, ErrorMessage = "User Type must be less than 255 characters")]
		public string UserType { get; set; }
		[MaxLength(100, ErrorMessage = "ActiveStatus length must be less than 100 characters")]
		public string ActiveStatus { get; set; }
		/*Foreign Keys*/
		[ForeignKey(nameof(User))]
		public string UserId { get; set; }

		[ForeignKey(nameof(Customer))]
		public long? CustomerId { get; set; }
		[ForeignKey(nameof(Survey))]
		public long? SurveyId { get; set; }
		[ForeignKey("Restaurant")]
		public long? RestaurantId { get; set; }

		[ForeignKey(nameof(RestaurantBranch))]
		public long? RestaurantBranchId { get; set; }

		/*Relationships*/
		public Customer Customer { get; set; }
		public Survey Survey { get; set; }
		public RestaurantBranch RestaurantBranch { get; set; }
		public Restaurant Restaurant { get; set; }
		public AppUser User { get; set; }

		/*ICollections*/
		public ICollection<CustomerFeedbackReview> CustomerFeedbackReviews { get; set; }
	}
}
