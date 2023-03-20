using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class ClientTypesService : IClientTypesService
    {
        private readonly IClientTypesRepo _repo;
        public ClientTypesService(IClientTypesRepo repo)
        {
            _repo = repo;
        }

        public async Task<ClientTypes> AddCityAsync(ClientTypes Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<ClientTypes> ArchiveCityAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<ClientTypes>> GetAllCitiesAsync()
        {
            return await _repo.GetAllAsync(OrderExp: x => x.Id);
        }

        public async Task<IEnumerable<ClientTypes>> GetCityByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }


        public async Task<ClientTypes> UpdateCityAsync(ClientTypes Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
