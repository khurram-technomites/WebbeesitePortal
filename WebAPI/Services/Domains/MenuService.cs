using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepo _repo;
        public MenuService(IMenuRepo repo)
        {
            _repo = repo;
        }
        public async Task<Menu> AddMenuAsync(Menu Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<Menu> ArchiveMenuAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<Menu>> GetAllAsync(long RestaurantId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == RestaurantId , ChildObjects : "MenuItem");
        }
        
        public async Task<IEnumerable<Menu>> GetGeneralMenuAsync()
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == null, ChildObjects: "MenuItem");
        }

        public async Task<IEnumerable<Menu>> GetAllByBranchIdAsync(long branchId , long id)
        {
            return await _repo.GetAllAsync(x => x.RestaurantBranchId == branchId && x.Status == Enum.GetName(typeof(Status), Status.Active) && x.IsPeriodic == false && x.Id != id);
        }
        public async Task<long> GetAllMenusCountAsync(long RestaurantId)
        {
            return await _repo.GetCount(x => x.RestaurantId == RestaurantId);
        }

        public async Task<IEnumerable<Menu>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "MenuItem,MenuItem.MenuItemOptions,MenuItem.MenuItemOptions.MenuItemOptionValues");
        }

        public async Task<Menu> UpdateMenuAsync(Menu Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
