using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Services.TypedClients;
using WebApp.ViewModels;
using HelperClasses.DTOs.Restaurant;
using WebApp.ErrorHandling;
using Newtonsoft.Json;
using System;
using WebApp.ViewModels.Restaurant;
using HelperClasses.Classes;

namespace WebApp.Areas.Restaurant.Controllers
{
	[Area("Restaurant")]
	public class RestaurantTableController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRestaurantTableClient _restaurantTableClient;
		private readonly IUserSessionManager _userSession;
		private readonly IRestaurantBranchClient _restaurantBranchClient;


		public RestaurantTableController(IMapper mapper, IRestaurantTableClient restaurantTableClient, IUserSessionManager userSession, IRestaurantBranchClient restaurantBranchClient)
		{
			_mapper = mapper;
			_restaurantTableClient = restaurantTableClient;
			_userSession = userSession;
			_restaurantBranchClient = restaurantBranchClient;
		}

		public async Task<IActionResult> Index()
		{
			PagingParameters paging = new PagingParameters();
			paging.PageNumber = 1;
			paging.PageSize = 10;

			long RestaurantId = _userSession.GetUserStore().Id;
			IEnumerable<RestaurantTableViewModel> info = _mapper.Map<IEnumerable<RestaurantTableViewModel>>(await _restaurantTableClient.GetAllByRestaurantIdAsync(RestaurantId));
			return View(info);

			//long restaurantId = _userSession.GetUserStore().Id;
			//var Info = _mapper.Map<IEnumerable<RestaurantPrinterSettingViewModel>>(await _restaurantTableClient.GetAllByRestaurantIdAsync(restaurantId));
			//return View(Info);
		}

		public async Task<IActionResult> Create()
		{
			long RestaurantId = _userSession.GetUserStore().Id;
			ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(RestaurantTableViewModel model)
		{
			try
			{
				var session = _userSession.GetUserStore();
				model.RestaurantId = _userSession.GetUserStore().Id;
				string message = string.Empty;
				if (ModelState.IsValid)
				{
					RestaurantTableDTO Result = _mapper.Map<RestaurantTableDTO>(await _restaurantTableClient.AddRestaurantTableAsync(_mapper.Map<RestaurantTableDTO>(model)));
					if (Result != null)
					{
						Result.RestaurantBranch = await _restaurantBranchClient.GetRestaurantBranchByIdAsync(Result.RestaurantBranchId);
					}
					return Json(new
					{
						success = true,
						url = "/Restaurant/RestaurantTable/Index",
						message = "Record Added Successfully",
						data = new
						{
							ID = Result.Id,
							Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
							Name = Result.Name,
							Serving = Result.Serving,
							Type = Result.RestaurantBranch.NameAsPerTradeLicense,
							IsActive = Result.ActiveStatus == Enum.GetName(typeof(Status), Status.Active) ? true : false
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

			RestaurantTableViewModel RestaurantTable = _mapper.Map<RestaurantTableViewModel>(await _restaurantTableClient.GetAllByIdAsync(id));

			return View(RestaurantTable);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(RestaurantTableViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					model.RestaurantId = _userSession.GetUserStore().Id;

					RestaurantTableDTO Result = _mapper.Map<RestaurantTableDTO>(await _restaurantTableClient.UpdateRestaurantTableAsync(_mapper.Map<RestaurantTableDTO>(model)));
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
							ID = Result.Id,
							Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
							Name = Result.Name,
							Serving = Result.Serving,
							Type = Result.RestaurantBranch.NameAsPerTradeLicense,
							IsActive = Result.ActiveStatus == Enum.GetName(typeof(Status), Status.Active) ? true : false
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
				await _restaurantTableClient.DeleteRestaurantTableAsync(id);

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
				RestaurantTableViewModel Result = _mapper.Map<RestaurantTableViewModel>(await _restaurantTableClient.ToggleActiveStatus(id));
				return Json(new
				{
					success = true,
					message = "Status Updated Successfully",
					data = new
					{
						ID = Result.Id,
						Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
						Name = Result.Name,
						Type = Result.RestaurantBranch.NameAsPerTradeLicense,
						Serving = Result.Serving,
						IsActive = Result.ActiveStatus == Enum.GetName(typeof(Status), Status.Active) ? true : false
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
		public async Task<IActionResult> Details(long id)
		{
			RestaurantTableViewModel restaurantTable = _mapper.Map<RestaurantTableViewModel>(await _restaurantTableClient.GetAllByIdAsync(id));

			return View(restaurantTable);
		}
	}
}
