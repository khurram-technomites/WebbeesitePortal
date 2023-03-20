using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IUserService
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();

        Task<long> GetAllUsersCountAsync();
        Task<IEnumerable<AppUser>> GetUsersByIdAsync(string UserId);
        Task<IEnumerable<AppUser>> GetUsersByLogoAsync(string Logo);
        Task<IEnumerable<AppUser>> GetUserByNumberAndCheck(string contact, string LoginFor);
        Task<IEnumerable<AppUser>> GetUserByEmailAndCheck(string email, string LoginFor);
        Task<IEnumerable<AppUser>> GetUserByNumber(string contact, string id = "");


    }
}
