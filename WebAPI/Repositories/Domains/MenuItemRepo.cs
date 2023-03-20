using AutoMapper;
using HelperClasses.DTOs.Menu;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class MenuItemRepo : Repository<MenuItem>, IMenuItemRepo
    {
        private new readonly FougitoContext _context;
        private readonly IMapper _mapper;
        public MenuItemRepo(FougitoContext context, ILoggerManager logger, IMapper mapper) : base(context, logger)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MenuItemByMenuDTO>> GetMenuItemByMenuId(long menuId)
        {
            var result = await _context.MenuItems.Include(x => x.Menu).Include(x => x.Category)
                                                                            .Where(x => x.MenuId == menuId &&
                                                                            x.ArchivedDate == null).OrderBy(x=>x.CategoryPosition)                       //(x.Menu.EndTime.HasValue == false || x.Menu.EndTime >= DateTime.UtcNow))
                                                                            .ToListAsync();

            // Group at DB level was not doable
            List<MenuItemByMenuDTO> filter = (from MT in result
                                              group MT by new
                                              {
                                                  MT.CategoryId,
                                                  MT.Category.Name,
                                                  MT.Category.Status,
                                                  MT.Category.Description,
                                                  MT.Category.Image,
                                                  
                                              } into g
                                              select new MenuItemByMenuDTO
                                              {
                                                  CategoryId = g.Key.CategoryId,
                                                  CategoryName = g.Key.Name,
                                                  CategoryStatus = g.Key.Status,
                                                  CategoryDescription = g.Key.Description,
                                                  CategoryImage = g.Key.Image,
                                                  MenuItems = _mapper.Map<List<MenuItemByCategoryDTO>>(g.ToList())
                                              }).ToList();

            return filter;
        }

        public async Task<bool> CheckMainPrice(long id)
        {
            var result = await _context.MenuItems.Include(x => x.MenuItemOptions).Where(x => x.Id == id && x.ArchivedDate == null && x.MenuItemOptions.Any(x => x.IsPriceMain == true)).FirstOrDefaultAsync();

            return  result != null ? true : false;
        }
    }
}
