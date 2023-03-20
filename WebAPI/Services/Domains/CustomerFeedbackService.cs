using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
	public class CustomerFeedbackService : ICustomerFeedbackService
	{
		private readonly ICustomerFeedbackRepo _repo;
		public CustomerFeedbackService(ICustomerFeedbackRepo repo)
		{
			_repo = repo;
		}

		public async Task<IEnumerable<CustomerFeedback>> GetAllAsync()
		{
			return await _repo.GetAllAsync(ChildObjects: "Customer, Survey, User");
		}

		public async Task<IEnumerable<CustomerFeedback>> GetAllAsync(PagingParameters Pagination)
		{
			return await _repo.GetAllAsync(Pagination: Pagination, OrderExp: x => x.Id, ChildObjects: "Customer, Survey, User");
		}

		public async Task<IEnumerable<CustomerFeedback>> GetAllByBranchIdAsync(long RestaurantBranchId)
		{
			return await _repo.GetAllAsync(x => x.RestaurantBranchId == RestaurantBranchId, ChildObjects: "Customer, Survey, RestaurantBranch, Restaurant, User");
		}

		public async Task<IEnumerable<CustomerFeedback>> GetAllByRestaurantIdAsync(long RestaurantId)
		{
			return await _repo.GetAllAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "Customer, Survey, RestaurantBranch, Restaurant, User");
		}

		public async Task<CustomerFeedback> GetByIdAsync(long Id)
		{
			IEnumerable<CustomerFeedback> list = await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Customer, Survey, RestaurantBranch, Restaurant, User, CustomerFeedbackReviews, CustomerFeedbackReviews.SurveyQuestion, CustomerFeedbackReviews.Emoji, CustomerFeedbackReviews.SurveyOption");
			return list.FirstOrDefault();
		}

		public async Task<CustomerFeedback> AddAsync(CustomerFeedback Entity)
		{
			return await _repo.InsertAsync(Entity);
		}

		public async Task<CustomerFeedback> UpdateAsync(CustomerFeedback Entity)
		{
			return await _repo.UpdateAsync(Entity);
		}

		public async Task<CustomerFeedback> ArchiveAsync(long Id)
		{
			return await _repo.ArchiveAsync(Id);
		}

	}

}
