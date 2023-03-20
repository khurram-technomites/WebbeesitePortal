using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class RestaurantRatingController : Controller
    {
        private readonly IRestaurantRatingClient _client;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        [BindProperty]
        public RestaurantRatingViewModel Model { get; set; }
        public RestaurantRatingController(IMapper mapper, IRestaurantRatingClient client , IUserSessionManager userSession)
        {
            _client = client;
            _mapper = mapper;
            _userSession = userSession;
        }
        public async Task<ActionResult> Index()
        {
            var RestaurantId = _userSession.GetUserStore().Id;
            var info = _mapper.Map<IEnumerable<RestaurantRatingViewModel>>(await _client.GetRestaurantRatingByRestaurantID(RestaurantId));
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
                        Showonweb = Result.ShowOnWebsite
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
        [HttpPost]
        public async Task<ActionResult> ChangeStatus(string status )
        {
            var RestaurantId = _userSession.GetUserStore().Id;
            IEnumerable<RestaurantRatingViewModel> model = _mapper.Map<IEnumerable<RestaurantRatingViewModel>>(await _client.GetRestaurantRatingByStatus(status , RestaurantId));
            return PartialView(model);
        }
        [HttpPost]
        public async Task<ActionResult> GoLive(long RatingId)
        {
            RestaurantRatingViewModel restaurantRating = _mapper.Map<RestaurantRatingViewModel>(await _client.GetRestaurantRatingByID(RatingId));
            restaurantRating.Id = RatingId;
            restaurantRating.User = null;
            restaurantRating.Restaurant = null;

            RestaurantRatingDTO Result = await _client.Edit(_mapper.Map<RestaurantRatingDTO>(restaurantRating));

            Result.Id = restaurantRating.Id;

            return Json(new
            {
                success = true,
                message = "Record Updated Successfully",
                data = new
                {
                    success = false,
                    message = "Rating live successfully!"
                }
            });

        }
    }
}
