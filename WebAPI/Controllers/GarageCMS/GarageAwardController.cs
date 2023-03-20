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
    [Route("api/Garage/Award")]
    [ApiController]
    public class GarageAwardController : ControllerBase
    {
        private readonly IGarageAwardService _garageAward;
        private readonly IMapper _mapper;
        private readonly IFTPUpload _fTPUpload;


        public GarageAwardController(IGarageAwardService garageAward, IMapper mapper, IFTPUpload fTPUpload)
        {

            _garageAward = garageAward;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("GetAll/Garages/{id}")]
        public async Task<IActionResult> GetAll(long id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageAwardDTO>>(await _garageAward.GetAllGarageAwardsAsync(id)));
        }
        [HttpGet("GetAll/Garages/Count/{id}")]
        public async Task<IActionResult> GetCount(long id)
        {
            return Ok(_mapper.Map<long>(await _garageAward.GetCountAllGarageAwardsAsync(id)));
        }



        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<GarageAwardDTO> List = _mapper.Map<IEnumerable<GarageAwardDTO>>(await _garageAward.GetGarageAwardByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Add(GarageAwardDTO Model)
        {
            

            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                string LogoPath = "/Images/Garage/Award";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }

            return Ok(_mapper.Map<GarageAwardDTO>(await _garageAward.AddGarageAwardAsync(_mapper.Map<GarageAward>(Model))));
        }


        [HttpPut]
        public async Task<IActionResult> Update(GarageAwardDTO Model)
        {
            string LogoPath = "/Images/Garage/Award";

            IEnumerable<GarageAward> List = await _garageAward.GetGarageAwardByIdAsync(Model.Id);
            GarageAward GarageAward = List.FirstOrDefault();

            if (!string.IsNullOrEmpty(GarageAward.ImagePath))
            {
                if (Model.ImagePath.Contains("Draft"))
                {
                    if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                    {
                        Model.ImagePath = LogoPath;
                    }
                }
            }



            GarageAward model = _mapper.Map(Model, GarageAward);

            return Ok(_mapper.Map<GarageAwardDTO>(await _garageAward.UpdateGarageAwardAsync(_mapper.Map<GarageAward>(model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageAwardDTO>(await _garageAward.ArchiveGarageAwardAsync(Id)));
        }

    }
}
