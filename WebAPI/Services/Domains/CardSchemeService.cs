using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CardSchemeService: ICardSchemeService
    {
        private readonly ICardSchemeRepo _repo;
        public CardSchemeService(ICardSchemeRepo repo)
        {
            _repo=repo;
        }

        public async Task<IEnumerable<CardScheme>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<CardScheme>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x=>x.Id==Id);
        }
        public async Task<CardScheme> AddCardScheme(CardScheme Model )
        {
            return await _repo.InsertAsync(Model);
        }
        public async Task<CardScheme> UpdateCardScheme(CardScheme Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<CardScheme> ArchiveCardScheme(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
