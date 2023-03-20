using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
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
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin , GarageOwner")]
    public class CarModelController : ControllerBase
    {
        private readonly ICarModelService _service;
        private readonly IMapper _mapper;
        private readonly IFTPUpload _fTPUpload;

        public CarModelController(ICarModelService service, IMapper mapper, IFTPUpload fTPUpload)
        {
            _service = service;
            _fTPUpload = fTPUpload;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<CarModelDTO>>(await _service.GetAllAsync()).OrderBy(x => x.CarMake.Name));
        }

        [HttpGet("Master")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMaster()
        {
            return Ok(new SuccessResponse<IEnumerable<CarModelDTO>>("", _mapper.Map<IEnumerable<CarModelDTO>>(await _service.GetAllAsync())));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<CarModelDTO> List = _mapper.Map<IEnumerable<CarModelDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpGet("ByMake/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCarMake(long Id)
        {
            return Ok(new SuccessResponse<IEnumerable<CarModelDTO>>("", _mapper.Map<IEnumerable<CarModelDTO>>(await _service.GetByCarMakeAsync(Id))));
        }

        [HttpPost]
        public async Task<IActionResult> Add(CarModelDTO Model)
        {
            string LogoPath = "/Images/CarModel/";
            if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
            {
                Model.Logo = LogoPath;
            }
            return Ok(_mapper.Map<CarModelDTO>(await _service.AddCarModelAsync(_mapper.Map<CarModel>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CarModelDTO Model)
        {
            string LogoPath = "/Images/CarModel/";

            IEnumerable<CarModel> List = await _service.GetByIdAsync(Model.Id);
            CarModel carModel = List.FirstOrDefault();

            if (Model.Logo.Contains("Draft"))
            {
                if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                {
                    Model.Logo = LogoPath;
                }
            }


            CarModel model = _mapper.Map(Model, carModel);
            return Ok(_mapper.Map<CarModelDTO>(await _service.UpdateCarModelAsync(_mapper.Map<CarModel>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<CarModelDTO>(await _service.ArchiveCarModelAsync(Id)));
        }

        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<CarModel> carMake = await _service.GetByIdAsync(Id);
            CarModel make = carMake.FirstOrDefault();

            if (make.Status == Enum.GetName(typeof(Status), Status.Active))
                make.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                make.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _service.UpdateCarModelAsync(make));
        }
    }
}
