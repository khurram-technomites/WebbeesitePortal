using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.Garage.Filter;
using HelperClasses.DTOs.SparePartsDealer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Garages
{
	[Route("api/Garage/Request")]
	[ApiController]
	[Authorize(Roles = "GarageOwner")]
	public class SparePartRequestController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IRequestService _requestService;
		private readonly ISparePartRequestQuoteService _requestQuoteService;
		private readonly IGarageService _garageService;
		private readonly IFTPUpload _fTPUpload;
		private readonly INumberRangeService _numberRangeService;
		private readonly INotificationService _notificationService;
		private readonly IFCMUserSessionService _fCMUserSession;
		private readonly IIntegrationSettingService _integrationSettingService;
		private readonly ICarMakeService _carMakeService;
		private readonly ICarModelService _carModelService;

		public SparePartRequestController(IMapper mapper, IRequestService requestService, IFTPUpload fTPUpload, IGarageService garageService,
			INumberRangeService numberRangeService, INotificationService notificationService, IFCMUserSessionService fCMUserSession,
			IIntegrationSettingService integrationSettingService, ISparePartRequestQuoteService requestQuoteService,
			ICarMakeService carMakeService, ICarModelService carModelService)
		{
			_mapper = mapper;
			_requestService = requestService;
			_fTPUpload = fTPUpload;
			_garageService = garageService;
			_numberRangeService = numberRangeService;
			_notificationService = notificationService;
			_fCMUserSession = fCMUserSession;
			_integrationSettingService = integrationSettingService;
			_requestQuoteService = requestQuoteService;
			_carMakeService = carMakeService;
			_carModelService = carModelService;
		}

		[HttpPost("AllRequest")]
		public async Task<IActionResult> GetAllRequest(SparePartRequestFilter Fiter)
		{
			string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return Ok(new SuccessResponse<IEnumerable<SparePartRequestDTO>>("", _mapper.Map<IEnumerable<SparePartRequestDTO>>(await _requestService.GetAllRequestsAsync(Fiter, UserId))));
		}
		[HttpPost("Filter/{GarageId}")]
		public async Task<IActionResult> GetAllRequestForFilter(long GarageId, SparePartRequestFilter Fiter)
		{

			return Ok(new SuccessResponse<IEnumerable<SparePartRequestDTO>>("", _mapper.Map<IEnumerable<SparePartRequestDTO>>(await _requestService.GetAllRequestForFilterAsync(Fiter, GarageId))));
		}
		[HttpPost("ActiveRequests/{GarageId}")]
		public async Task<IActionResult> GetAllActiveRequestByGarageId(long GarageId, SparePartRequestFilter Filter)
		{
			var activeRequest = _mapper.Map<IEnumerable<SparePartRequestDTO>>(await _requestService.GetActiveRequestByGarageIdAsync(Filter, GarageId));
			if (activeRequest.Count() == 0)
			{
				return Conflict(new ErrorDetails(409, "No data found.!", string.Empty));

			}
			return Ok(new { Status = "Success", Data = activeRequest });
		}
		[HttpPost("AllRequests/{GarageId}")]
		public async Task<IActionResult> GetAllRequestByGarageId(long GarageId, SparePartRequestFilter Filter)
		{
			var Count = await _requestService.GetTotalCount();
			return Ok(new
			{
				Status = "Success",
				Data = _mapper.Map<IEnumerable<SparePartRequestDTO>>(await _requestService.GetAllRequestByGarageIdAsync(GarageId, Filter)),
				TotalCount = Count
			});
		}
		[HttpGet("By/{Id}")]
		public async Task<IActionResult> GetRequestById(long Id)
		{
			var request = _mapper.Map<IEnumerable<SparePartRequestDTO>>(await _requestService.GetRequestByIdAsync(Id));
			return Ok(new { Status = "success", Data = request });
		}
		[HttpGet("Request/{SparePartRequestQuoteId}")]
		public async Task<IActionResult> GetRequestBySparePartRequestQuoteId(long SparePartRequestQuoteId)
		{
			var Request = _mapper.Map<IEnumerable<SparePartRequestDTO>>(await _requestService.GetRequestBySparePartRequestQuoteId(SparePartRequestQuoteId));
			return Ok(new { Status = "Success", Data = Request });
		}
		[HttpGet("Request/SparePartRequestQuote/{Id}")]
		public async Task<IActionResult> GetSparePartRequestQuoteById(long Id)
		{
			var quoteRequest = _mapper.Map<IEnumerable<SparePartRequestQuoteDTO>>(await _requestQuoteService.GetByIdAsync(Id));
			return Ok(new { Status = "Success", Data = quoteRequest });
		}
		[HttpGet("ViewAll/{Id}")]
		public async Task<IActionResult> ViewAll(long Id)
		{
			var quoteRequest = _mapper.Map<IEnumerable<SparePartRequestQuoteDTO>>(await _requestQuoteService.GetBySparePartRequestIdAsync(Id));
			return Ok(new { Status = "Success", Data = quoteRequest });
		}
		[HttpGet("AllRequests/Count")]
		public async Task<IActionResult> getCount()
		{
			return Ok(new
			{
				Message = "",
				Result = await _requestService.GetTotalCount()
			});
		}
		[HttpPost("Request")]
		public async Task<IActionResult> AddRequest(SparePartRequestDTO Model)
		{
			string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			IEnumerable<Garage> garages = await _garageService.GetGarageByUserAsync(UserId);

			Model.GarageId = garages.FirstOrDefault().Id;
			Model.SequenceNumber = await _numberRangeService.GetNumberRangeByName("SPAREPARTREQUEST");
			Model.PaymentMethod = Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Card);

			string MulkiyaFront = $"/Images/SparePartRequest/{Model.GarageId}/";
			string MulkiyaBack = $"/Images/SparePartRequest/{Model.GarageId}/";
			if (_fTPUpload.MoveFile(Model.MulkiyaImageFront, ref MulkiyaFront))
				Model.MulkiyaImageFront = MulkiyaFront;

			if (_fTPUpload.MoveFile(Model.MulkiyaImageBack, ref MulkiyaBack))
				Model.MulkiyaImageBack = MulkiyaBack;

			foreach (var paths in Model.SparePartRequestImages)
			{
				string Image = $"/Images/SparePartRequestImages/{Model.GarageId}/";

				if (_fTPUpload.MoveFile(paths.Image, ref Image))
					paths.Image = Image;
			}

			Model.Status = Enum.GetName(typeof(SparePartRequestStatus), SparePartRequestStatus.Pending);

			SparePartRequestDTO result = _mapper.Map<SparePartRequestDTO>(await _requestService.AddRequestAsync(_mapper.Map<SparePartRequest>(Model)));

			IEnumerable<string> AvailableDealers = await _requestService.GetSparePartDealersBySparePartRequestId(result.Id);

			if (AvailableDealers.Any())
			{
				List<FCMUserSession> FCMList = new();
				NotificationDTO notification = new()
				{
					OriginatorId = UserId,
					OriginatorName = garages.FirstOrDefault().NameAsPerTradeLicense,
					Description = "New Spare part required. Make a quotation at your earliest.",
					NotificationReceivers = new List<NotificationReceiverDTO>()
					{

					},
					OriginatorType = Enum.GetName(typeof(Logins), Logins.ServiceStaff),
					Url = "/Admin/Garage/Index",
					RecordId = result.Id
				};

				foreach (var Id in AvailableDealers)
				{
					notification.NotificationReceivers.Add(
					new NotificationReceiverDTO
					{
						ReceiverId = Id,
						IsSeen = false,
						IsDelivered = false,
						IsRead = false,
						ReceiverType = Enum.GetName(typeof(Logins), Logins.SparePartDealer),
					});

					IEnumerable<FCMUserSession> List = await _fCMUserSession.GetUserSessionTokensByUser(Id);
					if (FCMList.Count > 0)
					{
						FCMList.AddRange(List);
					}
				}

				await _notificationService.AddNotification(_mapper.Map<Notification>(notification));

				if (FCMList.Any())
				{
					string[] tokens = FCMList.Distinct().Select(x => x.FirebaseToken).ToArray();
					IEnumerable<IntegrationSetting> settings = await _integrationSettingService.GetAllAsync();
					var response = PushNotifications.SendPushNotification(tokens, "Hey! New spare part request recieved", "", new
					{
						result.SequenceNumber,
						result.Id
					}, settings.FirstOrDefault().AutomobileFCMKey, false);
				}
			}

			return Ok(new SuccessResponse<SparePartRequestDTO>("", result));
		}
		[HttpPut("Request")]
		public async Task<IActionResult> UpdateRequest(SparePartRequestDTO Model)
		{
			IEnumerable<SparePartRequest> GarageList = await _requestService.GetRequestByIdAsync(Model.Id);

			SparePartRequest garage = _mapper.Map(Model, GarageList.FirstOrDefault());

			string MulkiyaFront = $"/Images/SparePartRequest/{garage.GarageId}/";
			string MulkiyaBack = $"/Images/SparePartRequest/{garage.GarageId}/";
			IEnumerable<SparePartRequest> List = await _requestService.GetRequestByIdAsync(garage.Id);
			SparePartRequest sparePartRequest = List.FirstOrDefault();

			if (!string.IsNullOrEmpty(garage.MulkiyaImageFront) && garage.MulkiyaImageFront.Contains("/Draft/"))
			{
				if (_fTPUpload.MoveFile(garage.MulkiyaImageFront, ref MulkiyaFront))
					garage.MulkiyaImageFront = MulkiyaFront;
			}

			if (!string.IsNullOrEmpty(garage.MulkiyaImageBack) && garage.MulkiyaImageBack.Contains("/Draft/"))
			{
				if (_fTPUpload.MoveFile(garage.MulkiyaImageBack, ref MulkiyaBack))
					garage.MulkiyaImageBack = MulkiyaBack;
			}

			foreach (var item in garage.SparePartRequestImages)
			{
				string Image = $"/Images/SparePartRequestImages/{garage.GarageId}/";

				if (item.Image.Contains("/Draft/"))
					if (_fTPUpload.MoveFile(item.Image, ref Image))
						item.Image = Image;
			}

			return Ok(_mapper.Map<SparePartRequestDTO>(await _requestService.UpdateRequestAsync(garage)));
		}

		[HttpPut("CancelRequest/{Id}")]
		public async Task<IActionResult> AcceptRequest(long Id)
		{
			IEnumerable<SparePartRequest> GarageList = await _requestService.GetRequestByIdAsync(Id);


			SparePartRequest garage = GarageList.FirstOrDefault();
			garage.Status = Enum.GetName(typeof(StatusforGarageRequest), StatusforGarageRequest.Cancelled);
			if (garage.SparePartRequestQuotes.Count() > 0)
			{
				if (garage.SparePartRequestQuotes.FirstOrDefault().IsAccepted == true)
					garage.SparePartRequestQuotes.FirstOrDefault().IsAccepted = false;

			}
			if (garage.SparePartRequestQuoteId != null)
				garage.SparePartRequestQuoteId = null;
			_mapper.Map<SparePartRequestDTO>(await _requestService.UpdateRequestAsync(garage));
			return Ok(new { Status = "Success", Message = "Request has been cancelled " });


		}

		[HttpGet("UnAuthorized")]
		public async Task<IActionResult> UnAuthorized()
		{
			return Unauthorized();
		}
		[HttpDelete("Image/{Id}")]
		public async Task<IActionResult> Delete(long Id)
		{
			await _requestService.DeleteGarageRequestImage(Id);
			return Ok(new
			{
				Status = "Success"
			});
		}
	}
}