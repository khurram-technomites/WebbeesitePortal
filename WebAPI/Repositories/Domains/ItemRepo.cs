using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class ItemRepo : Repository<Item>, IItemRepo
    {
        public ItemRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }

        public async Task<IEnumerable<Item>> GetCategoriesByMenuID(long categoryId , long restaurantId , long menuId)
        {
            var result2 = _context.MenuItems.Where(x => x.MenuId == menuId && x.CategoryId == categoryId).Select(x => x.ItemId).Distinct().ToList();
            var result = await _context.Items.Where(x => !result2.Contains(x.Id) && x.CategoryId == categoryId && x.RestaurantId == restaurantId).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Item>> GetGeneralItems(long restaurantId)
        {
            var result2 = _context.Items.Where(x => x.RestaurantId == restaurantId).Select(x => x.Name).Distinct().ToList();
            var result = await _context.Items.Include(y => y.ItemOptions).ThenInclude(z => z.ItemOptionValues).Where(x => !result2.Contains(x.Name) && x.RestaurantId == null && x.ArchivedDate == null).ToListAsync();

            return result;
        }
    }
}
