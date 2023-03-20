using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
	public interface ICustomerFeedbackReviewService
	{
		Task<IEnumerable<CustomerFeedbackReview>> GetAllAsync();
		Task<IEnumerable<CustomerFeedbackReview>> GetAllByCustomerFeedbackIdAsync(long CustomerFeedbackId);
		Task<CustomerFeedbackReview> GetByIdAsync(long Id);
		Task<CustomerFeedbackReview> AddAsync(CustomerFeedbackReview Model);
		Task<CustomerFeedbackReview> UpdateAsync(CustomerFeedbackReview Model);
		Task<CustomerFeedbackReview> ArchiveAsync(long Id);
	}
}
