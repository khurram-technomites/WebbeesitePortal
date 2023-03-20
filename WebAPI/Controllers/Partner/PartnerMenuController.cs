using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Partner
{
    [Route("api/Partner/Menu")]
    [ApiController]
    [Authorize(Roles = "RestaurantServiceStaff")]
    public class PartnerMenuController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IRestaurantServiceStaffService _restaurantServiceStaffService;
        private readonly IMenuItemService _menuItemService;
        private readonly IMapper _mapper;

        public PartnerMenuController(IRestaurantService restaurantService, IMapper mapper, IMenuItemService menuItemService,
            IRestaurantServiceStaffService restaurantServiceStaffService)
        {
            _restaurantService = restaurantService;
            _menuItemService = menuItemService;
            _restaurantServiceStaffService = restaurantServiceStaffService;
            _mapper = mapper;
        }

        [HttpGet("ByBranch/{BranchId}")]
        public async Task<IActionResult> Menu(long BranchId)
        {
            return Ok(await _restaurantService.GetBranchMenuForPartner(BranchId));
        }

        [HttpPut("{MenuId}/Category/{CategoryId}/ToggleStatus/{StatusId}")]
        public async Task<IActionResult> ToggleStatus(long MenuId, long CategoryId, Status StatusId)
        {
            IEnumerable<MenuItem> menuItems = await _menuItemService.GetAllByCategoryAndMenuAsync(CategoryId, MenuId);

            foreach (var menuitem in menuItems)
                menuitem.Status = Enum.GetName(typeof(Status), StatusId);

            await _menuItemService.UpdateMenuItemAsync(menuItems);

            return Ok(new SuccessResponse<string>(string.Format("Category {0} successfully", Enum.GetName(typeof(Status), StatusId)), ""));
        }

        [HttpPut("MenuItem/{MenuItemId}/ToggleStatus/{StatusId}")]
        public async Task<IActionResult> ToggleStatus(long MenuItemId, Status StatusId)
        {
            IEnumerable<MenuItem> menuItems = await _menuItemService.GetById(MenuItemId);

            menuItems.FirstOrDefault().Status = Enum.GetName(typeof(Status), StatusId);

            await _menuItemService.UpdateMenuItemAsync(menuItems.FirstOrDefault());

            return Ok(new SuccessResponse<string>(string.Format("Item {0} successfully", Enum.GetName(typeof(Status), StatusId)), ""));
        }

        [HttpPut]
        public async Task<IActionResult> Update(MenuCategoryItemDTO MenuItem)
        {
            IEnumerable<MenuItem> menuItems = await _menuItemService.GetById(MenuItem.Id);

            MenuItem menuItem = _mapper.Map(MenuItem, menuItems.FirstOrDefault());

            return Ok(new SuccessResponse<MenuCategoryItemDTO>("", _mapper.Map<MenuCategoryItemDTO>(await _menuItemService.UpdateMenuItemAsync(menuItem))));
        }

        [HttpGet("{MenuItemId}/Options")]
        public async Task<IActionResult> GetOptions(long MenuItemId)
        {
            IEnumerable<MenuItem> list = await _menuItemService.GetById(MenuItemId);

            return Ok(new SuccessResponse<MenuCategoryItemDTO>("", _mapper.Map<MenuCategoryItemDTO>(list.FirstOrDefault())));
        }
    }
}
