using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly ICustomerSessionService _customerSessionService;
        private readonly IRestaurantCustomerService _restaurantCustomerservice;
        private readonly IMapper _mapper;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;

        public CustomerController(ICustomerService service, IMapper mapper, IFTPUpload fTPUpload, IRestaurantCustomerService restaurantCustomerService, UserManager<AppUser> userManager)
        {
            _restaurantCustomerservice = restaurantCustomerService;
            _service = service;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _userManager = userManager;
        }

        [HttpGet("GetAll/{restaurantId}")]
        public async Task<IActionResult> GetAll(long restaurantId)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantCustomerDTO>>(await _restaurantCustomerservice.GetRestaurantCustomersAsync(restaurantId)));
        }

        [HttpGet("DropDown/{restaurantId}")]
        public async Task<IActionResult> DropDown(long restaurantId)
        {
            return Ok(await _service.GetAllCustomersDropDownByRestaurantIdAsync(restaurantId));
        }

        [HttpGet("DropDownByAdmin/")]
        public async Task<IActionResult> DropDownByAdmin()
        {
            return Ok(await _service.GetAllCustomersDropDownByAdminAsync());
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<CustomerDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<CustomerDTO> List = _mapper.Map<IEnumerable<CustomerDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Add(CustomerDTO Model)
        {
            Model.Status = Enum.GetName(typeof(Status), Status.Active);
            return Ok(_mapper.Map<CustomerDTO>(await _service.AddCustomerAsync(_mapper.Map<Customer>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CustomerDTO Model)
        {

            return Ok(_mapper.Map<CustomerDTO>(await _service.UpdateCustomerAsync(_mapper.Map<Customer>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            CustomerDTO result = _mapper.Map<CustomerDTO>(await _service.ArchiveCustomerAsync(Id));

            AppUser user = await _userManager.FindByIdAsync(result.UserId);
            user.IsDeleted = true;

            await _userManager.UpdateAsync(user);

            return Ok(result);

        }
        [HttpGet("GetAllCustomerSession")]
        public async Task<IActionResult> GetAllCustomerSession()
        {
            return Ok(_mapper.Map<IEnumerable<CustomerSessionDTO>>(await _customerSessionService.GetAll()));
        }
        [HttpGet("GetCustomerSessionById/{Id}")]
        public async Task<IActionResult> GetCustomerSessionById(long Id)
        {
            IEnumerable<CustomerSessionDTO> List = _mapper.Map<IEnumerable<CustomerSessionDTO>>(await _customerSessionService.GetCustomerSessionByID(Id));
            return Ok(List.FirstOrDefault());
        }
        [HttpPost("GetCustomerSessionFirebaseTokens/{Id}/{isPushNotificationAllowed}")]
        public async Task<ActionResult> GetCustomerSessionFirebaseTokens(long Id, bool? isPushNotificationAllowed)
        {
            return Ok(_mapper.Map<IEnumerable<CustomerSessionDTO>>(await _customerSessionService.GetFireBasGetCustomerSessionFirebaseTokens(Id, isPushNotificationAllowed)));

        }

        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<Customer> Customer = await _service.GetByIdAsync(Id);
            Customer make = Customer.FirstOrDefault();

            if (make.Status == Enum.GetName(typeof(Status), Status.Active))
                make.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                make.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _service.UpdateCustomerAsync(make));
        }
    }
}
