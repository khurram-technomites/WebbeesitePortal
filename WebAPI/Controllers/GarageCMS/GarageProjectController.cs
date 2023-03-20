using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    [Authorize(Roles = "GarageOwner")]
    public class GarageProjectController : ControllerBase
    {
        private readonly IGarageProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly IFTPUpload _fTPUpload;
        public GarageProjectController(IGarageProjectService projectService, IMapper mapper, IFTPUpload fTPUpload)
        {
            _projectService = projectService;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("{GarageId}/Project")]
        public async Task<IActionResult> GetByProject(long GarageId)
        {
            IEnumerable<GarageProjectDTO> result = _mapper.Map<IEnumerable<GarageProjectDTO>>(await _projectService.GetByGarageAsync(GarageId));

            return Ok(result);
        }

        [HttpGet("Count/{GarageId}/Project")]
        public async Task<IActionResult> GetCountByProject(long GarageId)
        {
           var result = _mapper.Map<long>(await _projectService.GetCountByGarageAsync(GarageId));

            return Ok(result);
        }
        [HttpGet("Project/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<GarageProjectDTO> result = _mapper.Map<IEnumerable<GarageProjectDTO>>(await _projectService.GetByIdAsync(Id));

            return Ok(result.FirstOrDefault());
        }

        [HttpPost("Project")]
        public async Task<IActionResult> Post(GarageProjectDTO Model)
        {
            return Ok(_mapper.Map<GarageProjectDTO>(await _projectService.AddProjectAsync(_mapper.Map<GarageProject>(Model))));
        }

        [HttpPut("Project")]
        public async Task<IActionResult> Put(GarageProjectDTO Model)
        {
            //string Path = "/Images/Garage/" + Model.GarageId + "/";

            foreach (var images in Model.GarageProjectImages)
            {
                string imagePath = "/Images/Garage/" + Model.GarageId + "/";
                if (_fTPUpload.MoveFile(images.ImagePath, ref imagePath))
                {
                    images.ImagePath = imagePath;
                }
            }
            return Ok(_mapper.Map<GarageProjectDTO>(await _projectService.UpdateProjectAsync(_mapper.Map<GarageProject>(Model))));
        }

        [HttpDelete("Project/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageProjectDTO>(await _projectService.ArchiveProjectAsync(Id)));
        }

        [HttpPut("Project/{Id}/Status")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<GarageProject> List = await _projectService.GetByIdAsync(Id);

            GarageProject result = List.FirstOrDefault();

            if (result.Status == Enum.GetName(typeof(Status), Status.Active))
                result.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                result.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<GarageProjectDTO>(await _projectService.UpdateProjectAsync(result)));
        }
    }
}
