using AutoMapper;
using HelperClasses.DTOs.CustomerFeedback;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Models;
using System.Threading.Tasks;

namespace WebAPI.Mapper.CustomerFeedbackReviewMapper
{
	public class CustomerFeedbackReviewMapper : Profile
	{
		public CustomerFeedbackReviewMapper()
		{
			CreateMap<CustomerFeedbackReview, CustomerFeedbackReviewDTO>();
			CreateMap<CustomerFeedbackReviewDTO, CustomerFeedbackReview>()
                .ForAllMembers(x => x.Condition((source, destination, member) => member != null))
				;
		}
	}
}
