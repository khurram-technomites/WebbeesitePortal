using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _repo;

        public UserService(IUserRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            //if (string.IsNullOrEmpty(pagingParameters.Search))
            return await _repo.GetAllAsync(x => x.IsDeleted == false && x.LoginFor == Enum.GetName(typeof(Logins), Logins.Admin), null, null, OrderExp: x => x.Email);
            //else            //    return await _repo.GetAllAsync(x => x.IsDeleted == false && EF.Functions.Like(x.Email, "%" + pagingParameters.Search + "%") ||
            //    EF.Functions.Like(x.FirstName, "%" + pagingParameters.Search + "%") ||
            //    EF.Functions.Like(x.LastName, "%" + pagingParameters.Search + "%"), null, pagingParameters);
        }
        public async Task<long> GetAllUsersCountAsync()
        {
            return await _repo.GetCount(x => !x.IsDeleted && x.IsActive && x.LoginFor == "Admin");
        }
        public async Task<IEnumerable<AppUser>> GetUserByEmailAndCheck(string email, string LoginFor)
        {
            return await _repo.GetByIdAsync(x => x.Email == email && x.LoginFor == LoginFor);
        }

        public async Task<IEnumerable<AppUser>> GetUserByNumberAndCheck(string contact, string LoginFor)
        {
            return await _repo.GetByIdAsync(x => x.PhoneNumber == contact && x.LoginFor == LoginFor);
        }

        public async Task<IEnumerable<AppUser>> GetUserByNumber(string contact , string id = "")
        {
            return await _repo.GetByIdAsync(x => x.PhoneNumber == contact && x.Id != id);
        }

        public async Task<IEnumerable<AppUser>> GetUsersByIdAsync(string UserId)
        {
            return await _repo.GetByIdAsync(x => x.Id == UserId && x.IsDeleted == false);
        }

        public async Task<IEnumerable<AppUser>> GetUsersByLogoAsync(string Logo)
        {
            return await _repo.GetByIdAsync(x => x.Logo == Logo);
        }

        public int TotalRecord()
        {
            return _repo.RecordCount();
        }
    }
}
