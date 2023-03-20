using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketDocumentController : Controller
    {

        private readonly ITicketDocumentService _ticketService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;
        public TicketDocumentController(ITicketDocumentService ticketService, IMapper mapper, IFTPUpload fTPUpload)
        {
            _ticketService = ticketService;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<TicketDocumentDTO>>(await _ticketService.GetAllTicketDocumentsAsync()));
        }
        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            return Ok(_mapper.Map<TicketDocumentDTO>(await _ticketService.GetTicketDocumentByIdAsync(Id)));
        }
        [HttpPost]
        public async Task<IActionResult> Add(TicketDocument Model)
        {
            return Ok(_mapper.Map<TicketDocumentDTO>(await _ticketService.AddTicketDocumentAsync(_mapper.Map<TicketDocument>(Model))));
        }
    }
}
