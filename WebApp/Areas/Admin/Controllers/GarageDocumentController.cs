using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelperClasses.DTOs.Garage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Automobile Manager")]
    public class GarageDocumentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGarageDocumentClient _client;

        public GarageDocumentController(IMapper mapper, IGarageDocumentClient client)
        {
            _mapper = mapper;
            _client = client;
        }

        [HttpPost]
        public async Task<IActionResult> Add(GarageDocumentViewModel Model)
        {
            Model.ExpiryDateTime = Convert.ToDateTime(Model.ExpiryDateTime);
            var garage = _mapper.Map<GarageDocumentViewModel>(await _client.AddGarage(_mapper.Map<GarageDocumentDTO>(Model)));
            return Json(new
            {
                success = true,
                message = "Document added successfully ...",
                data = garage
            });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long Id)
        {
            await _client.Delete(Id);
            return Json(new
            {
                success = true,
                message = "Document deleted successfully ..."
            });
        }
    }
}
