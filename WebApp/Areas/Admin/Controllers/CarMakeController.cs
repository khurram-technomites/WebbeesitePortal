using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Admin.Models;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CarMakeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICarMakeClient _client;
        private readonly IFileUpload _fileUpload;

        //[BindProperty]
        //public UserViewModel Model { get; set; }

        public CarMakeController(IMapper mapper, ICarMakeClient client, IFileUpload fileUpload)
        {
            _mapper = mapper;
            _fileUpload = fileUpload;
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var Info = _mapper.Map<IEnumerable<CarMakeViewModel>>(await _client.GetAllCarMakesAsync());
            return View(Info);
        }

        public async Task<IActionResult> Create()
        {
            return View(new CarMakeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarMakeViewModel model)
        {
            try
            {
                model.Status = Enum.GetName(typeof(Status), Status.Active);
                string message = string.Empty;
                if (ModelState.IsValid)
                {

                    CarMakeDTO Result = await _client.AddCarMakeAsync(_mapper.Map<CarMakeDTO>(model));

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/CarMake/Index",
                        message = "Record Created Successfully!",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate.ToString("dd MMM yyyy"),
                            Name = Result.Name,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active)
                        }
                    });


                }
                else
                {
                    message = "Please fill the form properly ...";
                }
                return Json(new { success = false, message = message });
            }
            catch (ApiException ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                });
            }

        }

        public async Task<IActionResult> Edit(long id)
        {
            CarMakeViewModel carMakeDTO = _mapper.Map<CarMakeViewModel>(await _client.GetCarMakeByIdAsync(id));
            return View(carMakeDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CarMakeViewModel model)
        {
            try
            {
                CarMakeDTO Result = await _client.UpdateCarMakeAsync(_mapper.Map<CarMakeDTO>(model));

                Result.Id = model.Id;

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.Name,
                        //Parent = Parent != null ? (Parent.CategoryName) : "",
                        //IsParentCategoryDeleted = category.IsParentCategoryDeleted.HasValue ? category.IsParentCategoryDeleted.Value.ToString() : bool.FalseString,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
                    }
                });
            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                    message = err.Message
                });
            }

        }

        public async Task<ActionResult> ToggleActiveStatus(long id)
        {
            try
            {
                CarMakeViewModel Result = _mapper.Map<CarMakeViewModel>(await _client.ToggleActiveStatus(id));

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.Name,
                        //Parent = Parent != null ? (Parent.CategoryName) : "",
                        //IsParentCategoryDeleted = category.IsParentCategoryDeleted.HasValue ? category.IsParentCategoryDeleted.Value.ToString() : bool.FalseString,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
                    }

                });
            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                    message = err.Message
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _client.DeleteCarMakeAsync(id);

                return Json(new
                {
                    success = true,
                    message = "Record Deleted Successfully"
                });
            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                    message = err.Message
                });
            }
        }

        public async Task<IActionResult> Details(long id)
        {
            CarMakeViewModel model = _mapper.Map<CarMakeViewModel>(await _client.GetCarMakeByIdAsync(id));
            return View(model);
        }

        public async Task<IActionResult> CarMakeReport()
        {
            var Info = _mapper.Map<IEnumerable<CarMakeViewModel>>(await _client.GetAllCarMakesAsync());
            return new CSVResult<CarMakeViewModel>(Info, "CarMake");
        }


    }
}
