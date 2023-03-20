using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.Garage.Filter;
using HelperClasses.DTOs.SparePartsDealer;
using Microsoft.AspNetCore.Authorization;
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

namespace WebAPI.Controllers.SparePartsDealer
{
    [Route("api/SparePartDealer")]
    [ApiController]
    [Authorize]
    public class SparePartRequestAndQuotesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISparePartRequestQuoteService _service;
        private readonly IRequestService _requestService;
        private readonly ISparePartsDealerService _sparePartsDealerService;
        private readonly IFTPUpload _fTPUpload;
        private readonly INotificationService _notificationService;
        private readonly IFCMUserSessionService _fCMUserSession;
        private readonly IIntegrationSettingService _integrationSettingService;

        public SparePartRequestAndQuotesController(ISparePartRequestQuoteService service, IMapper mapper,
            IRequestService requestService, ISparePartsDealerService sparePartsDealerService, IFTPUpload fTPUpload,
            INotificationService notificationService, IFCMUserSessionService fCMUserSession, IIntegrationSettingService integrationSettingService)
        {
            _service = service;
            _mapper = mapper;
            _requestService = requestService;
            _sparePartsDealerService = sparePartsDealerService;
            _fTPUpload = fTPUpload;
            _notificationService = notificationService;
            _fCMUserSession = fCMUserSession;
            _integrationSettingService = integrationSettingService;
        }

        [HttpPost("Requests/Fetch")]
        public async Task<IActionResult> AvailableRequests(SparePartQuoteFilter Filter)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var request = new SuccessResponse<IEnumerable<SparePartsAvailableRequestDTO>>("", _mapper.Map<IEnumerable<SparePartsAvailableRequestDTO>>(await _service.GetAllByUserAndFilterAsync(UserId, Filter)));

            return Ok(request);
        }

        [HttpGet("Requests/{Id}")]
        public async Task<IActionResult> GetRequestById(long Id)
        {
            IEnumerable<SparePartRequestDTO> result = _mapper.Map<IEnumerable<SparePartRequestDTO>>(await _requestService.GetRequestByIdAsync(Id));

            return Ok(new SuccessResponse<SparePartRequestDTO>("", result.FirstOrDefault()));
        }

        [HttpPost("Quote")]
        public async Task<IActionResult> PostQuote(SparePartRequestQuoteDTO Model)
        {
            var Request = _mapper.Map<IEnumerable<SparePartRequestDTO>>(await _requestService.GetRequestByIdAsync(Model.SparePartRequestId));
            if (Request.FirstOrDefault().Status == "Cancelled")
            {
                return Ok(new ErrorDetails(409, "Request has been cancelled", ""));
            }
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Models.SparePartsDealer> Dealer = await _sparePartsDealerService.GetSparePartsDealerByUserAsync(UserId);

            Model.SparePartsDealerId = Dealer.FirstOrDefault().Id;
            Model.FougitoCommision = 0M;
            Model.DeliveryCharges = 0M;
            Model.TotalPrice = 0M;
            Model.Status = "Pending";
            IEnumerable<SparePartRequestQuote> quotecheck = await _service.GetBySparePartsDealerIdAsync(Dealer.FirstOrDefault().Id);
            foreach (var item in quotecheck)
            {
                if (item.SparePartRequestId == Model.SparePartRequestId)
                {
                    return Ok(new ErrorDetails(409, "You have already send quote for this request", ""));
                }
                
            }
            foreach (var paths in Model.SparePartRequestQuoteImages)
            {
                string Image = $"/Images/SparePartRequestQuotes/{Model.SparePartRequestId}/";

                if (_fTPUpload.MoveFile(paths.Image, ref Image))
                    paths.Image = Image;
            }
            //if (result.SparePartsDealerId == Model.SparePartsDealerId)
            //{
            //    return Ok(new ErrorDetails(409, "You have already send quote for this request", ""));
            //}
            SparePartRequestQuoteDTO result = _mapper.Map<SparePartRequestQuoteDTO>(await _service.AddSparePartRequestQuoteAsync(_mapper.Map<SparePartRequestQuote>(Model)));
          

            IEnumerable<SparePartRequest> SPrequest = await _requestService.GetRequestByIdAsync(result.SparePartRequestId);

            if (SPrequest.Any())
            {
                IEnumerable<FCMUserSession> FCMList = await _fCMUserSession.GetUserSessionTokensByUser(SPrequest.FirstOrDefault().Garage.UserId);
                NotificationDTO notification = new()
                {
                    OriginatorId = UserId,
                    OriginatorName = Dealer.FirstOrDefault().NameAsPerTradeLicense,
                    Description = "New quote recieved for you spare part request.",
                    NotificationReceivers = new List<NotificationReceiverDTO>()
                    {
                        new NotificationReceiverDTO
                        {
                            ReceiverId = SPrequest.FirstOrDefault().Garage.UserId,
                            IsSeen = false,
                            IsDelivered = false,
                            IsRead = false,
                            ReceiverType = Enum.GetName(typeof(Logins), Logins.Garage),
                        }
                    },
                    OriginatorType = Enum.GetName(typeof(Logins), Logins.SparePartDealer),
                    RecordId = result.Id
                };

                await _notificationService.AddNotification(_mapper.Map<Notification>(notification));

                if (FCMList.Any())
                {
                    string[] tokens = FCMList.Select(x => x.FirebaseToken).ToArray();
                    IEnumerable<IntegrationSetting> settings = await _integrationSettingService.GetAllAsync();
                    var response = PushNotifications.SendPushNotification(tokens, "Hey! New spare part request recieved", "", new
                    {
                        result.Id
                    }, settings.FirstOrDefault().PartnerFCMKey, false);
                }
            }

            return Ok(new SuccessResponse<SparePartRequestQuoteDTO>("", result));
        }

        [HttpDelete("Quote/{Id}")]
        public async Task<IActionResult> CancelQuote(long Id)
        {
            IEnumerable<SparePartRequestQuote> SparePartRequestQuotes = await _service.GetByIdAsync(Id);

            if (SparePartRequestQuotes.Any())
            {
                SparePartRequestQuote partRequestQuote = SparePartRequestQuotes.FirstOrDefault();

                if (!partRequestQuote.IsAccepted)
                {
                    await _service.ArchiveSparePartRequestQuoteAsync(Id);

                    return Ok(new SuccessResponse<string>("Quote cancelled successfully", ""));
                }
                else
                    return Conflict(new ErrorDetails(409, "Cannot cancel the quote because it is already accepted", ""));
            }
            else
                return Conflict(new ErrorDetails(409, "Invalid Id", ""));
        }
    }
}
