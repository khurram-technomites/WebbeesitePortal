using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface IGarageMenuRepo: IRepository<GarageMenu>
    {
        Task<IEnumerable<GarageMenu>> GetMenuByGarageId(long GarageId);
    }
}
