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
    [Authorize(Roles = "Admin")]
    public class CarMakeController : ControllerBase
    {
        private readonly ICarMakeService _carMakeService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;

        public CarMakeController(ICarMakeService carMakeService, IMapper mapper, IFTPUpload fTPUpload)
        {
            _carMakeService = carMakeService;
            _fTPUpload = fTPUpload;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<CarMakeDTO>>(await _carMakeService.GetAllCarMakesAsync()).OrderBy(x => x.Name));
        }

        [HttpGet("Master")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMaster()
        {
            return Ok(new SuccessResponse<IEnumerable<CarMakeDTO>>("", _mapper.Map<IEnumerable<CarMakeDTO>>(await _carMakeService.GetAllCarMakesAsync())));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<CarMakeDTO> List = _mapper.Map<IEnumerable<CarMakeDTO>>(await _carMakeService.GetCarMakeByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Add(CarMakeDTO Model)
        {
            string LogoPath = "/Images/CarMake/";
            if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
            {
                Model.Logo = LogoPath;
            }

            return Ok(_mapper.Map<CarMakeDTO>(await _carMakeService.AddCarMakeAsync(_mapper.Map<CarMake>(Model))));
        }


        [HttpPut]
        public async Task<IActionResult> Update(CarMakeDTO Model)
        {
            string LogoPath = "/Images/CarMake/";

            IEnumerable<CarMake> List = await _carMakeService.GetCarMakeByIdAsync(Model.Id);
            CarMake carMake = List.FirstOrDefault();

            if (!string.IsNullOrEmpty(carMake.Logo))
            {
                if (Model.Logo.Contains("Draft"))
                {
                    if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                    {
                        Model.Logo = LogoPath;
                    }
                }
            }
            


            CarMake model = _mapper.Map(Model, carMake);

            return Ok(_mapper.Map<CarMakeDTO>(await _carMakeService.UpdateCarMakeAsync(_mapper.Map<CarMake>(model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<CarMakeDTO>(await _carMakeService.ArchiveCarMakeAsync(Id)));
        }


        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<CarMake> carMake = await _carMakeService.GetCarMakeByIdAsync(Id);
            CarMake make = carMake.FirstOrDefault();

            if (make.Status == Enum.GetName(typeof(Status), Status.Active))
                make.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                make.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _carMakeService.UpdateCarMakeAsync(make));
        }
    }
}
