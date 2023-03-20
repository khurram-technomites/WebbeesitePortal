using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface ISparePartMenuRepo : IRepository<SparePartMenu>
    {
        Task<IEnumerable<SparePartMenu>> GetMenuBySparePartDealerId(long SparePartDealerId);

    }
}
