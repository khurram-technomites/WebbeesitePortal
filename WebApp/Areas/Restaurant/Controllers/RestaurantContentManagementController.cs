using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
	[Area("Restaurant")]
	[Authorize(Roles = "RestaurantOwner")]
	public class RestaurantContentManagementController : Controller
	{
		private readonly IRestaurantContentManagementClient _restaurantContentManagement;
		private readonly IRestaurantBannerSettingClient _restaurantBannerSetting;
		private readonly IResturantClient _resturant;
		private readonly IRestaurantBranchClient _restaurantBranch;
		private readonly IMapper _mapper;
		private readonly IUserSessionManager _userSession;
		public RestaurantContentManagementController(IRestaurantContentManagementClient restaurantContentManagement,
			IResturantClient resturant,
			IMapper mapper,
			IRestaurantBranchClient restaurantBranch,
			IUserSessionManager userSession
			, IRestaurantBannerSettingClient restaurantBannerSetting)
		{
			_restaurantContentManagement = restaurantContentManagement;
			_resturant = resturant;
			_mapper = mapper;
			_userSession = userSession;
			_restaurantBranch = restaurantBranch;
			_restaurantBannerSetting = restaurantBannerSetting;
		}
		public async Task<IActionResult> Index()
		{
			var RestaurantId = _userSession.GetUserStore().Id;
			var restaurants = _mapper.Map<RestaurantViewModel>(await _resturant.GetResturantByID(RestaurantId));
			var restaurantBanner = _mapper.Map<IEnumerable<RestaurantBannerSettingViewModel>>(await _restaurantBannerSetting.GetAllRestaurantBannerSettingsAsync(RestaurantId));
			//var restaurantBranch = _mapper.Map<IEnumerable<RestaurantBranchViewModel>>(await _restaurantBranch.GetAllRestaurantBranchsAsync(RestaurantId));
			ViewBag.Restaurant = restaurants;
			ViewBag.RestaurantBanner = restaurantBanner.Where(x => x.BannerType == Enum.GetName(typeof(BannerType), BannerType.MenuBanner)).FirstOrDefault();
			//ViewBag.RestaurantBranch = restaurantBranch.FirstOrDefault();
			RestaurantContentManagementViewModel info = _mapper.Map<RestaurantContentManagementViewModel>(await _restaurantContentManagement.GetRestaurantContentManagementByRestaurantId(RestaurantId));			
			if (info == null)
			{
				RestaurantContentManagementViewModel content = new RestaurantContentManagementViewModel();
				content.restaurant = restaurants;
				return View(content);
			}
			info.restaurant = restaurants;
			return View(info);
		}
		public async Task<IActionResult> Update(long? Id, RestaurantContentManagementViewModel restaurantContentManagementViewModel)
		{
			try
			{
				var RestaurantId = _userSession.GetUserStore().Id;
				restaurantContentManagementViewModel.RestaurantId = RestaurantId;
				if (Id.HasValue && Id > 0)
				{
					RestaurantContentManagementDTO Result = await _restaurantContentManagement.Update(_mapper.Map<RestaurantContentManagementDTO>(restaurantContentManagementViewModel));

					Result.Id = restaurantContentManagementViewModel.Id;
					RestaurantContentManagementViewModel model = _mapper.Map<RestaurantContentManagementViewModel>(Result);
					return Json(new
					{
						success = true,
						message = "Record Updated Successfully",
						data = model
					});
				}
				else
				{
					RestaurantContentManagementDTO Result = await _restaurantContentManagement.Create(_mapper.Map<RestaurantContentManagementDTO>(restaurantContentManagementViewModel));
					RestaurantContentManagementViewModel model = _mapper.Map<RestaurantContentManagementViewModel>(Result);
					return Json(new
					{
						success = true,
						message = "Record Updated Successfully",
						data = model
					});
				}


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

			/*}*/

			return Json(new
			{
				success = false,
				message = "Fill all required fields and submit the form again"
			});
		}
		public async Task<ActionResult> Edit(long id)
		{
			RestaurantViewModel model = _mapper.Map<RestaurantViewModel>(await _resturant.GetResturantByID(id));
			return View(model);
		}
		[HttpPost]
		public async Task<ActionResult> Edit(long id, string DescriptionImage)
		{
			try
			{

				RestaurantViewModel model = _mapper.Map<RestaurantViewModel>(await _resturant.GetResturantByID(id));
				model.Id = _userSession.GetUserStore().Id;
				model.DescriptionImage = DescriptionImage;

				RestaurantDTO result = await _resturant.Edit(_mapper.Map<RestaurantDTO>(model));
				return Json(new
				{
					success = true,
					message = "Banner Details updated successfully!",
					data = new { DescriptionImage = result.DescriptionImage }
				});

			}
			catch (ApiException ex)
			{
				return Json(new
				{
					success = false,
					message = "OOops! Something went wrong. Please try later."
				});
			}
		}

		[HttpPost]
		public async Task<IActionResult> Create(string filePath)
		{
			string message = string.Empty;
			if (filePath != null && filePath.Length > 0)
			{
				try
				{
					RestaurantViewModel objBannerImage = new RestaurantViewModel();
					objBannerImage.DescriptionImage = filePath;
					objBannerImage.Id = _userSession.GetUserStore().Id;

					var resturant = await _resturant.Edit(_mapper.Map<RestaurantDTO>(objBannerImage));

					return Json(new
					{
						success = true,
						message = "About us image updated successfully!",
						filePath = resturant.DescriptionImage
					});
				}
				catch (ApiException ex)
				{
					return Json(new
					{
						success = false,
						message = "Oops! Something Went Wrong",
					});
				}
			}
			return Json(new
			{
				success = false,
				message = "Oops! invalid image",
			});
		}

		[HttpPost]
		public async Task<IActionResult> Footer(string filePath)
		{
			string message = string.Empty;
			if (filePath != null && filePath.Length > 0)
			{
				try
				{
					RestaurantContentManagementViewModel objBannerImage = new();
					objBannerImage.FooterImage = filePath;
					objBannerImage.RestaurantId = _userSession.GetUserStore().Id;

					var restaurantContentManagement = await _restaurantContentManagement.Footer(_mapper.Map<RestaurantContentManagementDTO>(objBannerImage));

					return Json(new
					{
						success = true,
						message = "Footer banner updated successfully!",
						filePath = restaurantContentManagement.FooterImage
					});
				}
				catch (ApiException ex)
				{
					return Json(new
					{
						success = false,
						message = "Oops! Something Went Wrong",
					});
				}
			}
			return Json(new
			{
				success = false,
				message = "Oops! invalid image",
			});
		}

		[HttpPost]
		public async Task<IActionResult> PromotionCreate(string filePath)
		{
			string message = string.Empty;
			if (filePath != null && filePath.Length > 0)
				try
				{
					RestaurantBannerSettingViewModel objBannerImage = new();
					objBannerImage.ImagePath = filePath;
					objBannerImage.RestaurantId = _userSession.GetUserStore().Id;
					objBannerImage.BannerType = Enum.GetName(typeof(BannerType), BannerType.MenuBanner);

					var RestaurantBannerSetting = await _restaurantBannerSetting.UpdateRestaurantBannerSettingAsync(_mapper.Map<RestaurantBannerSettingDTO>(objBannerImage));
					return Json(new
					{
						success = true,
						message = "Menu banner updated successfully!",
						filepath = RestaurantBannerSetting.ImagePath
					});
				}
				catch (ApiException ex)
				{
					return Json(new
					{
						success = false,
						message = "Oops! Something Went Wrong",
					});
				}
			return Json(new
			{
				success = false,
				message = "Oops! Something Went Wrong",

			});
		}

		public async Task<ActionResult> PromoEdit(long id)
		{
			RestaurantBannerSettingViewModel model = _mapper.Map<RestaurantBannerSettingViewModel>(await _restaurantBannerSetting.GetRestaurantBannerSettingByIdAsync(id));
			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> PromoEdit(long id, string ImagePath)
		{
			try
			{

				RestaurantBannerSettingViewModel model = _mapper.Map<RestaurantBannerSettingViewModel>(await _restaurantBannerSetting.GetRestaurantBannerSettingByIdAsync(id));
				model.Id = _userSession.GetUserStore().Id;
				model.ImagePath = ImagePath;
				RestaurantBannerSettingDTO result = await _restaurantBannerSetting.UpdateRestaurantBannerSettingAsync(_mapper.Map<RestaurantBannerSettingDTO>(model));
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
					message = "OOops! Something went wrong. Please try later."
				});
			}
		}
		public async Task<ActionResult> DeleteAboutUs(long Id)
		{

			try
			{

				RestaurantViewModel model = _mapper.Map<RestaurantViewModel>(await _resturant.GetResturantByID(Id));


				RestaurantDTO result = await _resturant.EditAboutUsImage(Id);
				return Json(new
				{
					success = true,
					message = "Banner image updated successfully!",
					data = new { DescriptionImage = result.DescriptionImage }
				});

			}
			catch (ApiException ex)
			{
				return Json(new
				{
					success = false,
					message = "OOops! Something went wrong. Please try later."
				});
			}
		}
		public async Task<ActionResult> DeleteMenuImage(long Id)
		{

			try
			{

				RestaurantBannerSettingViewModel model = _mapper.Map<RestaurantBannerSettingViewModel>(await _restaurantBannerSetting.GetRestaurantBannerSettingByIdAsync(Id));


				RestaurantBannerSettingDTO result = await _restaurantBannerSetting.UpdateRestaurantBannerSettingMenuImage(Id);
				return Json(new
				{
					success = true,
					message = "Banner image updated successfully!",
					data = new { DescriptionImage = result.ImagePath }
				});

			}
			catch (ApiException ex)
			{
				return Json(new
				{
					success = false,
					message = "OOops! Something went wrong. Please try later."
				});
			}
		}

		[HttpPost]
		public async Task<ActionResult> UpdateLogo(long Id, string filePath)
		{
			try
			{

				RestaurantViewModel model = _mapper.Map<RestaurantViewModel>(await _resturant.GetResturantByID(Id));
				model.Id = Id;
				model.Logo = filePath;

				RestaurantDTO result = await _resturant.Edit(_mapper.Map<RestaurantDTO>(model));
				return Json(new
				{
					success = true,
					message = "Logo updated successfully!",
					filepath = result.Logo
				});

			}
			catch (ApiException ex)
			{
				return Json(new
				{
					success = false,
					message = "OOops! Something went wrong. Please try later."
				});
			}
		}

		[HttpPost]
		public async Task<ActionResult> UpdateSecondaryLogo(long Id, string filePath)
		{
			try
			{
				RestaurantViewModel model = _mapper.Map<RestaurantViewModel>(await _resturant.GetResturantByID(Id));
				model.Id = Id;
				model.SecondaryLogo = filePath;

				RestaurantDTO result = await _resturant.Edit(_mapper.Map<RestaurantDTO>(model));
				return Json(new
				{
					success = true,
					message = "Logo updated successfully!",
					filepath = result.Logo
				});

			}
			catch (ApiException ex)
			{
				return Json(new
				{
					success = false,
					message = "OOops! Something went wrong. Please try later."
				});
			}
		}

		[HttpPost]
		public async Task<ActionResult> UpdateThumbnailImage(long Id, string filePath)
		{
			try
			{
				RestaurantViewModel model = _mapper.Map<RestaurantViewModel>(await _resturant.GetResturantByID(Id));
				model.Id = Id;
				model.ThumbnailImage = filePath;

				RestaurantDTO result = await _resturant.Edit(_mapper.Map<RestaurantDTO>(model));
				return Json(new
				{
					success = true,
					message = "Logo updated successfully!",
					filepath = result.Logo
				});

			}
			catch (ApiException ex)
			{
				return Json(new
				{
					success = false,
					message = "OOops! Something went wrong. Please try later."
				});
			}
		}

		[HttpPost]
		public async Task<ActionResult> UpdateFavicon(long Id, string filePath)
		{
			try
			{
				RestaurantViewModel model = _mapper.Map<RestaurantViewModel>(await _resturant.GetResturantByID(Id));
				model.Id = Id;
				model.Favicon = filePath;

				RestaurantDTO result = await _resturant.Edit(_mapper.Map<RestaurantDTO>(model));
				return Json(new
				{
					success = true,
					message = "Favicon updated successfully!",
					filepath = result.Logo
				});

			}
			catch (ApiException ex)
			{
				return Json(new
				{
					success = false,
					message = "OOops! Something went wrong. Please try later."
				});
			}
		}

		[HttpPost]
		public async Task<ActionResult> UpdateColorTheme(long id, string ThemeColor)
		{
			try
			{

				RestaurantViewModel model = _mapper.Map<RestaurantViewModel>(await _resturant.GetResturantByID(id));
				model.Id = _userSession.GetUserStore().Id;
				model.ThemeColor = ThemeColor;

				RestaurantDTO result = await _resturant.Edit(_mapper.Map<RestaurantDTO>(model));
				return Json(new
				{
					success = true,
					message = "Theme color updated successfully!",
					data = new { ThemeColor = result.ThemeColor }
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
