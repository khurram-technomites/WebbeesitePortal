using AutoMapper;
using HelperClasses.DTOs.Aggregators;
using HelperClasses.DTOs.CardScheme;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;
using WebAPI.Services.Domains;

namespace WebAPI.Controllers.CardSchemes
{
    [Route("api/CardScheme")]
    [ApiController]
    [Authorize(Roles = "Admin , RestaurantOwner , RestaurantCashierStaff")]
    public class CardSchemeController : ControllerBase
    {
        private readonly ICardSchemeService _currencyNoteService;
        private readonly IMapper _mapper;

        public CardSchemeController(ICardSchemeService currencyNoteService, IMapper mapper)
        {
            _currencyNoteService = currencyNoteService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllasync()
        {
            return Ok(new SuccessResponse<IEnumerable<CardSchemeDTO>>("Data received successfully", _mapper.Map<IEnumerable<CardSchemeDTO>>(await _currencyNoteService.GetAllAsync())));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            return Ok(new SuccessResponse<IEnumerable<CardSchemeDTO>>("Data received successfully", _mapper.Map<IEnumerable<CardSchemeDTO>>(await _currencyNoteService.GetByIdAsync(Id))));

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCardScheme(CardSchemeDTO Model)
        {
            return Ok(new SuccessResponse<CardSchemeDTO>("Data added successfully", _mapper.Map<CardSchemeDTO>(await _currencyNoteService.AddCardScheme(_mapper.Map<CardScheme>(Model)))));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateCardScheme(CardSchemeDTO Model)
        {
            IEnumerable<CardScheme> list = await _currencyNoteService.GetByIdAsync(Model.Id);
            CardScheme currencyNoteDTO = list.FirstOrDefault();
            currencyNoteDTO = _mapper.Map(Model, currencyNoteDTO);

            return Ok(new SuccessResponse<CardSchemeDTO>("Data updated successfully", _mapper.Map<CardSchemeDTO>(await _currencyNoteService.UpdateCardScheme(_mapper.Map<CardScheme>(currencyNoteDTO)))));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(new SuccessResponse<CardSchemeDTO>("Data deleted successfully", _mapper.Map<CardSchemeDTO>(await _currencyNoteService.ArchiveCardScheme(Id))));
        }
    }
}
