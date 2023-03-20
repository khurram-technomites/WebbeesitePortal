using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISurveyOptionService
    {
        Task<IEnumerable<SurveyOption>> GetAllBySurveyIdAsync(long SurveyId);
        //Task<IEnumerable<SurveyOption>> GetAllBySurveyQuestionIdAsync(long SurveyQuestionId);
        Task<IEnumerable<SurveyOption>> GetByIdAsync(long Id);
        Task<SurveyOption> AddSurveyOptionAsync(SurveyOption Model);
        Task<SurveyOption> UpdateSurveyOptionAsync(SurveyOption Model);
        Task<SurveyOption> ArchiveSurveyOptionAsync(long Id);
    }
}
