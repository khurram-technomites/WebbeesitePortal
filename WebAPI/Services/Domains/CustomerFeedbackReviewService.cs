using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
	public class CustomerFeedbackReviewService : ICustomerFeedbackReviewService
	{
		private readonly ICustomerFeedbackReviewRepo _repo;
		public CustomerFeedbackReviewService(ICustomerFeedbackReviewRepo repo)
		{
			_repo = repo;
		}

		public async Task<IEnumerable<CustomerFeedbackReview>> GetAllAsync()
		{
			return await _repo.GetAllAsync(ChildObjects: "CustomerFeedback, SurveyQuestion, Emoji, SurveyOption");
		}

		public async Task<IEnumerable<CustomerFeedbackReview>> GetAllByCustomerFeedbackIdAsync(long CustomerFeedbackId)
		{
			return await _repo.GetAllAsync(x => x.CustomerFeedbackId == CustomerFeedbackId, ChildObjects: "CustomerFeedback, SurveyQuestion, Emoji, SurveyOption");
		}

		public async Task<CustomerFeedbackReview> GetByIdAsync(long Id)
		{
			IEnumerable<CustomerFeedbackReview> list = await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "CustomerFeedback, SurveyQuestion, Emoji, SurveyOption");
			return list.FirstOrDefault();
		}

		public async Task<CustomerFeedbackReview> AddAsync(CustomerFeedbackReview Entity)
		{
			return await _repo.InsertAsync(Entity);
		}

		public async Task<CustomerFeedbackReview> UpdateAsync(CustomerFeedbackReview Entity)
		{
			return await _repo.UpdateAsync(Entity);
		}

		public async Task<CustomerFeedbackReview> ArchiveAsync(long Id)
		{
			return await _repo.ArchiveAsync(Id);
		}

	}
}
