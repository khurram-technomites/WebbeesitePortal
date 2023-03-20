using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage/Project")]
    [ApiController]
    public class GarageProjectImageController : ControllerBase
    {
        private readonly IGarageProjectImageService _imageService;
        private readonly IGarageService _garageService;
        private readonly IMapper _mapper;
        private readonly IFTPUpload _fTPUpload;
        public GarageProjectImageController(IGarageProjectImageService imageService, IMapper mapper, IFTPUpload fTPUpload, IGarageService garageService)
        {
            _imageService = imageService;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _garageService = garageService;
        }

        [HttpGet("{ProjectId}/Images")]
        public async Task<IActionResult> GetByProjectId(long ProjectId)
        {
            return Ok(_mapper.Map<IEnumerable<GarageProjectImageDTO>>(await _imageService.GetByProjectId(ProjectId)));
        }

        [HttpPost("Image")]
        public async Task<IActionResult> AddImage(GarageProjectImageDTO Model)
        {
            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                string LogoPath = "/Images/GarageProject/" + Model.GarageProjectId + "/";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }
            return Ok(_mapper.Map<GarageProjectImageDTO>(await _imageService.AddImage(_mapper.Map<GarageProjectImages>(Model))));
        }
    }
}
