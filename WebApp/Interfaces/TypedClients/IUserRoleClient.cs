using HelperClasses.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IUserRoleClient
    {
        Task<IEnumerable<IdentityUserRoleDTO>> GetUserRoles();
        Task<IdentityUserRoleDTO> GetUserRoleByRoleId(string RoleId);
        Task<IdentityUserRoleDTO> Create(string Name);
        Task<IdentityUserRoleDTO> Edit(string RoleId , string RoleName);

        Task<IdentityUserRoleDTO> Delete(string RoleId);
    }
}
