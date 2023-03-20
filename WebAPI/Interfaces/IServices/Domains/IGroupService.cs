using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetAllGroupsAsync();
        Task<IEnumerable<Group>> GetGroupByIdAsync(int Id);
        Task<Group> AddGroupAsync(Group Entity);
        Task<Group> UpdateGroupAsync(Group Entity);        
    }
}
