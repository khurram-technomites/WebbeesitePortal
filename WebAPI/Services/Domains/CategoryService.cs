using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _repo;
        private readonly IMenuItemService _menuService;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepo repo, IMenuItemService menuService, IMapper mapper)
        {
            _repo = repo;
            _menuService = menuService;
            _mapper = mapper;

        }
        public async Task<Category> AddCategoryAsync(Category Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<Category> ArchiveCategoryAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(long restaurantId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId, ChildObjects:"");
        }

        public async Task<List<CategoryDTO>> GetAllAsyncByBranchId(long restaurantBranchId)
        {
            return await _repo.GetByRestaurantBranchId(restaurantBranchId);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllByMenu(long MenuId, long restaurantId)
        {
            var menuItems = await _menuService.GetAllAsync(MenuId);
            var category = await _repo.GetAllAsync(x => x.RestaurantId == restaurantId);

            var result = from categories in category
                         join items in menuItems
                         on categories.Id equals items.CategoryId into cc
                         from items in cc.DefaultIfEmpty()
                         where categories.Id != items.CategoryId
                         select new
                         {
                             Id = categories.Id,
                             Name = categories.Name
                         };


            return _mapper.Map<IEnumerable<CategoryDTO>>(result);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllByMenuID(long MenuId, long restaurantId)
        {
            return _mapper.Map<IEnumerable<CategoryDTO>>(await _repo.GetCategoriesByMenuID(MenuId, restaurantId));
        }

        public async Task<IEnumerable<Category>> GetByParentIdAsync(long ParentId, long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId && x.ParentCategoryId == ParentId);
        }

        public async Task<IEnumerable<Category>> GetParentCategoriesAsync(long RestaurantId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == RestaurantId && x.ParentCategoryId == null);
        }

        public async Task<Category> GetIdbyNameAsync(long restaurantId , string Name)
        {
           
            IEnumerable<Category> result =  await _repo.GetAllAsync(x => x.RestaurantId == restaurantId && x.Name.Equals(Name));

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Category>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "MenuItems,Items,CouponCategories");
        }

        public async Task<IEnumerable<Category>> GetGeneralCategoriesAsync(long restaurantId)
        {
            return await _repo.GetGeneralCategories(restaurantId);
        }


        public async Task<Category> UpdateCategoryAsync(Category Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<long> GetAllCategoriesCountAsync(long restaurantId)
        {
            return await _repo.GetCount(x => x.RestaurantId == restaurantId);
        }

        public async Task<IEnumerable<Category>> AddRangeAsync(IEnumerable<Category> List)
        {
            return await _repo.InsertRangeAsync(List);
        }

        public async Task<long> GetPositionCount(long RestaurantId)
        {
            IEnumerable<Category> result = await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId);

            if(result.Any())
                return result.Max(x => x.Position);

            return 0;
        }
    }
}
