using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Restaurant.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface ICustomerFavouriteBranchesRepo : IRepository<CustomerFavouriteBranches>
    {
        IEnumerable<RestaurantCardResponseDTO> GetAllByCustomer(RestaurantFilter Filter, long CustomerId);
    }
}
