using HelperClasses.DTOs.Aggregators;
using HelperClasses.DTOs.CardScheme;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ICardSchemeClient
    {
        Task<IEnumerable<CardSchemeDTO>> GetAllCardSchemeAsync();
        Task<IEnumerable<CardSchemeDTO>> GetCardSchemeByIdAsync(long Id);
        Task<CardSchemeDTO> AddCardSchemeAsync(CardSchemeDTO Entity);
        Task<CardSchemeDTO> UpdateCardSchemeAsync(CardSchemeDTO Entity);
        Task DeleteCardSchemeAsync(long CardSchemeId);
    }
}
