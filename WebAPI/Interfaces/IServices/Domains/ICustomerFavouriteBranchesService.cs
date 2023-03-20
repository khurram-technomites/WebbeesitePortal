using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Restaurant.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICustomerFavouriteBranchesService
    {
        IEnumerable<RestaurantCardResponseDTO> GetAllByCustomer(RestaurantFilter Filter, long CustomerId);
        Task<IEnumerable<CustomerFavouriteBranches>> GetByUserAndBranch(long CustomerId, long BranchId);
        Task<CustomerFavouriteBranches> AddFavouriteBranch(CustomerFavouriteBranches Model);
        Task DeleteFavouriteBranch(long CustomerId, long BranchId);
    }
}
