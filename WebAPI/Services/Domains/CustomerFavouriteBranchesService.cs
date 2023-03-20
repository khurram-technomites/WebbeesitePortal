using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Restaurant.Filter;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CustomerFavouriteBranchesService : ICustomerFavouriteBranchesService
    {
        private readonly ICustomerFavouriteBranchesRepo _repo;

        public CustomerFavouriteBranchesService(ICustomerFavouriteBranchesRepo repo)
        {
            _repo = repo;
        }

        public async Task<CustomerFavouriteBranches> AddFavouriteBranch(CustomerFavouriteBranches Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task DeleteFavouriteBranch(long CustomerId, long BranchId)
        {
            IEnumerable<CustomerFavouriteBranches> result = await GetByUserAndBranch(CustomerId, BranchId);

            await _repo.DeleteAsync(result.FirstOrDefault().Id);
        }

        public IEnumerable<RestaurantCardResponseDTO> GetAllByCustomer(RestaurantFilter Filter, long CustomerId)
        {
            return _repo.GetAllByCustomer(Filter, CustomerId);
        }

        public async Task<IEnumerable<CustomerFavouriteBranches>> GetByUserAndBranch(long CustomerId, long BranchId)
        {
            return await _repo.GetByIdAsync(x => x.BranchId == BranchId && x.CustomerId == CustomerId);
        }
    }
}