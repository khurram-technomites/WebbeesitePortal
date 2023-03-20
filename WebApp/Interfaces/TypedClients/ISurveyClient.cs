using HelperClasses.DTOs.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISurveyClient
    {
        Task<IEnumerable<SurveyDTO>> GetAllSurveyByRestaurantAsync(long RestaurantId);
        Task<IEnumerable<SurveyDTO>> GetAllSurveyByBranchAsync(long BranchId);
        Task<SurveyDTO> GetSurveyByIdAsync(long SurveyId);
        Task<SurveyDTO> AddSurveyAsync(SurveyDTO Entity);
        Task<SurveyDTO> UpdateSurveyAsync(SurveyDTO Entity);
        Task<SurveyDTO> ToggleActiveStatus(long SurveyId);
        Task DeleteSurveyAsync(long SurveyId);
    }
}
