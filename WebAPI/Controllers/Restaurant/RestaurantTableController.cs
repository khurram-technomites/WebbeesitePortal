using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/Restaurant")]
    [ApiController]
    [Authorize(Roles = "Admin , RestaurantOwner , RestaurantCashierStaff")]
    public class RestaurantTableController : ControllerBase
    {
        private readonly IRestaurantTableService _restaurantTableService;
        private readonly IMapper _mapper;
        public RestaurantTableController(IRestaurantTableService restaurantTableService, IMapper mapper)
        {
            _restaurantTableService = restaurantTableService;
            _mapper = mapper;
        }

        #region Status Repsonse Apis

        [Authorize(Roles = "Admin")]
        [HttpGet("Table")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new SuccessResponse<IEnumerable<RestaurantTableDTO>>("Data received successfully", _mapper.Map<IEnumerable<RestaurantTableDTO>>(await _restaurantTableService.GetAllAsync())));
        }

        [HttpGet("Table/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            return Ok(new SuccessResponse<RestaurantTableDTO>("Data by Id received successfully", _mapper.Map<RestaurantTableDTO>(await _restaurantTableService.GetByIdAsync(Id))));
        }

        [HttpGet("GetByRestaurantId/{restaurantId}/Table")]
        public async Task<IActionResult> GetByRestaurantId(long restaurantId)
        {
            return Ok(new SuccessResponse<IEnumerable<RestaurantTableDTO>>("Data by restaurant received successfully", _mapper.Map<IEnumerable<RestaurantTableDTO>>(await _restaurantTableService.GetByRestaurantIdAsync(restaurantId))));
        }

        [HttpGet("GetByRestaurantBranchId/{branchId}/Table")]
        public async Task<IActionResult> GetByBranchId(long branchId)
        {
            return Ok(new SuccessResponse<IEnumerable<RestaurantTableDTO>>("Data by branch received successfully", _mapper.Map<IEnumerable<RestaurantTableDTO>>(await _restaurantTableService.GetByRestaurantBranchIdAsync(branchId))));
        }

        [HttpGet("GetReservedByRestaurantBranchId/{branchId}/Table")]
        public async Task<IActionResult> GetReservedByBranchId(long branchId)
        {
            return Ok(new SuccessResponse<IEnumerable<RestaurantTableDTO>>("Data by branch received successfully", _mapper.Map<IEnumerable<RestaurantTableDTO>>(await _restaurantTableService.GetReservedByRestaurantBranchIdAsync(branchId, Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved)))));
        }

        [HttpPost("Table")]
        public async Task<IActionResult> AddTable(RestaurantTableDTO Model)
        {
            Model.Status = Enum.GetName(typeof(Status), Status.Active);
            return Ok(new SuccessResponse<RestaurantTableDTO>("Data created successfully", _mapper.Map<RestaurantTableDTO>(await _restaurantTableService.AddRestaurantTableAsync(_mapper.Map<RestaurantTable>(Model)))));
        }

        [HttpPut("Table")]
        public async Task<IActionResult> UpdateTable(RestaurantTableDTO Model)
        {
            RestaurantTable restaurantTable = await _restaurantTableService.GetByIdAsync(Model.Id);
            restaurantTable = _mapper.Map(Model, restaurantTable);

            return Ok(new SuccessResponse<RestaurantTableDTO>("Data created successfully", _mapper.Map<RestaurantTableDTO>(await _restaurantTableService.UpdateRestaurantTableAsync(_mapper.Map<RestaurantTable>(restaurantTable)))));
        }

        [HttpGet("Table/{Id}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            RestaurantTable table = await _restaurantTableService.GetByIdAsync(Id);
            /*Active*/
            if (table.ActiveStatus == Enum.GetName(typeof(Status), Status.Active))
                table.ActiveStatus = Enum.GetName(typeof(Status), Status.Inactive);
            else
                table.ActiveStatus = Enum.GetName(typeof(Status), Status.Active);

            return Ok(new SuccessResponse<RestaurantTableDTO>("Data updated successfully", _mapper.Map<RestaurantTableDTO>(await _restaurantTableService.UpdateRestaurantTableAsync(_mapper.Map<RestaurantTable>(table)))));
        }

        [HttpDelete("Table/{Id}")]
        public async Task<IActionResult> DeleteTable(long Id)
        {
            _mapper.Map<RestaurantTableDTO>(await _restaurantTableService.ArchiveRestaurantTableAsync(Id));
            return Ok(new SuccessResponse<RestaurantTableDTO>("Data updated successfully", null));
        }

        #endregion
    }
}
