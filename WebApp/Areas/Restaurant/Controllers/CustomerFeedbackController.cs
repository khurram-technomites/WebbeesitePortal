using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.CustomerFeedback;
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
	public class CustomerFeedbackController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ICustomerFeedbackClient _client;
		private readonly IUserSessionManager _userSession;
		private readonly IRestaurantBranchClient _restaurantBranchClient;

		//[BindProperty]
		//public UserViewModel Model { get; set; }

		public CustomerFeedbackController(IMapper mapper, ICustomerFeedbackClient client , IUserSessionManager userSession, IRestaurantBranchClient restaurantBranchClient)
		{
			_mapper = mapper;
			_client = client;
			_userSession = userSession;
			_restaurantBranchClient = restaurantBranchClient;
		}

		public async Task<IActionResult> Index()
		{
			long restaurantId = _userSession.GetUserStore().Id;
			var Info = _mapper.Map<IEnumerable<CustomerFeedbackViewModel>>(await _client.GetByRestaurantIdAsync(restaurantId));
			return View(Info);
		}

		public async Task<IActionResult> Create()
		{
			long RestaurantId = _userSession.GetUserStore().Id;
			ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CustomerFeedbackViewModel model)
		{
			try
			{
				var session = _userSession.GetUserStore();
				model.Status = Enum.GetName(typeof(Status), Status.Active);
				model.RestaurantId = _userSession.GetUserStore().Id;
				string message = string.Empty;
				if (ModelState.IsValid)
				{
					CustomerFeedbackDTO Result = await _client.AddAsync(_mapper.Map<CustomerFeedbackDTO>(model));
					return Json(new
					{
						success = true,
						url = "/Restaurant/CustomerFeedback/Index",
						message = "Record Added Successfully",
						data = new
						{
							ID = Result.Id,
							Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
							Name = Result.Name,
							BranchName = Result.RestaurantBranch != null ? Result.RestaurantBranch.NameAsPerTradeLicense : "-",
							SurveyName = Result.Survey != null ? Result.Survey.Name : "-",
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
			CustomerFeedbackViewModel CustomerFeedbackDTO = _mapper.Map<CustomerFeedbackViewModel>(await _client.GetByIdAsync(id));
			return View(CustomerFeedbackDTO);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(CustomerFeedbackViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					model.RestaurantId = _userSession.GetUserStore().Id;
					CustomerFeedbackDTO Result = await _client.UpdateAsync(_mapper.Map<CustomerFeedbackDTO>(model));

					Result.Id = model.Id;

					return Json(new
					{
						success = true,
						message = "Record Updated Successfully",
						data = new
						{
							ID = Result.Id,
							Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
							Name = Result.Name,
							BranchName = Result.RestaurantBranch != null ? Result.RestaurantBranch.NameAsPerTradeLicense : "-",
							SurveyName = Result.Survey != null ? Result.Survey.Name : "-",
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
				CustomerFeedbackViewModel Result = _mapper.Map<CustomerFeedbackViewModel>(await _client.ToggleActiveStatus(id));

				return Json(new
				{
					success = true,
					message = "Status Updated Successfully",
					data = new
					{
						ID = Result.Id,
						Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
						Name = Result.Name,
						BranchName = Result.RestaurantBranch != null ? Result.RestaurantBranch.NameAsPerTradeLicense : "-",
						SurveyName = Result.Survey != null ? Result.Survey.Name : "-",
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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(long id)
		{
			try
			{
				await _client.DeleteAsync(id);

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
			return View(_mapper.Map<CustomerFeedbackViewModel>(await _client.GetByIdAsync(id)));
		}

		public async Task<IActionResult> CustomerFeedbackReport()
		{
			PagingParameters pagging = new PagingParameters();
			pagging.PageSize = 10;
			pagging.PageNumber = 1;
			var Info = _mapper.Map<IEnumerable<CustomerFeedbackViewModel>>(await _client.GetByRestaurantIdAsync(_userSession.GetUserStore().Id));
			return new CSVResult<CustomerFeedbackViewModel>(Info, "CustomerFeedback");
		}


	}
}
