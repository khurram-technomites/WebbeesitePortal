using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
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
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class CarModelController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICarModelClient _client;
        private readonly ICarMakeClient _carMakeClient;
        //[BindProperty]
        //public UserViewModel Model { get; set; }

        public CarModelController(IMapper mapper, ICarModelClient client, ICarMakeClient carMakeClient)
        {
            _carMakeClient = carMakeClient;
            _mapper = mapper;
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var Info = _mapper.Map<IEnumerable<CarModelViewModel>>(await _client.GetAllCarModelsAsync());
            return View(Info);
        }

        public async Task<IActionResult> Create()
        {
            var carMake = _mapper.Map<IEnumerable<CarMakeViewModel>>(await _carMakeClient.GetAllCarMakesAsync());
            ViewBag.CarMake = carMake;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarModelViewModel model)
        {
            try
            {
                model.Status = Enum.GetName(typeof(Status), Status.Active);
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    //string FilePath = string.Format("{0}{1}{2}", Server.MapPath("~/Assets/AppFiles/Images/Sports/"), Name.Replace(" ", "_"), "/Image");

                    //string absolutePath = Server.MapPath("~");
                    //string relativePath = string.Format("/Assets/AppFiles/Images/Sports/{0}/", Name.Replace(" ", "_"));
                    //Sport category = new Sport();
                    //category.Name = Name;
                    //category.NameAr = NameAr;

                    //category.Position = Position;
                    ////for (int i = 0; i < Request.Files.Count; i++)
                    ////{

                    ////}
                    //if (Request.Files["Image"] != null)
                    //{

                    //	category.Logo = Request.Files["Image"] != null ? Uploader.UploadImage(Request.Files["Image"], absolutePath, relativePath, "Logo", ref message, "Image") : null;
                    //}
                    //if (Request.Files["Icon"] != null)
                    //{

                    //	category.Icon = Request.Files["Icon"] != null ? Uploader.UploadImage(Request.Files["Icon"], absolutePath, relativePath, "Icon", ref message, "Image") : null;
                    //}
                    ////if (Request.Files.Count>0)
                    ////{
                    ////	//category.Logo = Uploader.UploadImage(Request.Files[0], absolutePath, relativePath, Request.Files[0].FileName, ref message, "Image");
                    ////	category.Icon = Request.Files[0].FileName == "logo" ? Uploader.UploadImage(Request.Files[1], absolutePath, relativePath, "Icon", ref message, "Image");

                    ////}
                    ////if (Request.Files[1] != null)
                    ////{ 
                    ////}


                    CarModelDTO Result = await _client.AddCarModelAsync(_mapper.Map<CarModelDTO>(model));

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/CarModel/Index",
                        message = "Record successfully created!",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            CarMake = Result.Name,
                            Name = Result.Name,
                            //Parent = Parent != null ? (Parent.CategoryName) : "",
                            //IsParentCategoryDeleted = category.IsParentCategoryDeleted.HasValue ? category.IsParentCategoryDeleted.Value.ToString() : bool.FalseString,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
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
            var carMake = _mapper.Map<IEnumerable<CarMakeViewModel>>(await _carMakeClient.GetAllCarMakesAsync());
            ViewBag.CarMake = carMake;
            CarModelViewModel CarModel = _mapper.Map<CarModelViewModel>(await _client.GetCarModelByIdAsync(id));
            return View(CarModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CarModelViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Logo = model.Logo == "undefined" ? "" : model.Logo;

                    CarModelDTO Result = await _client.UpdateCarModelAsync(_mapper.Map<CarModelDTO>(model));

                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            CarMake = Result.Name,
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

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });

        }

        public async Task<ActionResult> ToggleActiveStatus(long id)
        {
            try
            {
                CarModelViewModel Result = _mapper.Map<CarModelViewModel>(await _client.ToggleActiveStatus(id));

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        CarMake = Result.CarMake.Name,
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
                await _client.DeleteCarModelAsync(id);

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
            CarModelViewModel a = _mapper.Map<CarModelViewModel>(await _client.GetCarModelByIdAsync(id));
            return View(a);
        }

        public async Task<IActionResult> CarModelReport()
        {
            PagingParameters pagging = new PagingParameters();
            pagging.PageSize = 10;
            pagging.PageNumber = 1;
            var Info = _mapper.Map<IEnumerable<CarModelViewModel>>(await _client.GetAllCarModelsAsync());
            return new CSVResult<CarModelViewModel>(Info, "CarModel");
        }
    }
}
