using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IUserClient
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(string UserId);
        Task<bool> ChangePasswordAsync(string UserId , string OldPassword , string NewPassword);
        Task<UserDTO> AddUserAsync(UserDTO Entity);
        Task<int> GetTotalRecordsOfUsers();
        Task<UserDTO> UpdateUserAsync(UserDTO Entity);
        Task DeleteUserAsync(string UserId);
        Task<UserDTO> ToggleActiveStatus(string UserId);

    }
}
