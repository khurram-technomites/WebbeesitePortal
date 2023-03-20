using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
    public class ClientContentMediaController : Controller
    {
        private readonly IClientContentMediaService _clientContentMediaService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;

        public ClientContentMediaController(IClientContentMediaService clientContentMedia, IMapper mapper, IFTPUpload fTPUpload)
        {
            _clientContentMediaService = clientContentMedia;
            _fTPUpload = fTPUpload;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<ClientContentMediaDTO>>(await _clientContentMediaService.GetAllClientContentMediaAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<ClientContentMediaDTO> List = _mapper.Map<IEnumerable<ClientContentMediaDTO>>(await _clientContentMediaService.GetClientContentMediaByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }
        [HttpGet("ClientId/{ClientId}")]
        public async Task<IActionResult> GetByClientId(long ClientId)
        {
            IEnumerable<ClientContentMediaDTO> List = _mapper.Map<IEnumerable<ClientContentMediaDTO>>(await _clientContentMediaService.GetClientContentMediaByClientIdAsync(ClientId));
            return Ok(List);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClientContentMediaDTO Model)
        {
            return Ok(_mapper.Map<ClientContentMediaDTO>(await _clientContentMediaService.AddClientContentMediaAsync(_mapper.Map<ClientContentMedia>(Model))));
        }
        [HttpPost("Range")]
        public async Task<IActionResult> AddRange(IEnumerable<ClientContentMediaDTO> Model)
        {
                foreach (var content in Model)
                {
                    if (content.DocumentPath is not null && content.DocumentPath.Contains("Draft"))
                    {
                        string LogoPath = "/Content/Garage/" + content.ClientId+ "/";
                        if (_fTPUpload.MoveFile(content.DocumentPath, ref LogoPath))
                        {
                        content.DocumentPath = LogoPath;
                        }
                       
                     }
                 }

            return Ok(_mapper.Map<IEnumerable<ClientContentMediaDTO>>(await _clientContentMediaService.AddClientContentMediaRangeAsync(_mapper.Map<IEnumerable<ClientContentMedia>>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            IEnumerable<ClientContentMediaDTO> List = _mapper.Map<IEnumerable<ClientContentMediaDTO>>(await _clientContentMediaService.GetClientContentMediaByIdAsync(Id));
            await _clientContentMediaService.ArchiveClientContentMediaAsync(Id);
            _fTPUpload.DeleteFile(Uri.UnescapeDataString(List.FirstOrDefault().DocumentPath));
            return Ok();
        }
    }
}
