using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelperClasses.DTOs;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;
using Fingers10.ExcelExport.ActionResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using HelperClasses.Classes;
using System.Linq;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserClient _client;
        private readonly IUserRoleClient _role;

        [BindProperty]
        public UserViewModel Model { get; set; }

        public UserController(IMapper mapper, IUserClient client, IUserRoleClient role)
        {
            _mapper = mapper;
            _client = client;
            _role = role;
        }

        public async Task<IActionResult> Index()
        {
            var user = _mapper.Map<IEnumerable<UserViewModel>>(await _client.GetAllUsersAsync());
            return View(user.Where(x => x.Email != "customer@fougito.com"));
        }
        public async Task<IActionResult> Create(string UserID)
        {

            var roles = _mapper.Map<IEnumerable<IdentityUserRoleViewModel>>(await _role.GetUserRoles());
            List<IdentityUserRoleViewModel> ListRole = new List<IdentityUserRoleViewModel>();
            foreach (var item in roles)
            {
                if (!Enum.GetNames(typeof(Roles)).Contains(item.Name))
                {

                    ListRole.Add(item);


                }

            }
            ViewBag.Roles = ListRole;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roles = _mapper.Map<IEnumerable<IdentityUserRoleViewModel>>(await _role.GetUserRoles());
                List<IEnumerable<IdentityUserRoleViewModel>> ListRole = new List<IEnumerable<IdentityUserRoleViewModel>>();
                foreach (var item in roles)
                {
                    if (!Enum.GetNames(typeof(Roles)).Contains(item.Name))
                    {

                        ListRole.Append(roles);

                    }

                }
                ViewBag.Roles = ListRole;
                try
                {
                    /*return RedirectToAction(nameof(Create), new { UserID = "" });*/
                    //model.Role = Enum.GetName(typeof(Roles), Roles.Admin);
                    UserDTO Result = await _client.AddUserAsync(_mapper.Map<UserDTO>(model));
                    Result.IsActive = true;
                    return Json(new
                    {
                        success = true,
                        message = "Record Created Successfully",
                        data = new
                        {
                            Date = Result.CreatedOn.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.LastName,
                            /* PhoneNumber = Result.PhoneNumber,*/
                            Email = Result.Email,
                            IsActive = Result.IsActive = true,
                            UserId = Result.UserId
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

        public async Task<IActionResult> Edit(string UserId)
        {

            var roles = _mapper.Map<IEnumerable<IdentityUserRoleViewModel>>(await _role.GetUserRoles());
            List<IdentityUserRoleViewModel> ListRole = new List<IdentityUserRoleViewModel>();
            foreach (var item in roles)
            {
                if (!Enum.GetNames(typeof(Roles)).Contains(item.Name))
                {

                    ListRole.Add(item);


                }

            }
            ViewBag.Roles = ListRole;
            UserViewModel user = _mapper.Map<UserViewModel>(await _client.GetUserByIdAsync(UserId));
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormCollection Form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserDTO Result = await _client.UpdateUserAsync(_mapper.Map<UserDTO>(Model));

                    Result.UserId = Model.UserId;
                    //Result.IsActive = Model.IsActive;

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            Date = Result.CreatedOn.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.LastName,
                            /*  PhoneNumber = Result.PhoneNumber,*/
                            Email = Result.Email,
                            IsActive = Result.IsActive == true ? true : false,
                            UserId = Result.UserId
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

        public async Task<IActionResult> Details(string UserId)
        {
            UserViewModel user = _mapper.Map<UserViewModel>(await _client.GetUserByIdAsync(UserId));
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string UserId)
        {
            try
            {
                await _client.DeleteUserAsync(UserId);

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

        public async Task<ActionResult> ToggleActiveStatus(string UserId)
        {
            try
            {
                UserViewModel Result = _mapper.Map<UserViewModel>(await _client.ToggleActiveStatus(UserId));
                Result.UserId = UserId;


                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        Date = Result.CreatedOn.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.LastName,
                        /*PhoneNumber = Result.PhoneNumber,*/
                        Email = Result.Email,
                        IsActive = Result.IsActive,
                        UserId = Result.UserId
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
    }
}
