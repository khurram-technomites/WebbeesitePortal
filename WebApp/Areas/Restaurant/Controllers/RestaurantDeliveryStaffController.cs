using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
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
	public class RestaurantDeliveryStaffController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IRestaurantDeliveryStaffClient _client;
		private readonly IUserSessionManager _userSession;
		private readonly IRestaurantBranchClient _restaurantBranchClient;

		//[BindProperty]
		//public UserViewModel Model { get; set; }

		public RestaurantDeliveryStaffController(IMapper mapper, IRestaurantDeliveryStaffClient client, IUserSessionManager userSession, IRestaurantBranchClient restaurantBranchClient)
		{
			_mapper = mapper;
			_client = client;
			_userSession = userSession;
			_restaurantBranchClient = restaurantBranchClient;
		}

		public async Task<IActionResult> Index()
		{
			long restaurantId = _userSession.GetUserStore().Id;
			var Info = _mapper.Map<IEnumerable<RestaurantDeliveryStaffViewModel>>(await _client.GetAllRestaurantDeliveryStaffsAsync(restaurantId));
			return View(Info);
		}

		public async Task<IActionResult> Create()
		{
			long RestaurantId = _userSession.GetUserStore().Id;
			ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(RestaurantDeliveryStaffViewModel model)
		{
			try
			{
				var session = _userSession.GetUserStore();
				model.Status = Enum.GetName(typeof(Status), Status.Active);
				model.RestaurantId = _userSession.GetUserStore().Id;
				string message = string.Empty;
				if (ModelState.IsValid)
				{
					RestaurantDeliveryStaffDTO Result = await _client.AddRestaurantDeliveryStaffAsync(_mapper.Map<RestaurantDeliveryStaffDTO>(model));
                    RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _restaurantBranchClient.GetRestaurantBranchByIdAsync(model.RestaurantBranchId));

                    Result.BranchName = branches.NameAsPerTradeLicense;
                    return Json(new
					{
						success = true,
						url = "/Restaurant/RestaurantDeliveryStaff/Index",
						message = "Record Added Successfully",
						data = new
						{
                            ID = Result.Id,
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.FirstName,
                            LastName = Result.LastName,
                            BranchName = Result.BranchName,
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
			RestaurantDeliveryStaffViewModel RestaurantDeliveryStaffDTO = _mapper.Map<RestaurantDeliveryStaffViewModel>(await _client.GetRestaurantDeliveryStaffByIdAsync(id));
			return View(RestaurantDeliveryStaffDTO);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(RestaurantDeliveryStaffViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					model.RestaurantId = _userSession.GetUserStore().Id;
					RestaurantDeliveryStaffDTO Result = await _client.UpdateRestaurantDeliveryStaffAsync(_mapper.Map<RestaurantDeliveryStaffDTO>(model));
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

		public async Task<ActionResult> ToggleActiveStatus(long id)
		{
			try
			{
				RestaurantDeliveryStaffViewModel Result = _mapper.Map<RestaurantDeliveryStaffViewModel>(await _client.ToggleActiveStatus(id));
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
						//Parent = Parent != null ? (Parent.CategoryName) : "",
						//IsParentCategoryDeleted = category.IsParentCategoryDeleted.HasValue ? category.IsParentCategoryDeleted.Value.ToString() : bool.FalseString,
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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(long id)
		{
			try
			{
				await _client.DeleteRestaurantDeliveryStaffAsync(id);

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
			return View(_mapper.Map<RestaurantDeliveryStaffViewModel>(await _client.GetRestaurantDeliveryStaffByIdAsync(id)));
		}

		public async Task<IActionResult> RestaurantDeliveryStaffReport()
		{
			PagingParameters pagging = new PagingParameters();
			pagging.PageSize = 10;
			pagging.PageNumber = 1;
			var Info = _mapper.Map<IEnumerable<RestaurantDeliveryStaffViewModel>>(await _client.GetAllRestaurantDeliveryStaffsAsync(_userSession.GetUserStore().Id));
			return new CSVResult<RestaurantDeliveryStaffViewModel>(Info, "RestaurantDeliveryStaff");
		}


	}
}
