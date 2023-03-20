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
    [Authorize(Roles = "Admin,Vendor")]
    public class ClientDomainSuggestionsController : Controller
    {
        private readonly IClientDomainSuggestionsService _clientDomainSuggestionsService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;

        public ClientDomainSuggestionsController(IClientDomainSuggestionsService clientDomainSuggestionsService, IMapper mapper, IFTPUpload fTPUpload)
        {
            _clientDomainSuggestionsService = clientDomainSuggestionsService;
            _fTPUpload = fTPUpload;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<ClientDomainSuggestionsDTO>>(await _clientDomainSuggestionsService.GetAllClientDomainAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<ClientDomainSuggestionsDTO> List = _mapper.Map<IEnumerable<ClientDomainSuggestionsDTO>>(await _clientDomainSuggestionsService.GetClientDomainByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpGet("ClientId/{ClientId}")]
        public async Task<IActionResult> GetByClientId(long ClientId)
        {
            IEnumerable<ClientDomainSuggestionsDTO> List = _mapper.Map<IEnumerable<ClientDomainSuggestionsDTO>>(await _clientDomainSuggestionsService.GetClientDomainByClientIdAsync(ClientId));
            return Ok(List);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClientDomainSuggestionsDTO Model)
        {
            return Ok(_mapper.Map<ClientDomainSuggestionsDTO>(await _clientDomainSuggestionsService.AddClientDomainAsync(_mapper.Map<ClientDomainSuggestions>(Model))));
        }
        [HttpPost("Range")]
        public async Task<IActionResult> AddRange(IEnumerable<ClientDomainSuggestionsDTO> Model)
        {
            return Ok(_mapper.Map<IEnumerable<ClientDomainSuggestionsDTO>>(await _clientDomainSuggestionsService.AddClientDomainRangeAsync(_mapper.Map<IEnumerable<ClientDomainSuggestions>>(Model))));
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            await _clientDomainSuggestionsService.ArchiveClientDomainAsync(Id);
            return Ok();
        }
    }
}
