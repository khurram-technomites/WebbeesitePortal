using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SubscriberController : ControllerBase
    {
        private readonly ISubscribeService _subService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        public SubscriberController(ISubscribeService subService, IMapper mapper, IFTPUpload fTPUpload, IEmailService emailService)
        {
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _subService = subService;
            _emailService = emailService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<SubscriberDTO>>(await _subService.GetAllSubscribersAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            IEnumerable<SubscriberDTO> cities = _mapper.Map<IEnumerable<SubscriberDTO>>(await _subService.GetSubscriberByIdAsync(Id));
            return Ok(cities.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Post(SubscriberDTO Model)
        {

            return Ok(_mapper.Map<CityDTO>(await _subService.AddSubscriberAsync(_mapper.Map<Subscriber>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Put(SubscriberDTO Model)
        {

            IEnumerable<Subscriber> List = await _subService.GetSubscriberByIdAsync(Model.Id);
            Subscriber subs = List.FirstOrDefault();
            subs = _mapper.Map(Model, subs);

            return Ok(_mapper.Map<CityDTO>(await _subService.UpdateSubscriberAsync(_mapper.Map<Subscriber>(subs))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            _mapper.Map<SubscriberDTO>(await _subService.ArchiveSubscriberAsync(Id));
            return Ok();
        }
        [HttpGet("DateWise")]
        public async Task<IActionResult> GetsubscribersDateWise(DateTime FromDate, DateTime ToDate)
        {
            
            return Ok(_mapper.Map<IEnumerable<SubscriberDTO>>(await _subService.GetFilteredSubscribers(FromDate, ToDate)));
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(SubscriberDTO Model)
        {
            IEnumerable<Subscriber> List = await _subService.GetAllSubscribersAsync();

            if (!List.Any())
                return Conflict();

            string email = List.FirstOrDefault().Email;

            await _emailService.SendSubscriberEmail(Model.Email, email, Model.Message);

            return Ok(new SuccessResponse<string>("Email sent successfully", null));
        }

    }
}
