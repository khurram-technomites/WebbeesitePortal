using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs;
using WebApp.ErrorHandling;
using Newtonsoft.Json;
using Fingers10.ExcelExport.ActionResults;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AreaController : Controller
    {
        private readonly IAreaClient _areaService;
        private readonly ICityClient _cityService;
        private readonly IMapper _mapper;
        [BindProperty]
        public AreaViewModel Model { get; set; }
        public AreaController(IAreaClient areaService, ICityClient cityService, IMapper mapper)
        {
            _areaService = areaService;
            _cityService = cityService;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<AreaViewModel>>(await _areaService.GetAreas());
            return View(info);
        }
        public async Task<ActionResult> List()
        {

            return PartialView();
        }
        public async Task<IActionResult> Details(long Id)
        {
            return View(_mapper.Map<AreaViewModel>(await _areaService.GetAreaByID(Id)));
        }
        public async Task<ActionResult> Create()
        {
            var cities = _mapper.Map<IEnumerable<CityViewModel>>(await _cityService.GetCities());
            ViewBag.City = cities;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(AreaViewModel model)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    //return RedirectToAction(nameof(Create), new { Id = "" });
                    AreaDTO Result = await _areaService.Create(_mapper.Map<AreaDTO>(model));

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Area/Index",
                        message = "Area Created Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            CreationDate = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy") : "-",
                            Name = Result.Name,
                            CityId = Result.CityId,
                            //Parent = Parent != null ? (Parent.CategoryName) : "",
                            //IsParentCategoryDeleted = category.IsParentCategoryDeleted.HasValue ? category.IsParentCategoryDeleted.Value.ToString() : bool.FalseString,
                            /*IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false*/
                        }
                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Admin/Area/Index",
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
        [HttpPost]
        public async Task<IActionResult> Edit(AreaViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    AreaDTO Result = await _areaService.Edit(_mapper.Map<AreaDTO>(model));

                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            CreationDate = Result.CreationDate.ToString("dd MMM yyyy"),
                            Name = Result.Name,
                            CityId = Result.CityId,
                            //Parent = Parent != null ? (Parent.CategoryName) : "",
                            //IsParentCategoryDeleted = category.IsParentCategoryDeleted.HasValue ? category.IsParentCategoryDeleted.Value.ToString() : bool.FalseString,
                            /*IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false*/
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
        public async Task<IActionResult> Edit(long Id)
        {
            var cities = _mapper.Map<IEnumerable<CityViewModel>>(await _cityService.GetCities());
            ViewBag.City = cities;
            AreaViewModel areaDTO = _mapper.Map<AreaViewModel>(await _areaService.GetAreaByID(Id));
            return View(areaDTO);
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _areaService.Delete(Id);

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
        public async Task<IActionResult> AreaMakeReport()
        {
            var info = _mapper.Map<List<AreaViewModel>>(await _areaService.GetAreas());
            return new CSVResult<AreaViewModel>(info, "Area");
        }
    }
}
