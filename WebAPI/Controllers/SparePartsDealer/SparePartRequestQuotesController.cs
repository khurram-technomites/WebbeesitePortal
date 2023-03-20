using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.Garage.Filter;
using HelperClasses.DTOs.SparePartsDealer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.SparePartsDealer
{
    [Route("api/SparePartDealer")]
    [ApiController]
    [Authorize(Roles = "SparePartDealer,GarageOwner")]
    public class SparePartRequestQuotesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISparePartRequestQuoteService _service;
        private readonly IRequestService _requestService;
        private readonly ISparePartTransactionHistoryService _transactionService;

        public SparePartRequestQuotesController(ISparePartRequestQuoteService service, IMapper mapper,
            ISparePartTransactionHistoryService transactionService , IRequestService requestService)
        {
            _service = service;
            _mapper = mapper;
            _transactionService = transactionService;
            _requestService = requestService;
        }

        [HttpPost("Quotes/Fetch")]
        public async Task<IActionResult> Fetch(SparePartQuoteFilter Filter)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Quotes = new SuccessResponse<IEnumerable<SparePartsAvailableRequestDTO>>("", _mapper.Map<IEnumerable<SparePartsAvailableRequestDTO>>(await _service.GetAllByUserAndFilterAsync(UserId, Filter)));
            return Ok(new { Status = "Success" , Data = Quotes});
        }
        [HttpGet("Quote/ByRequest/{SparePartRequestId}")]
        public async Task<IActionResult> GetBySparePartRequestId(long SparePartRequestId)
        {
            var QuotesByRequest = _mapper.Map<IEnumerable<SparePartRequestQuoteDTO>>(await _service.GetBySparePartRequestIdAsync(SparePartRequestId));
            return Ok(new { Status = "Success" , Data = QuotesByRequest });
        }
        [HttpGet("Quote/{SparePartDealerId}/Pending")]
        public async Task<IActionResult> GetPendingQuotesBySparePartRequestId(long SparePartDealerId)
        {
            var PendingQuotes = _mapper.Map<IEnumerable<SparePartRequestQuoteDTO>>(await _service.GetPendingQuotesBySparePartRequestIdAsync(SparePartDealerId));
            return Ok(new { Status= "Success" , Data = PendingQuotes});
        }
        [HttpGet("AvailableQuote/{SparePartRequestId}")]
        public async Task<IActionResult> GetQuotesBySparePartRequestId(long SparePartRequestId)
        {
            var PendingQuotes = _mapper.Map<IEnumerable<SparePartRequestQuoteDTO>>(await _service.GetQuotesBySparePartRequestIdAsync(SparePartRequestId));
            return Ok(new { Status = "Success", Data = PendingQuotes });
        }
        [HttpGet("AllQuotes/Count")]
        public async Task<IActionResult> getCount()
        {
            return Ok(new
            {
                Message = "",
                Result = await _requestService.GetTotalCount()
            });
        }
        
        [HttpGet("Quote/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            var Quote = _mapper.Map<IEnumerable<SparePartRequestQuoteDTO>>(await _service.GetByIdAsync(Id));
            return Ok(new { Status = "Success" , Data = Quote});
        }
        [HttpGet("Quotes/By/{SparePartsDealerId}")]
        public async Task<IActionResult> GetBySparePartsDealerId(long SparePartsDealerId)
        {
            var QuoteBySparePartDealer = new SuccessResponse<IEnumerable<SparePartRequestQuoteDTO>>("", _mapper.Map<IEnumerable<SparePartRequestQuoteDTO>>(await _service.GetBySparePartsDealerIdAsync(SparePartsDealerId)));
            //var QuoteBySparePartDealer = _mapper.Map<IEnumerable<SparePartRequestQuoteDTO>>(await _service.GetBySparePartsDealerIdAsync(SparePartsDealerId));
            return Ok(QuoteBySparePartDealer);
        }
        [HttpPost("Quotes/Filter/{SparePartsDealerId}")]
        public async Task<IActionResult> GetFilterById(long SparePartsDealerId, SparePartQuoteFilter Filter)
        {
            var Quotes = new SuccessResponse<IEnumerable<SparePartRequestQuoteDTO>>("", _mapper.Map<IEnumerable<SparePartRequestQuoteDTO>>(await _service.GetAllQuoteForFilterAsync(SparePartsDealerId, Filter)));
            //var Quotes = _mapper.Map<IEnumerable<SparePartRequestQuoteDTO>>(await _service.GetAllQuoteForFilterAsync(SparePartsDealerId, Filter));
            return Ok(Quotes);
        }
        [HttpPut("Quote/Accept/{Id}")]
        public async Task<IActionResult> AcceptQuote(long Id)
        {
            IEnumerable<SparePartRequestQuote> GarageList = await _service.GetByIdAsync(Id);
            SparePartRequestQuote garage = GarageList.FirstOrDefault();
            IEnumerable<SparePartRequest> requests = await _requestService.GetRequestByIdAsync(garage.SparePartRequestId);
            SparePartRequest request = requests.FirstOrDefault();
            request.Status = Enum.GetName(typeof(SparePartRequestStatus), SparePartRequestStatus.Active);
            _mapper.Map<SparePartRequestDTO>(await _requestService.UpdateRequestAsync(request));
            if (garage.IsAccepted == false)
            {
                garage.IsAccepted = true;
            }
            var quote = _mapper.Map<SparePartRequestQuoteDTO>(await _service.UpdateSparePartRequestQuoteAsync(garage));
            return Ok(new { Status = "Success", Data = quote });
        }
        [HttpGet("Quote/MyWallet/{SparePartDealerId}")]
        public async Task<IActionResult> MyWallet(long SparePartDealerId)
        {
            var Quote = _mapper.Map<IEnumerable<SparePartTransactionHistoryDTO>>(await _transactionService.MyWallet(SparePartDealerId));
            var walletSum = _mapper.Map<decimal>(await _transactionService.getWallet(SparePartDealerId));
            return Ok(new { Status = "Success", Data = Quote , Wallet = walletSum.ToString("F", CultureInfo.InvariantCulture) });
        }
        [HttpGet("Quote/MyWallet/GetAll")]
        public async Task<IActionResult> GetAllWallet()
        {
            var Quote = _mapper.Map<IEnumerable<SparePartTransactionHistoryDTO>>(await _transactionService.GetAllTransactionHistory());
            return Ok(new { Status = "Success", Data = Quote });
        }
        [HttpPost("Quotes/MyWallet/Filter/{SparePartDealerId}")]
        public async Task<IActionResult> MyWalletFilter(long SparePartDealerId ,SparePartQuoteWalletFilter Filter)
        {
            //DateTime start = DateTime.Parse(StartDate);
            //DateTime end = DateTime.Parse(EndDate);
            var Quotes = new SuccessResponse<IEnumerable<SparePartTransactionHistoryDTO>>("", _mapper.Map<IEnumerable<SparePartTransactionHistoryDTO>>(await _transactionService.GetAllQuoteForFilterAsync(SparePartDealerId, Filter)));
           decimal walletSum = _mapper.Map<decimal>(await _transactionService.getWallet(SparePartDealerId));
            return Ok(new { Quotes, Wallet = walletSum });
        }
    }
}
