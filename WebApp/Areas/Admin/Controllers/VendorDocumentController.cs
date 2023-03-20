﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;
using System;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Automobile Manager")]
    public class VendorDocumentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVendorDocumentClient _client;

        public VendorDocumentController(IMapper mapper, IVendorDocumentClient client)
        {
            _mapper = mapper;
            _client = client;
        }

        [HttpPost]
        public async Task<IActionResult> Create(VendorDocumentViewModel Model)
        {
            Model.ExpiryDateTime = Convert.ToDateTime(Model.ExpiryDateTime);
            var garage = _mapper.Map<VendorDocumentViewModel>(await _client.AddVendorDocument(_mapper.Map<VendorDocumentDTO>(Model)));
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
