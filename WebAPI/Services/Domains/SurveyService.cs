using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepo _repo;
        public SurveyService(ISurveyRepo repo)
        {
            _repo = repo;
        }
        public async Task<Survey> AddSurveyAsync(Survey Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<Survey> ArchiveSurveyAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<Survey>> GetAllAsync()
        {
            return await _repo.GetAllAsync(ChildObjects: "Restaurant , RestaurantBranch");
        }


        public async Task<IEnumerable<Survey>> GetAllByBranchIdAsync(long branchId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantBranchId == branchId && x.Status == Enum.GetName(typeof(Status), Status.Active), ChildObjects: "Restaurant , RestaurantBranch, SurveyQuestions, SurveyQuestions.SurveyOptions");
        }

        public async Task<IEnumerable<Survey>> GetAllByRestaurantIdAsync(long restaurantId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId, ChildObjects: "Restaurant , RestaurantBranch");
        }

        public async Task<IEnumerable<Survey>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Restaurant , RestaurantBranch, SurveyQuestions, SurveyQuestions.SurveyOptions");
        }

        public async Task<Survey> UpdateSurveyAsync(Survey Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
