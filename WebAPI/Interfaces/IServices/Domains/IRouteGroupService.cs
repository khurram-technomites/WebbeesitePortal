using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRouteGroupService
    {
        Task<IEnumerable<RouteGroup>> GetAllRouteGroups();
        Task<IEnumerable<RouteGroup>> GetRouteGroupsById(int Id);
        Task<IEnumerable<RouteGroup>> GetByGroup(int Id);
        Task<IEnumerable<RouteGroup>> GetByGroupForLogin(int Id);
        Task<IEnumerable<RouteGroup>> AddRangeAsync(IEnumerable<RouteGroup> Entities);
        Task<IEnumerable<RouteGroup>> UpdateRangeAsync(IEnumerable<RouteGroup> Entities);
    }
}
