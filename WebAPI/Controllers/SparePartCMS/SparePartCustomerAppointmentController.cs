using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Interfaces.IServices;
using WebAPI.Models;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelperClasses.DTOs.SparePartCMS;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartCustomerAppointmentController : ControllerBase
    {
        private readonly ISparePartCustomerAppointmentService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;
        public SparePartCustomerAppointmentController(ISparePartCustomerAppointmentService service, IMapper mapper, IUserService userService, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
            _userManager = userManager;
        }

        [HttpGet("CustomerAppointment")]
        public async Task<IActionResult> GetAllGarageContentManagement()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartCustomerAppointmentDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("CustomerAppointment/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartCustomerAppointmentDTO>>(await _service.GetSparePartCustomerAppointByIdAsync(Id)));
        }

        [HttpGet("{Id}/CustomerAppointment")]
        public async Task<IActionResult> GetAllBySparePartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartCustomerAppointmentDTO>>(await _service.GetSparePartCustomerAppointBySparePartDealerIdAsync(Id)));
        }

        [HttpPost("CustomerAppointment")]
        public async Task<IActionResult> Add(GarageCustomerAppointmentDTO Model)
        {

            return Ok(_mapper.Map<SparePartCustomerAppointmentDTO>(await _service.AddSparePartCustomerAppointAsync(_mapper.Map<SparePartCustomerAppointment>(Model))));
        }

        [HttpPut("CustomerAppointment")]
        public async Task<IActionResult> Update(SparePartCustomerAppointmentDTO Model)
        {
            return Ok(_mapper.Map<SparePartCustomerAppointmentDTO>(await _service.UpdateSparePartCustomerAppointAsync(_mapper.Map<SparePartCustomerAppointment>(Model))));
        }

        [HttpDelete("CustomerAppointment/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartCustomerAppointmentDTO>(await _service.ArchiveSparePartCustomerAppointAsync(Id)));
        }
    }
}
