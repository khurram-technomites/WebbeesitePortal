using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelperClasses.DTOs;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;
using Fingers10.ExcelExport.ActionResults;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private readonly IUserRoleClient _role;
        private readonly IMapper _mapper;
        [BindProperty]
        public IdentityUserRoleViewModel Model { get; set; }
        public UserRoleController(IUserRoleClient role, IMapper mapper)
        {
            _role = role;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<IdentityUserRoleViewModel>>(await _role.GetUserRoles());
            return View(info);
        }
        public async Task<ActionResult> List()
        {
            return PartialView();
        }
        public async Task<IActionResult> Details(string RoleId)
        {
            IdentityUserRoleViewModel RoleDetail = _mapper.Map<IdentityUserRoleViewModel>(await _role.GetUserRoleByRoleId(RoleId));
            return View(RoleDetail);
        }
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(string RoleName)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    IdentityUserRoleDTO role = await _role.Create(RoleName);

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/UserRole/Index",
                        message = "User Role Created Successfully",
                        data = new
                        {
                            ID = role.Id,
                            role.Name,
                        }
                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Admin/UserRole/Index",
                        success = false,
                        message = err.Message
                    });
                }
            }

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string RoleId, string RoleName)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    IdentityUserRoleDTO Result = _mapper.Map<IdentityUserRoleDTO>(await _role.Edit(RoleId, RoleName));

                    Result.Id = RoleId;

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Name = Result.Name,
                        }
                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        success = false,
                        message = err.Message
                    });
                }

            }

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });

        }
        public async Task<IActionResult> Edit(string RoleId)
        {
            IdentityUserRoleViewModel userRoleDTO = _mapper.Map<IdentityUserRoleViewModel>(await _role.GetUserRoleByRoleId(RoleId));
            return View(userRoleDTO);
        }

        public async Task<ActionResult> Delete(string RoleId)
        {
            try
            {
                await _role.Delete(RoleId);

                return Json(new
                {
                    success = true,
                    message = "Record Deleted Successfully"
                });
            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                    message = err.Message
                });
            }
        }
    }
}
