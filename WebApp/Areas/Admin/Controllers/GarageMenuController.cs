using AutoMapper;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;
using HelperClasses.DTOs.CardScheme;
using HelperClasses.DTOs.GarageCMS;
using WebApp.ErrorHandling;
using System.Linq;
using Newtonsoft.Json;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GarageMenuController : Controller
    {
        private readonly IGarageMenuClient _client;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;
        public GarageMenuController(IGarageMenuClient client, IMapper mapper, IUserSessionManager userSessionManager)
        {
            _client = client;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
        }

        public async Task<IActionResult> Index()
        {
            PagingParameters paging = new PagingParameters();
            paging.PageNumber = 1;
            paging.PageSize = 10;

            IEnumerable<GarageMenuViewModel> Info = _mapper.Map<IEnumerable<GarageMenuViewModel>>(await _client.GetAllAsync());
            return View(Info);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GarageMenuViewModel model)
        {
            try
            {
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    GarageMenuDTO Result = await _client.AddGarageMenuAsync(_mapper.Map<GarageMenuDTO>(model));


                    return Json(new
                    {
                        success = true,
                        url = "/Admin/GarageMenu/Index",
                        message = "Record Added Successfully",
                        data = new
                        {
                            Title = Result.Title,
                            Route = Result.Route,
                            ID = Result.Id,
                        }
                    });


                }
                else
                {
                    message = "Please fill the form properly ...";
                }
                return Json(new { success = false, message = message });
            }
            catch (ApiException ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                });
            }


        }

        public async Task<IActionResult> Edit(long id)
        {
            var Info = _mapper.Map<IEnumerable<GarageMenuViewModel>>(await _client.GetAllByIdAsync(id));
            return View(Info.FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GarageMenuViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    GarageMenuDTO Result = await _client.UpdateGarageMenuAsync(_mapper.Map<GarageMenuDTO>(model));


                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/GarageMenu/Index",
                        message = "Record Added Successfully",
                        data = new
                        {
                            Title = Result.Title,
                            Route = Result.Route,
                            ID = Result.Id,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _client.DeleteGarageMenuAsync(id);

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

        public async Task<IActionResult> Details(long id)
        {
            var Info = _mapper.Map<IEnumerable<GarageMenuViewModel>>(await _client.GetAllByIdAsync(id));
            return View(Info.FirstOrDefault());
        }
    }
}
