using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class ClientIndustriesService : IClientIndustriesService
    {
        private readonly IClientIndustriesRepo _repo;
        public ClientIndustriesService(IClientIndustriesRepo repo)
        {
            _repo = repo;
        }

        public async Task<ClientIndustries> AddClientIndustriesAsync(ClientIndustries Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<ClientIndustries> ArchiveClientIndustriesAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<ClientIndustries>> GetAllClientIndustriesAsync()
        {
            return await _repo.GetAllAsync(OrderExp: x => x.Id);
        }



        public async Task<IEnumerable<ClientIndustries>> GetClientIndustriesByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<ClientIndustries> UpdateClientIndustriesAsync(ClientIndustries Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
