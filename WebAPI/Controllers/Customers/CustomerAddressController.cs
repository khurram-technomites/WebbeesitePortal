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
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Customers
{
	[Route("api/Customer")]
	[ApiController]
	[Authorize(Roles = "Customer, RestaurantCashierStaff")]
	public class CustomerAddressController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ICustomerAddressService _customerAddressService;
		private readonly ICustomerService _customerService;

		public CustomerAddressController(IMapper mapper, ICustomerAddressService customerAddressService, ICustomerService customerService)
		{
			_mapper = mapper;
			_customerAddressService = customerAddressService;
			_customerService = customerService;
		}

		[HttpGet("{CustomerId}/Address")]
		public async Task<IActionResult> GetByCustomer(long CustomerId, bool recent = false, double Lat = 0, double Lng = 0)
		{
			IEnumerable<CustomerAddressDTO> address;
			if (CustomerId == 0)
			{
				string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				IEnumerable<Customer> customers = await _customerService.GetByUserIdAsync(UserId);

				address = _mapper.Map<IEnumerable<CustomerAddressDTO>>(await _customerAddressService.GetAddressByCustomerAsync(customers.FirstOrDefault().Id));
			}
			else
				address = _mapper.Map<IEnumerable<CustomerAddressDTO>>(await _customerAddressService.GetAddressByCustomerAsync(CustomerId));

			if (recent)
			{
				if (Lat != 0 && Lng != 0)
					foreach (var Address in address)
					{
						Address.Distance = DistanceHelper.DistanceTo(Lat, Lng, (double)Address.Latitude, (double)Address.Longitude);
					}
				return Ok(new SuccessResponse<IEnumerable<CustomerAddressDTO>>("", address.OrderBy(i => i.Distance).ToList()));
			}

			return Ok(new SuccessResponse<IEnumerable<CustomerAddressDTO>>("", address));
		}

		[HttpPost("Address")]
		public async Task<IActionResult> Post(CustomerAddressDTO Model)
		{
			string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			IEnumerable<Customer> customers = await _customerService.GetByUserIdAsync(UserId);
			Model.CustomerId = customers.FirstOrDefault().Id;

			return Ok(new SuccessResponse<CustomerAddressDTO>("", _mapper.Map<CustomerAddressDTO>(await _customerAddressService.AddCustomerAddressAsync(_mapper.Map<CustomerAddress>(Model)))));
		}

		[Authorize(Roles = "RestaurantCashierStaff")]
		[HttpPost("Address/POS")]
		public async Task<IActionResult> PostPOS(CustomerAddressDTO Model)
		{
			IEnumerable<CustomerAddress> addresses = await _customerAddressService.GetAddressByCustomerAsync(Model.CustomerId);

			if (addresses.Count() > 0 && addresses.FirstOrDefault(x => x.Type == Model.Type) != null)
			{
				Model.Id = addresses.FirstOrDefault(x => x.Type == Model.Type).Id;
				return Ok(new SuccessResponse<CustomerAddressDTO>("Data updated successfully", _mapper.Map<CustomerAddressDTO>(await _customerAddressService.UpdateCustomerAddressAsync(_mapper.Map<CustomerAddress>(Model)))));
			}

			return Ok(new SuccessResponse<CustomerAddressDTO>("Data added successfully", _mapper.Map<CustomerAddressDTO>(await _customerAddressService.AddCustomerAddressAsync(_mapper.Map<CustomerAddress>(Model)))));
		}

		[Authorize(Roles = "RestaurantCashierStaff")]
		[HttpGet("Address/Status")]
		public IActionResult GetStatus()
		{
			object status = new
			{
				home = Enum.GetName(typeof(CustomerAddressStatus), CustomerAddressStatus.Home),
				office = Enum.GetName(typeof(CustomerAddressStatus), CustomerAddressStatus.Office),
				other = Enum.GetName(typeof(CustomerAddressStatus), CustomerAddressStatus.Other),
			};

			return Ok(new SuccessResponse<object>("Data received successfully", status));
		}

		[HttpPut("Address")]
		public async Task<IActionResult> Put(CustomerAddressDTO Model)
		{
			return Ok(new SuccessResponse<CustomerAddressDTO>("Data updated successfully", _mapper.Map<CustomerAddressDTO>(await _customerAddressService.UpdateCustomerAddressAsync(_mapper.Map<CustomerAddress>(Model)))));
		}

		[HttpDelete("Address/{Id}")]
		public async Task<IActionResult> Delete(long Id)
		{
			await _customerAddressService.DeleteCustomerAddressAsync(Id);
			return Ok(new SuccessResponse<string>("Address deleted successfully", ""));
		}
	}
}
