using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
	public class SurveyQuestionService : ISurveyQuestionService
	{
		private readonly ISurveyQuestionRepo _repo;
		public SurveyQuestionService(ISurveyQuestionRepo repo)
		{
			_repo = repo;
		}
		public async Task<SurveyQuestion> AddSurveyQuestionAsync(SurveyQuestion Model)
		{
			return await _repo.InsertAsync(Model);
		}

		public async Task<SurveyQuestion> ArchiveSurveyQuestionAsync(long Id)
		{
			return await _repo.ArchiveAsync(Id);
		}


		public async Task<IEnumerable<SurveyQuestion>> GetByIdAsync(long Id)
		{
			return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Survey,SurveyOptions");
		}


		public async Task<IEnumerable<SurveyQuestion>> GetAllAsyncBySurveyId(long surveyId)
		{
			return await _repo.GetAllAsync(x => x.SurveyId == surveyId, ChildObjects: "Survey");
		}

		public async Task<IEnumerable<SurveyQuestion>> GetAllAsync()
		{
			return await _repo.GetAllAsync(ChildObjects: "Survey");
		}

		public async Task<IEnumerable<SurveyQuestion>> GetAllAsync(long RestaurantId)
		{
			return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "Survey");
		}

		public async Task<SurveyQuestion> UpdateSurveyQuestionAsync(SurveyQuestion Model)
		{
			return await _repo.UpdateAsync(Model);
		}
	}
}
