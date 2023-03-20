using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Handlers;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [UserPrivileges]
    [Authorize]

    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserClient _client;

        [BindProperty]
        public UserViewModel Model { get; set; }

        public UserController(IMapper mapper, IUserClient client)
        {
            _mapper = mapper;
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<UserViewModel>>(await _client.GetAllUsersAsync())); ;
        }



        //[HttpPost]
        //public async Task<JsonResult> GetList()
        //{
        //    int recordsTotal = await _client.GetTotalRecordsOfUsers();

        //    var draw = Request.Form["draw"].FirstOrDefault();
        //    var start = Request.Form["start"].FirstOrDefault();
        //    var length = Request.Form["length"].FirstOrDefault();
        //    var searchValue = Request.Form["search[value]"].FirstOrDefault();
        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt32(start) : 1;

        //    IEnumerable<UserViewModel> List = _mapper.Map<IEnumerable<UserViewModel>>(await _client.GetAllUsersAsync(skip, pageSize, searchValue));

        //    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = List });
        //}


        public IActionResult Create(string UserID)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return RedirectToAction(nameof(Create), new { UserID = "" });
                    UserDTO Result = await _client.AddUserAsync(_mapper.Map<UserDTO>(model));

                    return Json(new
                    {
                        success = true,
                        message = "Record Created Successfully",
                        data = _mapper.Map<UserViewModel>(Result)
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

        public async Task<IActionResult> Edit([FromQuery] string UserId)
        {
            return View(_mapper.Map<UserViewModel>(await _client.GetUserByIdAsync(UserId)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection Form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserDTO Result = await _client.UpdateUserAsync(_mapper.Map<UserDTO>(Model));

                    Result.UserId = Model.UserId;

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = _mapper.Map<UserViewModel>(Result)
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

        public async Task<IActionResult> Detail([FromQuery] string UserId)
        {
            return View(_mapper.Map<UserViewModel>(await _client.GetUserByIdAsync(UserId)));
        }

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
                    data = Result

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
