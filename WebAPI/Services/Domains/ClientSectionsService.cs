using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class ClientSectionsService : IClientSectionsService
    {
        private readonly IClientSectionsRepo _repo;
        public ClientSectionsService(IClientSectionsRepo repo)
        {
            _repo = repo;
        }

        public async Task<ClientSections> AddCityAsync(ClientSections Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<ClientSections> ArchiveCityAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<ClientSections>> GetAllCitiesAsync()
        {
            return await _repo.GetAllAsync(OrderExp: x => x.Id);
        }

        public async Task<IEnumerable<ClientSections>> GetCityByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<ClientSections> UpdateCityAsync(ClientSections Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
