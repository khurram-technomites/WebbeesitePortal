using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using HelperClasses.DTOs.Aggregators;
using HelperClasses.DTOs.CardScheme;
using WebApp.ErrorHandling;
using Newtonsoft.Json;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CardSchemeController : Controller
    {
        private readonly ICardSchemeClient _client;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IRestaurantBranchClient _restaurantBranchClient;
        public CardSchemeController(ICardSchemeClient client, IMapper mapper, IUserSessionManager userSessionManager, IRestaurantBranchClient restaurantBranchClient)
        {
            _client = client;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _restaurantBranchClient = restaurantBranchClient;
        }

        public async Task<IActionResult> Index()
        {
            PagingParameters paging = new PagingParameters();
            paging.PageNumber = 1;
            paging.PageSize = 10;

            IEnumerable<CardSchemeViewModel> Info = _mapper.Map<IEnumerable<CardSchemeViewModel>>(await _client.GetAllCardSchemeAsync());
            return View(Info);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CardSchemeViewModel model)
        {
            try
            {
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    CardSchemeDTO Result = await _client.AddCardSchemeAsync(_mapper.Map<CardSchemeDTO>(model));


                    return Json(new
                    {
                        success = true,
                        url = "/Admin/CardScheme/Index",
                        message = "Record Added Successfully",
                        data = new
                        {
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.Type,
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
            var Info = _mapper.Map<IEnumerable<CardSchemeViewModel>>(await _client.GetCardSchemeByIdAsync(id));
            return View(Info.FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CardSchemeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CardSchemeDTO Result = await _client.UpdateCardSchemeAsync(_mapper.Map<CardSchemeDTO>(model));


                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/CardScheme/Index",
                        message = "Record Added Successfully",
                        data = new
                        {
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.Type,
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
                await _client.DeleteCardSchemeAsync(id);

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
            var Info = _mapper.Map<IEnumerable<CardSchemeViewModel>>(await _client.GetCardSchemeByIdAsync(id));
            return View(Info.FirstOrDefault());
        }

    }
}
