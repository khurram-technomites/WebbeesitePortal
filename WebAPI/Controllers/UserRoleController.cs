using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserRoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UserRoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var getRole = await _roleManager.Roles.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<IdentityUserRoleDTO>>(getRole));
        }

        [HttpGet("{RoleId}")]
        public async Task<IActionResult> GetUserRoleByRoleId(string RoleId)
        {
            var getRole = _roleManager.Roles.Where(x => x.Id == RoleId).FirstOrDefault();
            return Ok(_mapper.Map<IdentityUserRoleDTO>(getRole));
        }

        [HttpPost("{RoleName}")]
        public async Task<IActionResult> Post(string RoleName)
        {
            IdentityRole role = new();
            role.Name = RoleName;
            role.NormalizedName = "ADMIN-" + RoleName.ToUpper();
            await _roleManager.CreateAsync(role);

            return Ok(_mapper.Map<IdentityRole>(role));
        }

        [HttpPut("{RoleId}/{RoleName}")]
        public async Task<IActionResult> Put(string RoleId, string RoleName)
        {

            IdentityRole role = new();
            var roleForUpdate = _roleManager.Roles.Where(x => x.Id == RoleId).FirstOrDefault();
            roleForUpdate.Name = RoleName;
            roleForUpdate.NormalizedName = "ADMIN-" + RoleName;
            return Ok(await _roleManager.UpdateAsync(roleForUpdate));
        }

        [HttpDelete("{RoleId}")]
        public async Task<IActionResult> Delete(string RoleId)
        {
            IdentityRole role = new();
            var roleForUpdate = _roleManager.Roles.Where(x => x.Id == RoleId).FirstOrDefault();
            await _roleManager.DeleteAsync(roleForUpdate);

            return Ok();
        }

    }
}
