using HelperClasses.DTOs.Restaurant;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;
using AutoMapper;
using WebAPI.Mapper.RestaurantMapper;
using HelperClasses.DTOs.Menu;

namespace WebAPI.Repositories.Domains
{
	public class CategoryRepo : Repository<Category>, ICategoryRepo
	{
		private readonly IRestaurantRepo _repo;
		private readonly IMapper _mapper;
		public CategoryRepo(FougitoContext context, ILoggerManager loggerManager, IRestaurantRepo repo, IMapper mapper) : base(context, loggerManager)
		{
			_repo = repo;
			_mapper = mapper;
		}

		//public async Task<List<Category>> GetCategoryByMenu(long menuId)
		//{
		//    var result = GetAllAsync

		//    // Group at DB level was not doable
		//    List<MenuItemByMenuDTO> filter = (from MT in result
		//                                      group MT by new
		//                                      {
		//                                          MT.CategoryId,
		//                                          MT.Category.Name,
		//                                          MT.Category.Status,
		//                                          MT.Category.Description,
		//                                          MT.Category.Image,
		//                                      } into g
		//                                      select new MenuItemByMenuDTO
		//                                      {
		//                                          CategoryId = g.Key.CategoryId,
		//                                          CategoryName = g.Key.Name,
		//                                          CategoryStatus = g.Key.Status,
		//                                          CategoryDescription = g.Key.Description,
		//                                          CategoryImage = g.Key.Image,
		//                                          MenuItems = _mapper.Map<List<MenuItemByCategoryDTO>>(g.ToList())
		//                                      }).ToList();

		//    return filter;
		//}

		public async Task<IEnumerable<Category>> GetCategoriesByMenuID(long MenuId, long restaurantId)
		{
			var result2 = _context.MenuItems.Where(x => x.MenuId == MenuId).Select(x => x.CategoryId).Distinct().ToList();
			var result = await _context.Categories.Where(x => !result2.Contains(x.Id) && x.RestaurantId == restaurantId && x.ArchivedDate == null).ToListAsync();

			return result;
		}

		public async Task<IEnumerable<Category>> GetGeneralCategories(long restaurantId)
		{
			var result2 = _context.Categories.Where(x => x.RestaurantId == restaurantId).Select(x => x.Name).Distinct().ToList();
			var result = await _context.Categories.Where(x => !result2.Contains(x.Name) && x.RestaurantId == null && x.ArchivedDate == null).ToListAsync();

			return result;
		}

		public async Task<List<CategoryDTO>> GetByRestaurantBranchId(long restaurantBranchId)
		{
			var menuItems = _mapper.Map<List<MenuItemDTO>>(await _repo.GetBranchMenuItems(restaurantBranchId)).ToList();

			var result = (from MT in menuItems
						  group MT by new
						  {
							  MT.CategoryId,
							  MT.Category.Name,
							  MT.Category.NameAR,
							  MT.Category.Description,
							  MT.Category.DescriptionAR,
							  MT.Category.Image,
							  MT.Category.Slug,
							  MT.Category.ParentCategoryId,
							  MT.Category.Position,
							  MT.Category.IsParentCategoryDeactivate,
							  MT.Category.IsDefault,
							  MT.Category.Status,
							  MT.Category.RestaurantId,
							  MT.Category.CreationDate
						  } into g
						  select new CategoryDTO
						  {
							  Id = g.Key.CategoryId,
							  Name = g.Key.Name,
							  NameAR = g.Key.NameAR,
							  Description = g.Key.Description,
							  DescriptionAR = g.Key.DescriptionAR,
							  Image = g.Key.Image,
							  Slug = g.Key.Slug,
							  ParentCategoryId = g.Key.ParentCategoryId,
							  Position = g.Key.Position,
							  IsParentCategoryDeactivate = g.Key.IsParentCategoryDeactivate,
							  IsDefault = g.Key.IsDefault,
							  Status = g.Key.Status,
							  RestaurantId = g.Key.RestaurantId,
							  CreationDate = g.Key.CreationDate,
							  MenuItems = _mapper.Map<List<MenuItemDTO>>(g.OrderBy(x => x.Position).ToList()),
							  Items = null,
							  CouponCategories = null,
						  }).ToList();

			return result;
		}
	}
}
