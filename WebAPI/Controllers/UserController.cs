using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly IRouteGroupService _routeGroupService;

        public UserController(IUserService service, IMapper mapper, UserManager<AppUser> userManager,
            ILogger<UserController> logger, IRouteGroupService routeGroupService)
        {
            _service = service;
            _routeGroupService = routeGroupService;
            _mapper = mapper;

            _userManager = userManager;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            //PagingParameters paging = new PagingParameters();
            //paging.PageSize = PageSize;
            //paging.Skip = Skip;
            //paging.Search = Search;

            return Ok(_mapper.Map<IEnumerable<UserDTO>>(await _service.GetAllUsersAsync()));
        }
        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetUserById(string UserId)
        {
            AppUser List = await _userManager.FindByIdAsync(UserId);
            var getRole = await _userManager.GetRolesAsync(List);
            UserDTO model = _mapper.Map<UserDTO>(List);
            model.Role = getRole.FirstOrDefault();
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserDTO Entity)
        {

            var User = new AppUser
            {
                UserName = Entity.Email,
                Email = Entity.Email,
                FirstName = Entity.FirstName,
                LastName = Entity.LastName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true,
                PhoneNumber = Entity.PhoneNumber,
                CreatedOn = DateTime.UtcNow,
                LoginFor = Enum.GetName(typeof(Logins), Logins.Admin)
            };

            IEnumerable<AppUser> IsExist = await _service.GetUserByNumber(Entity.PhoneNumber);

            if (IsExist.Any())
                return Conflict(new ErrorDetails(401, "User is already register with this phone number !", string.Empty));

            IdentityResult result = await _userManager.CreateAsync(User, Entity.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(User, Entity.Role);

                _logger.LogInformation("Registration Success for " + User.LastName);

                return Ok(_mapper.Map<UserDTO>(User));
            }
            else
                return BadRequest(result.Errors.First<IdentityError>().Description);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserDTO Entity)
        {
            AppUser User = await _userManager.FindByIdAsync(Entity.UserId);

            if (Entity.PhoneNumber != User.PhoneNumber)
            {
                IEnumerable<AppUser> IsExist = await _service.GetUserByNumberAndCheck(Entity.PhoneNumber, "Admin");

                if (IsExist.Any())
                    return Conflict("User is already register with this phone number !");
            }

            User.Email = Entity.Email;
            User.FirstName = Entity.FirstName;
            User.LastName = Entity.LastName;
            User.PhoneNumber = Entity.PhoneNumber;         

            await _userManager.UpdateAsync(User);

            IList<string> List = await _userManager.GetRolesAsync(User);

            if (Entity.Role != List.FirstOrDefault())
            {
                await _userManager.RemoveFromRolesAsync(User, List);
                await _userManager.AddToRoleAsync(User, Entity.Role);
            }

            if (!string.IsNullOrEmpty(Entity.Password))
            {
                var token = _userManager.GeneratePasswordResetTokenAsync(User);
                await _userManager.ResetPasswordAsync(User, token.ToString(), Entity.Password);
            }

            _logger.LogInformation("Record successfully updated for {0}", User.UserName);

            return Ok(User);
        }

        [HttpDelete("{UserId}")]
        public async Task<IActionResult> ArchiveUser(string UserId)
        {
            AppUser User = await _userManager.FindByIdAsync(UserId);

            User.IsDeleted = true;
            User.IsActive = false;

            await _userManager.UpdateAsync(User);

            return Ok();
        }

        [HttpPut("ChangePassword/{UserId}/{OldPassword}/{NewPassword}")]
        public async Task<ActionResult> ChangePassword(string UserId, string OldPassword, string NewPassword)
        {
            AppUser User = await _userManager.FindByIdAsync(UserId);
            IdentityResult ChangePassword = await _userManager.ChangePasswordAsync(User, OldPassword, NewPassword);
            return Ok();
        }
        [HttpPost("AsignRouteGroups")]
        public async Task<IActionResult> AssignRoutesToUser(UserRouteDTO Model)
        {
            AppUser User = await _userManager.FindByIdAsync(Model.UserId);

            IEnumerable<RouteGroup> RouteList = await _routeGroupService.GetByGroup(Model.GroupId);

            var ClaimList = new List<Claim>();

            foreach (var route in RouteList)
                ClaimList.Add(new Claim(route.Route.RoutePath, route.GroupId.ToString()));

            await _userManager.AddClaimsAsync(User, ClaimList);

            return Ok();

        }

        [HttpPut("ToggleStatus/{UserId}")]
        public async Task<IActionResult> ToggleStatus(string UserId)
        {
            AppUser User = await _userManager.FindByIdAsync(UserId);

            User.IsActive = User.IsActive != true;
            await _userManager.UpdateAsync(User);

            _logger.LogInformation("Record successfully updated for {0}", User.UserName);

            return Ok(User);
        }

    }
}
