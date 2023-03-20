using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantServiceStaff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
	[Area("Restaurant")]
	[Authorize(Roles = "RestaurantOwner")]
	public class RestaurantServiceStaffController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRestaurantServiceStaffClient _client;
		private readonly IUserSessionManager _userSession;
		private readonly IRestaurantBranchClient _restaurantBranchClient;

		public RestaurantServiceStaffController(IMapper mapper, IRestaurantServiceStaffClient client, IUserSessionManager userSession, IRestaurantBranchClient restaurantBranchClient)
		{
			_mapper = mapper;
			_client = client;
			_userSession = userSession;
			_restaurantBranchClient = restaurantBranchClient;
		}
		public async Task<IActionResult> Index()
		{
			long restaurantId = _userSession.GetUserStore().Id;
			var Info = _mapper.Map<IEnumerable<RestaurantServiceStaffViewModel>>(await _client.GetRestaurantServiceStaffByRestaurantIdAsync(restaurantId));
			return View(Info);
		}
		public async Task<IActionResult> Details(long Id)
		{
			RestaurantServiceStaffViewModel a = _mapper.Map<RestaurantServiceStaffViewModel>(await _client.GetRestaurantServiceStaffByIdAsync(Id));
			return View(a);
		}
		public async Task<ActionResult> ToggleActiveStatus(long id)
		{
			try
			{
				RestaurantServiceStaffViewModel Result = _mapper.Map<RestaurantServiceStaffViewModel>(await _client.ToggleActiveStatus(id));
				RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _restaurantBranchClient.GetRestaurantBranchByIdAsync(Result.RestaurantBranchId));

				Result.BranchName = branches.NameAsPerTradeLicense;

				return Json(new
				{
					success = true,
					message = "Status Updated Successfully",
					data = new
					{
						ID = Result.Id,
						Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
						Name = Result.FirstName,
						LastName = Result.LastName,
						BranchName = Result.BranchName,
						IsActive = Result.Status
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
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(long id)
		{
			try
			{
				await _client.DeleteRestaurantServiceStaffAsync(id);

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

		public async Task<IActionResult> Create()
		{
			long RestaurantId = _userSession.GetUserStore().Id;
			ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(RestaurantServiceStaffViewModel model)
		{
			try
			{
				string message = string.Empty;
				//model.Status = Enum.GetName(typeof(Status), Status.Active);

				if (ModelState.IsValid)
				{
					long restaurantId = _userSession.GetUserStore().Id;
					RestaurantServiceStaffViewModel viewModel = new RestaurantServiceStaffViewModel();
					RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _restaurantBranchClient.GetRestaurantBranchByIdAsync(model.RestaurantBranchId));

					viewModel.Email = model.Email;
					viewModel.FirstName = model.FirstName;
					viewModel.PhoneNumber = model.PhoneNumber;
					viewModel.LastName = model.LastName;
					viewModel.Logo = model.Logo;
					viewModel.Status = Enum.GetName(typeof(Status), Status.Inactive);
					viewModel.CreationDate = DateTime.UtcNow;
					viewModel.RestaurantId = restaurantId;
					viewModel.RestaurantBranchId = model.RestaurantBranchId;
					viewModel.Password = model.Password;
					RestaurantServiceStaffDTO Result = await _client.AddRestaurantServiceStaffAsync(_mapper.Map<RestaurantServiceStaffDTO>(viewModel));

					Result.BranchName = branches.NameAsPerTradeLicense;
					return Json(new
					{
						success = true,
						url = "/Restaurant/RestaurantServiceStaff/Index",
						message = "Staff successfully created.. !",
						data = new
						{
							ID = Result.Id,
							Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
							Name = Result.FirstName,
							LastName = Result.LastName,
							BranchName = Result.BranchName,
							IsActive = Result.Status
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
				ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
				return Json(new
				{
					success = false,
					message = err.Message
				});
			}
		}

		public async Task<IActionResult> Edit(long Id)
		{
			try
			{
				long RestaurantId = _userSession.GetUserStore().Id;
				ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);

				RestaurantServiceStaffViewModel registerModel = new RestaurantServiceStaffViewModel();
				RestaurantServiceStaffViewModel model = _mapper.Map<RestaurantServiceStaffViewModel>(await _client.GetRestaurantServiceStaffByIdAsync(Id));
				registerModel.Password = model.Password;
				registerModel.Email = model.Email;
				registerModel.PhoneNumber = model.PhoneNumber;
				registerModel.FirstName = model.FirstName;
				registerModel.LastName = model.LastName;
				//registerModel.UserName = model.User.UserName;
				registerModel.Status = model.Status;
				registerModel.Logo = model.Logo;
				registerModel.UserId = model.UserId;

				registerModel.RestaurantBranchId = model.RestaurantBranchId;
				return View(registerModel);
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
		public async Task<IActionResult> Edit(RestaurantServiceStaffViewModel model, long id)
		{
			if (ModelState.IsValid)

			{
				try
				{
					model.RestaurantId = _userSession.GetUserStore().Id;
					RestaurantServiceStaffDTO Result = await _client.UpdateRestaurantServiceStaffAsync(_mapper.Map<RestaurantServiceStaffDTO>(model));
					RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _restaurantBranchClient.GetRestaurantBranchByIdAsync(Result.RestaurantBranchId));

					Result.BranchName = branches.NameAsPerTradeLicense;
					Result.Id = model.Id;

					return Json(new
					{
						success = true,
						message = "Record Updated Successfully",
						data = new
						{
							ID = Result.Id,
							Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
							Name = Result.FirstName,
							LastName = Result.LastName,
							BranchName = Result.BranchName,
							IsActive = Result.Status
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
	}
}
