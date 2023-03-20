
using HelperClasses.DTOs;
using HelperClasses.DTOs.Menu;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Restaurant.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepo _repo;

        public RestaurantService(IRestaurantRepo repo)
        {
            _repo = repo;
        }

        public async Task<Restaurant> AddRestaurantAsync(Restaurant Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<Restaurant> ArchiveRestaurantAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<long> GetAllRestaurantsCountAsync()
        {
            return await _repo.GetCount();
        }
        public async Task<IEnumerable<Restaurant>> GetRestaurantByIdAsync(long? Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "User,RestaurantBranches,RestaurantBranches.City,RestaurantImages,RestaurantDocuments,RestaurantBranches.BranchSchedules");
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantForDropDwonAsync()
        {
            return await _repo.GetByIdAsync(x => x.SupplierCode == null);
        }
        public async Task<IEnumerable<Restaurant>> GetRestaurantForDropDwonAssignAsync()
        {
            return await _repo.GetByIdAsync(x => x.SupplierCode != null);
        }
        public async Task<IEnumerable<Restaurant>> GetRestaurantBySlugAsync(string Slug)
        {
            return await _repo.GetByIdAsync(x => x.Slug == Slug, ChildObjects: "RestaurantRatings");
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantByUserAsync(string UserId)
        {
            return await _repo.GetByIdAsync(x => x.UserId == UserId);
        }

        public async Task<Restaurant> UpdateRestaurantAsync(Restaurant Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }

        public IEnumerable<RestaurantCardResponseDTO> GetAllRestaurantsNearMe(RestaurantFilter Filter)
        {
            return _repo.GetAllNearMe(Filter);
        }

        public async Task<IEnumerable<PopularCategories>> GetPopularCategoriesByBranch(long BranchId)
        {
            return await _repo.GetAllPopularCategoriesByRestaurantBranch(BranchId);
        }

        public IEnumerable<LandingPageResponseDTO> GetRestaurantDetailsByOrigin(string Origin)
        {
            Origin = Origin.Replace("www.", "");
            return _repo.GetRestaurantDetailsByOrigin(Origin);
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantByOrigin(string Origin)
        {
            Origin = Origin.Replace("www.", "");
            return await _repo.GetRestaurantByOrigin(Origin);
        }

        public object GetNearestRestaurant(long restaurantId, decimal lat, decimal lng)
        {
            return _repo.GetNearestBranch(lat, lng, restaurantId);
        }

        public async Task<IEnumerable<LandingPageResponseDTO>> GetRestaurantBranchDetails(long BranchId, long? customerId)
        {
            return await _repo.GetRestaurantBranchDetails(BranchId, customerId);
        }

        public async Task<List<BranchMenuDTO>> GetBranchMenu(long BranchId)
        {
            return await _repo.GetBranchMenu(BranchId);
        }

        public async Task<IEnumerable<MenuPartnerDTO>> GetBranchMenuForPartner(long BranchId)
        {
            return await _repo.GetBranchMenuForPartner(BranchId);
        }

        public IEnumerable<RestaurantCardResponseDTO> GetTrending(RestaurantFilter Filter)
        {
            return _repo.Trending(Filter);
        }
    }
}
