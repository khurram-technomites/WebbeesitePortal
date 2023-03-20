using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SurveyOptionService : ISurveyOptionService
    {
        private readonly ISurveyOptionRepo _repo;
        public SurveyOptionService(ISurveyOptionRepo repo)
        {
            _repo = repo;
        }
        public async Task<SurveyOption> AddSurveyOptionAsync(SurveyOption Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SurveyOption> ArchiveSurveyOptionAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }


        public async Task<IEnumerable<SurveyOption>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }


        public async Task<IEnumerable<SurveyOption>> GetAllBySurveyIdAsync(long QuestionId)
        {
            return await _repo.GetByIdAsync(x => x.QuestionId == QuestionId);
        }

        //public async Task<IEnumerable<SurveyOption>> GetAllBySurveyQuestionIdAsync(long surveyOptionId)
        //{
        //    return await _repo.GetByIdAsync(x => x.SurveyId == surveyOptionId);
        //}

        public async Task<SurveyOption> UpdateSurveyOptionAsync(SurveyOption Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
