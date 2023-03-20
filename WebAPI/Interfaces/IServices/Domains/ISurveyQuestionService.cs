using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISurveyQuestionService
    {
        Task<IEnumerable<SurveyQuestion>> GetAllAsync();
        Task<IEnumerable<SurveyQuestion>> GetAllAsync(long RestaurantId);
        Task<IEnumerable<SurveyQuestion>> GetByIdAsync(long Id);
        Task<SurveyQuestion> AddSurveyQuestionAsync(SurveyQuestion Model);
        Task<SurveyQuestion> UpdateSurveyQuestionAsync(SurveyQuestion Model);
        Task<SurveyQuestion> ArchiveSurveyQuestionAsync(long Id);
        Task<IEnumerable<SurveyQuestion>> GetAllAsyncBySurveyId(long surveyId);
    }
}
