using AutoMapper.Mappers;
using HelperClasses.Classes;
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
    public class GarageMenuRepo : Repository<GarageMenu>, IGarageMenuRepo
    {
        public GarageMenuRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {
           

        }

        public async Task<IEnumerable<GarageMenu>> GetMenuByGarageId(long GarageId)
        {
            List<GarageMenu> list = new List<GarageMenu>();
            var result2 = _context.GarageMenuManagement.Where(x => x.GarageId == GarageId).Select(x => x.GarageMenuId).Distinct().ToList();
            var result = await _context.GarageMenu.Where(x => !result2.Contains(x.Id) && x.ArchivedDate == null).ToListAsync();

            foreach (var item in result)
            {
                if (item.Title == "Home" || item.Title == "About Us")
                {
                    list.Add(item);
                }
                var ClientModule = await _context.ClientModules.Where(x => x.ModuleId == item.ModuleID && x.Status == Enum.GetName(typeof(ClientPaymentStatus), ClientPaymentStatus.Paid) && x.ClientId == GarageId).ToListAsync();
                if (ClientModule.Count != 0)
                {
                    list.Add(item);

                }
            }
            return list;
        }
    }
}
