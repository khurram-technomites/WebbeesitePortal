using AutoMapper;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using HelperClasses.DTOs.GarageCMS;
using System.Threading.Tasks;
using WebApp.ViewModels;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Org.BouncyCastle.Asn1.X500;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class ProjectImageController : Controller
    {
        private readonly IGarageProjectImageClient _projectImageService;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _sessionManager;
        private readonly IGarageProjectClient _projectService;


        public ProjectImageController(IGarageProjectImageClient projectImageService, IMapper mapper, IUserSessionManager sessionManager, IGarageProjectClient projectService)
        {
            _projectImageService = projectImageService;
            _mapper = mapper;
            _sessionManager = sessionManager;
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(GarageProjectImageViewModel Model)
        {
            //long GarageId = _sessionManager.GetGarageStore().Id;
            //GarageProjectViewModel Result = _mapper.Map<GarageProjectViewModel>(await _projectService.GetByIdAsync(Model.GarageProjectId));
            //Model.GarageProjectId = GarageId;
            //path.FileName.Contains(cdn)
            //foreach (var item in path)
            //{
            //    Model.ImagePath = path.FileName;

            //}

            GarageProjectImageDTO result = await _projectImageService.AddProjectImage(_mapper.Map<GarageProjectImageDTO>(Model));

            return Json(new
            {
                success = true,
                //url = $"/Garage/Project/Edit/{result.Id}"
            });
        }
    }
}
