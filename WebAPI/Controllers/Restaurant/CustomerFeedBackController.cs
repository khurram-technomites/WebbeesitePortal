using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Interfaces.IServices;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelperClasses.DTOs.CustomerFeedback;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/Customer/FeedBack")]
    [ApiController]
    public class CustomerFeedBackController : ControllerBase
    {
        private readonly ICustomerFeedbackService _service;
        private readonly IMapper _mapper;
        public CustomerFeedBackController(ICustomerFeedbackService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        //[HttpGet("By/{restaurantId}")]
        //public async Task<IActionResult> GetAllGeneralItem(long restaurantId)
        //{
        //    return Ok(_mapper.Map<IEnumerable<CustomerFeedbackDTO>>(await _service.GetAllCustomerFeedbackAsync(restaurantId)));
        //}
    }
}
