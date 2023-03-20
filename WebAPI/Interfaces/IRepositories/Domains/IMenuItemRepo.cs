using HelperClasses.DTOs.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface IMenuItemRepo : IRepository<MenuItem>
    {
        Task<List<MenuItemByMenuDTO>> GetMenuItemByMenuId(long menuId);
        Task<bool> CheckMainPrice(long id);
    }
}
