using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.DeliveryStaff;
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

namespace WebAPI.Controllers.DeliveryStaff
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryStaffController : ControllerBase
    {
        private readonly IDeliveryStaffService _deliveryStaffService;
        private readonly IMapper _mapper;

        public DeliveryStaffController(IDeliveryStaffService deliveryStaffService, IMapper mapper, IUserService userService)
        {
            _deliveryStaffService = deliveryStaffService;
            _mapper = mapper;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(PagingParameters Paging)
        {
            return Ok(_mapper.Map<IEnumerable<DeliveryStaffDTO>>(await _deliveryStaffService.GetAllDeliveryStaffsAsync(Paging)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
        
            IEnumerable<DeliveryStaffDTO> staff = _mapper.Map<IEnumerable<DeliveryStaffDTO>>(await _deliveryStaffService.GetDeliveryStaffByIdAsync(Id));
            DeliveryStaffDTO staffModel = staff.FirstOrDefault();
          
            return Ok(staffModel);
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<DeliveryStaffDTO>(await _deliveryStaffService.ArchiveDeliveryStaffAsync(Id)));
        }
        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<Models.DeliveryStaff> cityList = await _deliveryStaffService.GetDeliveryStaffByIdAsync(Id);
            Models.DeliveryStaff city = cityList.FirstOrDefault();

            if (city.Status == Enum.GetName(typeof(Status), Status.Active))
                city.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                city.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<Models.DeliveryStaff>(await _deliveryStaffService.UpdateDeliveryStaffAsync(city)));
        }
    }
}
