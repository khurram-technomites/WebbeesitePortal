using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Restaurant Manager")]
    public class RestaurantDocumentController : Controller
    {
        private readonly IRestaurantDocumentClient _client;
        private readonly IMapper _mapper;

        public RestaurantDocumentController(IRestaurantDocumentClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        [HttpGet("ByRestaurant/{RestaurantId}")]
        public async Task<IActionResult> GetByRestaurant(long RestaurantId)
        {
            var restaurants = _mapper.Map<IEnumerable<RestaurantDocumentViewModel>>(await _client.GetDocumentByRestaurant(RestaurantId));
            return Json(new
            {
                success = true,
                data = restaurants
            });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            var restaurants = _mapper.Map<RestaurantDocumentViewModel>(await _client.GetDocumentByID(Id));
            return Json(new
            {
                success = true,
                data = restaurants
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(RestaurantDocumentViewModel Model)
        {
            Model.ExpiryDateTime = Convert.ToDateTime(Model.Date);
            var restaurants = _mapper.Map<RestaurantDocumentViewModel>(await _client.AddDocument(_mapper.Map<RestaurantDocumentDTO>(Model)));
            return Json(new
            {
                success = true,
                message = "Document added successfully ...",
                data = restaurants
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(RestaurantDocumentViewModel Model)
        {
            var restaurants = _mapper.Map<RestaurantDocumentViewModel>(await _client.UpdateDocument(_mapper.Map<RestaurantDocumentDTO>(Model)));
            return Json(new
            {
                success = true,
                message = "Document updated successfully ...",
                data = restaurants
            });
        }

        [HttpGet("Delete/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            await _client.DeleteDocument(Id);
            return Json(new
            {
                success = true,
                message = "Document deleted successfully ..."
            });
        }
    }
}
