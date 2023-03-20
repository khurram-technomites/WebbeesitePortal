using HelperClasses.DTOs.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISurveyOptionClient
    {
        Task<IEnumerable<SurveyOptionDTO>> GetAllBySurveyIdAsync(long SurveyId);
        Task<SurveyOptionDTO> GetSurveyOptionByIdAsync(long SurveyOptionId);
        Task<SurveyOptionDTO> AddSurveyOptionAsync(SurveyOptionDTO Entity);
        Task<SurveyOptionDTO> UpdateSurveyOptionAsync(SurveyOptionDTO Entity);
        Task DeleteSurveyOptionAsync(long SurveyOptionId);
    }
}
