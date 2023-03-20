using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepo _repo;

        public GroupService(IGroupRepo repo)
        {
            _repo = repo;
        }

        public async Task<Group> AddGroupAsync(Group Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            return await _repo.GetAllAsync(x => x.IsDeleted == false);
        }

        public async Task<IEnumerable<Group>> GetGroupByIdAsync(int Id)
        {
            return await _repo.GetByIdAsync(x => x.GroupId == Id && x.IsDeleted == false);
        }

        public async Task<Group> UpdateGroupAsync(Group Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
