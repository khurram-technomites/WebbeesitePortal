using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RouteGroupService : IRouteGroupService
    {
        private readonly IRouteGroupsRepo _repo;
        public RouteGroupService(IRouteGroupsRepo repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<RouteGroup>> AddRangeAsync(IEnumerable<RouteGroup> Entities)
        {
            return _repo.InsertRangeAsync(Entities);
        }

        public Task<IEnumerable<RouteGroup>> GetAllRouteGroups()
        {
            return _repo.GetAllAsync(x => x.Group.IsDeleted == false, "Group, Route");
        }

        public Task<IEnumerable<RouteGroup>> GetByGroup(int Id)
        {
            return _repo.GetByIdAsync(x => x.GroupId == Id && x.Group.IsDeleted == false, "Group, Route");
        }

        public Task<IEnumerable<RouteGroup>> GetByGroupForLogin(int Id)
        {
            return _repo.GetByIdAsync(x => x.GroupId == Id && x.Group.IsActive == true && x.Group.IsDeleted == false, "Group, Route");
        }

        public Task<IEnumerable<RouteGroup>> GetRouteGroupsById(int Id)
        {
            return _repo.GetByIdAsync(x => x.RouteGroupId == Id, "Group, Route");
        }

        public Task<IEnumerable<RouteGroup>> UpdateRangeAsync(IEnumerable<RouteGroup> Entities)
        {
            return _repo.UpdateRangeAsync(Entities);
        }
    }
}
