using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _service;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRouteGroupService _routeGroupService;
        private readonly IUserRefreshTokenService _userRefreshTokenService;
        private readonly IMapper _mapper;

        public GroupController(IGroupService service, IMapper mapper, UserManager<AppUser> userManager,
            IRouteGroupService routeGroupService, IUserRefreshTokenService userRefreshTokenService)
        {
            _service = service;
            _userManager = userManager;
            _routeGroupService = routeGroupService;
            _userRefreshTokenService = userRefreshTokenService;

            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            return Ok(_mapper.Map<IEnumerable<GroupDTO>>(await _service.GetAllGroupsAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetGroupsById(int Id)
        {
            IEnumerable<GroupDTO> List = _mapper.Map<IEnumerable<GroupDTO>>(await _service.GetGroupByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> AddGroupAsync(GroupDTO Entity)
        {
            Entity.CreatedOn = DateTime.UtcNow;
            return Ok(_mapper.Map<GroupDTO>(await _service.AddGroupAsync(_mapper.Map<Group>(Entity))));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGroupAsync(GroupDTO Entity)
        {
            if (Entity.IsActive == false)
                await RemoveUserClaims(Entity.GroupId, false);

            return Ok(_mapper.Map<GroupDTO>(await _service.UpdateGroupAsync(_mapper.Map<Group>(Entity))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> ArchiveGroupAsync(int Id)
        {
            IEnumerable<Group> List = await _service.GetGroupByIdAsync(Id);
            Group Model = new Group();

            if (List.Count() > 0)
            {
                Model = List.FirstOrDefault();
                Model.IsDeleted = true;
            }            

            await RemoveUserClaims(Model.GroupId, true);

            GroupDTO GroupDTO = _mapper.Map<GroupDTO>(await _service.UpdateGroupAsync(Model));

            return Ok(GroupDTO);
        }

        private async Task RemoveUserClaims(int GroupId, bool DeleteClaims)
        {
            IEnumerable<RouteGroup> RouteList = await _routeGroupService.GetByGroup(GroupId);
            var Claims = new List<Claim>();
            IList<AppUser> UserList = new List<AppUser>();

            foreach (var route in RouteList)
            {
                Claims.Add(new Claim(route.Route.RoutePath, route.GroupId.ToString()));

                var Users = await _userManager.GetUsersForClaimAsync(new Claim(route.Route.RoutePath, route.GroupId.ToString()));

                foreach (var user in Users)
                    UserList.Add(user);
            }

            foreach (var user in UserList.GroupBy(x => x.UserName).Select(x => x.First()).ToList())
            {
                if (DeleteClaims)
                    await _userManager.RemoveClaimsAsync(user, Claims);

                await _userRefreshTokenService.DeleteRefreshTokenAsync(user.Id);
            }
        }
    }
}
