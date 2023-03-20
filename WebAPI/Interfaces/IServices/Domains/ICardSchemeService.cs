
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICardSchemeService
    {
        Task<IEnumerable<CardScheme>> GetAllAsync();
        Task<IEnumerable<CardScheme>> GetByIdAsync(long Id);
        Task<CardScheme> AddCardScheme(CardScheme Model);
        Task<CardScheme> UpdateCardScheme(CardScheme Model);
        Task<CardScheme> ArchiveCardScheme(long Id);
    }
}
