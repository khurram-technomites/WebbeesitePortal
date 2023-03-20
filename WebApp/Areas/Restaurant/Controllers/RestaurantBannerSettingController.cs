using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
	[Authorize(Roles = "RestaurantOwner")]
	public class RestaurantBannerSettingController : Controller
    {
        private readonly IRestaurantBannerSettingClient _client;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IMapper _mapper;
        private readonly IFileUpload _fileUpload;

        public RestaurantBannerSettingController(IRestaurantBannerSettingClient client, IUserSessionManager userSessionManager, IMapper mapper, IFileUpload fileUpload)
        {
            _client = client;   
            _userSessionManager = userSessionManager;
            _mapper = mapper;
            _fileUpload = fileUpload;
        }

        public async Task<IActionResult> Index()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            IEnumerable<RestaurantBannerSettingViewModel> mainBanner = _mapper.Map<IEnumerable<RestaurantBannerSettingViewModel>>(await _client.GetBannerByType(restaurantId , "Banner"));
            IEnumerable<RestaurantBannerSettingViewModel> promotionBanner = _mapper.Map<IEnumerable<RestaurantBannerSettingViewModel>>(await _client.GetBannerByType(restaurantId, "PromotionBanner"));

            BannerViewModel banners = new BannerViewModel();
            banners.Banner = mainBanner.LastOrDefault();
            banners.PromotionBanner = promotionBanner.ToList();
            return View(banners);
        }

		[HttpPost]
		public async Task<IActionResult> Create(string image, string Url, string Lang)
		{
			string message = string.Empty;
			if (image != null && image.Length > 0)
				try
				{
					RestaurantBannerSettingViewModel objBannerImage = new RestaurantBannerSettingViewModel();
					objBannerImage.ImagePath = image;
                    objBannerImage.RestaurantId = _userSessionManager.GetUserStore().Id;
                    objBannerImage.Url = Url;
					objBannerImage.Lang = Lang;
					objBannerImage.BannerType = "Banner";

					await _client.AddRestaurantBannerSettingAsync(_mapper.Map<RestaurantBannerSettingDTO>(objBannerImage));
					ViewBag.Message = "File uploaded successfully";
					TempData["SuccessMessage"] = "File uploaded successfully";
				}
                catch (ApiException ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    TempData["ErrorMessage"] = "ERROR:" + ex.Message.ToString();
                }
            return RedirectPermanent("Index");
		}

		[HttpPost]
		public async Task<IActionResult> PromotionCreate(string image, string Url, string Lang)
		{
			string message = string.Empty;
			if (image != null && image.Length > 0)
				try
				{
					RestaurantBannerSettingViewModel objBannerImage = new RestaurantBannerSettingViewModel();
					objBannerImage.ImagePath = image;
					objBannerImage.RestaurantId = _userSessionManager.GetUserStore().Id;
					objBannerImage.Url = Url;
					objBannerImage.Lang = Lang;
					objBannerImage.BannerType = "PromotionBanner";

					await _client.AddRestaurantBannerSettingAsync(_mapper.Map<RestaurantBannerSettingDTO>(objBannerImage));
					ViewBag.Message = "File uploaded successfully";
					TempData["SuccessMessage"] = "File uploaded successfully";
				}
				catch (ApiException ex)
				{
					ViewBag.Message = "ERROR:" + ex.Message.ToString();
					TempData["ErrorMessage"] = "ERROR:" + ex.Message.ToString();
				}
			return RedirectPermanent("Index");
		}

		public async Task<ActionResult> Edit(long id)
		{
			RestaurantBannerSettingViewModel model  = _mapper.Map<RestaurantBannerSettingViewModel>(await _client.GetRestaurantBannerSettingByIdAsync(id));
			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Edit(long id, string Url, string Lang)
		{
			try
			{
				
				RestaurantBannerSettingViewModel model = _mapper.Map<RestaurantBannerSettingViewModel>(await _client.GetRestaurantBannerSettingByIdAsync(id));

				model.Url = Url;
				model.Lang = Lang;

				RestaurantBannerSettingDTO result = await _client.UpdateRestaurantBannerSettingAsync(_mapper.Map<RestaurantBannerSettingDTO>(model));
					return Json(new
					{
						success = true,
						message = "Banner Details updated successfully!",
						data = new { lang = result.Lang }
					});
				
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

		public async Task<ActionResult> PromotionEdit(long id)
		{
			RestaurantBannerSettingViewModel model = _mapper.Map<RestaurantBannerSettingViewModel>(await _client.GetRestaurantBannerSettingByIdAsync(id));
			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> PromotionEdit(long id, string Url, string Lang)
		{
			try
			{

				RestaurantBannerSettingViewModel model = _mapper.Map<RestaurantBannerSettingViewModel>(await _client.GetRestaurantBannerSettingByIdAsync(id));

				model.Url = Url;
				model.Lang = Lang;

				RestaurantBannerSettingDTO result = await _client.UpdateRestaurantBannerSettingAsync(_mapper.Map<RestaurantBannerSettingDTO>(model));
				return Json(new
				{
					success = true,
					message = "Promotion Banner updated successfully!",
					data = new { lang = result.Lang }
				});

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

		public async Task<ActionResult> Delete(long id)
		{

			try
			{
				await _client.DeleteRestaurantBannerSettingAsync(id);
			
				return Json(new
				{
					success = true,
					message = "Record successfully deleted"
				});
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
	}
}
