using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
namespace WebAPI.Controllers.Vendors
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientEmailController : ControllerBase
    {
        private readonly IClientEmailsService _clientEmailsService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;

        public ClientEmailController(IClientEmailsService clientEmailsService, IMapper mapper, IFTPUpload fTPUpload)
        {
            _clientEmailsService = clientEmailsService;
            _fTPUpload = fTPUpload;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<ClientEmailsDTO>>(await _clientEmailsService.GetAllClientEmailsAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<ClientEmailsDTO> List = _mapper.Map<IEnumerable<ClientEmailsDTO>>(await _clientEmailsService.GetClientEmailsByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }
        [HttpGet("ClientId/{ClientId}")]
        public async Task<IActionResult> GetByClientId(long ClientId)
        {
            IEnumerable<ClientEmailsDTO> List = _mapper.Map<IEnumerable<ClientEmailsDTO>>(await _clientEmailsService.GetClientEmailsByClientIdAsync(ClientId));
            return Ok(List);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClientEmailsDTO Model)
        {
            return Ok(_mapper.Map<ClientEmailsDTO>(await _clientEmailsService.AddClientEmailsAsync(_mapper.Map<ClientEmails>(Model))));
        }
        [HttpPost("Range")]
        public async Task<IActionResult> AddRange(IEnumerable<ClientEmailsDTO> Model)
        {
            return Ok(_mapper.Map<IEnumerable<ClientEmailsDTO>>(await _clientEmailsService.AddClientEmailsRangeAsync(_mapper.Map<IEnumerable<ClientEmails>>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            await _clientEmailsService.ArchiveClientEmailsAsync(Id);
            return Ok();
        }
    }
}
