using HelperClasses.DTOs.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISurveyQuestionClient
    {
        Task<IEnumerable<SurveyQuestionDTO>> GetAllSurveyQuestionAsync(long RestaurantId);
        Task<SurveyQuestionDTO> GetSurveyQuestionByIdAsync(long SurveyQuestionId);
        Task<SurveyQuestionDTO> AddSurveyQuestionAsync(SurveyQuestionDTO Entity);
        Task<SurveyQuestionDTO> UpdateSurveyQuestionAsync(SurveyQuestionDTO Entity);
        Task DeleteSurveyQuestionAsync(long SurveyQuestionId);
        Task<SurveyQuestionDTO> ToggleActiveStatus(long Id);
        Task<IEnumerable<SurveyQuestionDTO>> GetAllSurveyQuestionBySurveyAsync(long SurveyID);
    }
}
