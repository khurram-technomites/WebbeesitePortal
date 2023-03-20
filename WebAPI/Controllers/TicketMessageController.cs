using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Http;
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
    public class TicketMessageController : Controller
    {
        private readonly ITicketMessageService _ticketService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;
        public TicketMessageController(ITicketMessageService ticketService, IMapper mapper, IFTPUpload fTPUpload)
        {
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _ticketService = ticketService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<TicketMessagesDTO>>(await _ticketService.GetAllTicketMessagesAsync()));
        }
        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            return Ok(_mapper.Map<TicketMessagesDTO>(await _ticketService.GetTicketMessagesByIdAsync(Id)));
        }
        [HttpGet("TicketDocument/{Id}")]
        public async Task<IActionResult> GetByTicketDocumentId(long Id)
        {
            return Ok(_mapper.Map<TicketMessagesDTO>(await _ticketService.GetTicketMessagesByTicketDocumentIdAsync(Id)));
        }
        [HttpGet("Sender/{Id}")]
        public async Task<IActionResult> GetBySenderId(string Id)
        {
            return Ok(_mapper.Map<TicketMessagesDTO>(await _ticketService.GetTicketMessagesBySenderIdAsync(Id)));
        }
        [HttpGet("TicketConversation/{Id}")]
        public async Task<IActionResult> GetTicketConversation(long Id)
        {
            var conversation = _mapper.Map<IEnumerable<TicketMessagesDTO>>(await _ticketService.GetTicketMessagesByTicketIdAsync(Id));
            
            return Ok(conversation);
        }
        [HttpPost]
        public async Task<IActionResult> Add(TicketMessages Model)
        {
            string LogoPath = "/Images/TicketDocuments/";
            if (Model.TicketDocument != null)
            {
            if (_fTPUpload.MoveFile(Model.TicketDocument.URL, ref LogoPath))
            {
                Model.TicketDocument.URL = LogoPath;
            }
            }
            return Ok(_mapper.Map<TicketMessagesDTO>(await _ticketService.AddTicketMessagestAsync(_mapper.Map<TicketMessages>(Model))));
        }
        [HttpPut]
        public async Task<IActionResult> Put(TicketMessages Model)
        {

            return Ok(_mapper.Map<TicketMessagesDTO>(await _ticketService.UpdateTicketMessages(_mapper.Map<TicketMessages>(Model))));
        }

    }
}
