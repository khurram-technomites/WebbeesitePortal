using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISurveyService 
    {
        Task<IEnumerable<Survey>> GetAllAsync();
        Task<IEnumerable<Survey>> GetByIdAsync(long Id);
        Task<Survey> AddSurveyAsync(Survey Model);
        Task<Survey> UpdateSurveyAsync(Survey Model);
        Task<Survey> ArchiveSurveyAsync(long Id);
        Task<IEnumerable<Survey>> GetAllByBranchIdAsync(long branchId);
        Task<IEnumerable<Survey>> GetAllByRestaurantIdAsync(long restaurantId);
    }
}
