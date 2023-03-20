using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageRatingService : IGarageRatingService
    {
        private readonly IGarageRatingRepo _repo;

        public GarageRatingService(IGarageRatingRepo repo)
        {
            _repo = repo;
        }
        public async Task<GarageRating> AddRatingAsync(GarageRating Model)
        {
            return await _repo.InsertAsync(Model);
        }
        public async Task<IEnumerable<GarageRating>> GetAllRatingForGarageAsync()
        {
            return await _repo.GetAllAsync(OrderExp: x => x.Id , ChildObjects: "Garage , User");
        }
        public async Task<IEnumerable<GarageRating>> GetGarageRatingByIdAsync(long GarageId)
        {
            return await _repo.GetByIdAsync(x => x.Id == GarageId, ChildObjects: "Garage , User");
        }

        public async Task<GarageRating> UpdateGarageRatingAsync(GarageRating Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
