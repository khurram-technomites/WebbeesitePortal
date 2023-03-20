using AutoMapper;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RestaurantRatingController : Controller
    {
        private readonly IRestaurantRatingClient _client;
        private readonly IMapper _mapper;
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        [BindProperty]
        public RestaurantRatingViewModel Model { get; set; }
        public RestaurantRatingController(IMapper mapper, IRestaurantRatingClient client)
        {
            _client = client;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index()
        {
            IEnumerable<RestaurantRatingViewModel> info = _mapper.Map<IEnumerable<RestaurantRatingViewModel>>(await _client.GetRestaurantRatings());
            return View(info);
        }
        public async Task<IActionResult> Details(long RatingId)
        {
            RestaurantRatingViewModel restaurantRating = _mapper.Map<RestaurantRatingViewModel>(await _client.GetRestaurantRatingByID(RatingId));
            return View(restaurantRating);
        }
        public async Task<ActionResult> ToggleActiveStatus(long RatingId, string status)
        {
            try
            {
                if (status == true.ToString().ToLower())
                {
                    status = Enum.GetName(typeof(Status), Status.Approved);
                }
                else
                {
                    status = Enum.GetName(typeof(Status), Status.Rejected);
                }
                RestaurantRatingViewModel Result = _mapper.Map<RestaurantRatingViewModel>(await _client.ToggleActiveStatus(RatingId, status));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        CreationDate = Result.CreatedOn.ToString("dd MMM yyyy, hh:mm tt"),
                        Restaurant = Result.RestaurantId,
                        UserName = Result.User.Logo + "|" + Result.User.FirstName + "|" + Result.User.PhoneNumber,
                        Rating = Result.Rating,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Approved) ? true : false,
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
