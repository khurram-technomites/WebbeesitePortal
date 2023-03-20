using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantCashierStaff;
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
using WebApp.Services.TypedClients;
using WebApp.ViewModels;
using WebApp.ViewModels.Restaurant;

namespace WebApp.Areas.Restaurant.Controllers
{
	[Area("Restaurant")]
	//[Authorize(Roles = "RestaurantOwner")]
	public class RestaurantPrinterSettingController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRestaurantPrinterSettingClient _restaurantPrinterSettingClient;
		private readonly IRestaurantBranchClient _restaurantBranchClient;
		private readonly IUserSessionManager _userSession;

		public RestaurantPrinterSettingController(IMapper mapper, IRestaurantPrinterSettingClient restaurantPrinterSettingClient, IUserSessionManager userSession, IRestaurantBranchClient restaurantBranchClient)
		{
			_mapper = mapper;
			_restaurantPrinterSettingClient = restaurantPrinterSettingClient;
			_userSession = userSession;
			_restaurantBranchClient = restaurantBranchClient;
		}

		public async Task<IActionResult> Index()
		{
			long restaurantId = _userSession.GetUserStore().Id;
			var Info = _mapper.Map<IEnumerable<RestaurantPrinterSettingViewModel>>(await _restaurantPrinterSettingClient.GetAllByRestaurantIdAsync(restaurantId));
			return View(Info);

		}
		public async Task<IActionResult> Create()
		{
			long RestaurantId = _userSession.GetUserStore().Id;
			ViewBag.printerSetting = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(RestaurantPrinterSettingViewModel model)
		{
			try
			{
				var session = _userSession.GetUserStore();
				model.RestaurantId = _userSession.GetUserStore().Id;
				string message = string.Empty;
				if (ModelState.IsValid)
				{
					RestaurantPrinterSettingDTO Result = await _restaurantPrinterSettingClient.AddRestaurantPrinterSettingAsync(_mapper.Map<RestaurantPrinterSettingDTO>(model));
					if (Result != null)
					{
						Result.RestaurantBranch = await _restaurantBranchClient.GetRestaurantBranchByIdAsync(Result.RestaurantBranchId);
					}

					return Json(new
					{
						success = true,
						url = "/Restaurant/RestaurantPrinterSetting/Index",
						message = "Record Added Successfully",
						data = new
						{
							Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
							Id = Result.Id,
							Name = Result.Name,
							IP = Result.IP,
							Type = Result.Type,
							NameAsPerTradeLicense = Result.RestaurantBranch.NameAsPerTradeLicense,
							IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false

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
			long RestaurantId = _userSession.GetUserStore().Id;
			ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
			var Info = _mapper.Map<RestaurantPrinterSettingViewModel>(await _restaurantPrinterSettingClient.GetAllByIdAsync(id));

			return View(Info);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(RestaurantPrinterSettingViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					model.RestaurantId = _userSession.GetUserStore().Id;
					RestaurantPrinterSettingDTO Result = await _restaurantPrinterSettingClient.UpdateRestaurantPrinterSettingAsync(_mapper.Map<RestaurantPrinterSettingDTO>(model));

					if (Result != null)
					{
						Result.RestaurantBranch = await _restaurantBranchClient.GetRestaurantBranchByIdAsync(Result.RestaurantBranchId);
					}
					Result.Id = model.Id;

					return Json(new
					{
						success = true,
						message = "Record Updated Successfully",
						data = new
						{
							Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
							ID = Result.Id,
							Name = Result.Name,
							IP = Result.IP,
							Type = Result.Type,
							NameAsPerTradeLicense = Result.RestaurantBranch.NameAsPerTradeLicense,
							IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false

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
				await _restaurantPrinterSettingClient.DeleteRestaurantPrinterSettingAsync(id);

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

		public async Task<ActionResult> ToggleActiveStatus(long id)
		{
			try
			{
				RestaurantPrinterSettingViewModel Result = _mapper.Map<RestaurantPrinterSettingViewModel>(await _restaurantPrinterSettingClient.ToggleActiveStatus(id));

				return Json(new
				{
					success = true,
					message = "Status Updated Successfully",
					data = new
					{
						Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
						ID = Result.Id,
						Name = Result.Name,
						IP = Result.IP,
						Type = Result.Type,
						NameAsPerTradeLicense = Result.RestaurantBranch.NameAsPerTradeLicense,
						IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false

					}

				}); ;
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
			RestaurantPrinterSettingViewModel printerSetting = _mapper.Map<RestaurantPrinterSettingViewModel>(await _restaurantPrinterSettingClient.GetAllByIdAsync(id));
			ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(printerSetting.RestaurantId);
			return View(printerSetting);
		}

	}

}
