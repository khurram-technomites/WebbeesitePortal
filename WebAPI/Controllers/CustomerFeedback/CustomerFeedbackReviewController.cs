using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.CustomerFeedback;
using HelperClasses.DTOs.ServiceStaff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Models;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebAPI.Controllers.CustomerFeedbackReview
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerFeedbackReviewController : ControllerBase
	{
		private readonly ICustomerFeedbackReviewService _customerFeedbackService;
		private readonly IMapper _mapper;

		public CustomerFeedbackReviewController(ICustomerFeedbackReviewService customerFeedbackService, IMapper mapper, IUserService userService)
		{
			_customerFeedbackService = customerFeedbackService;
			_mapper = mapper;
		}

		[HttpPost("GetAll")]
		public async Task<IActionResult> GetAll()
		{
			return Ok(_mapper.Map<IEnumerable<CustomerFeedbackReviewDTO>>(await _customerFeedbackService.GetAllAsync()));
		}

		[HttpPost("GetAll/{Id}")]
		public async Task<IActionResult> GetAllByCustomerFeedback(long Id)
		{
			return Ok(_mapper.Map<IEnumerable<CustomerFeedbackReviewDTO>>(await _customerFeedbackService.GetAllByCustomerFeedbackIdAsync(Id)));
		}

		[HttpGet("{Id}")]
		public async Task<IActionResult> GetById(long Id)
		{
			IEnumerable<CustomerFeedbackReviewDTO> staff = _mapper.Map<IEnumerable<CustomerFeedbackReviewDTO>>(await _customerFeedbackService.GetByIdAsync(Id));
			CustomerFeedbackReviewDTO staffModel = staff.FirstOrDefault();

			return Ok(staffModel);
		}

		[HttpDelete("{Id}")]
		public async Task<IActionResult> Delete(long Id)
		{
			return Ok(_mapper.Map<CustomerFeedbackReviewDTO>(await _customerFeedbackService.ArchiveAsync(Id)));
		}

	}
}
