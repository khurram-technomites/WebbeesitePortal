using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
	public interface ICustomerFeedbackService
	{
		Task<IEnumerable<CustomerFeedback>> GetAllAsync();
		Task<IEnumerable<CustomerFeedback>> GetAllAsync(PagingParameters Pagination);
		Task<IEnumerable<CustomerFeedback>> GetAllByRestaurantIdAsync(long RestaurantId);
		Task<IEnumerable<CustomerFeedback>> GetAllByBranchIdAsync(long RestaurantBranchId);
		Task<CustomerFeedback> GetByIdAsync(long Id);
		Task<CustomerFeedback> AddAsync(CustomerFeedback Model);
		Task<CustomerFeedback> UpdateAsync(CustomerFeedback Model);
		Task<CustomerFeedback> ArchiveAsync(long Id);
	}
}
