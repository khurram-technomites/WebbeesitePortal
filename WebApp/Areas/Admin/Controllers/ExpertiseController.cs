using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Expertises.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class ExpertiseController : Controller
    {
        private readonly IExpertiseClient _ExpertiseService;
        private readonly IMapper _mapper;
        [BindProperty]
        public ExpertiseViewModel Model { get; set; }
        public ExpertiseController(IExpertiseClient ExpertiseService, IMapper mapper)
        {
            _ExpertiseService = ExpertiseService;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<ExpertiseViewModel>>(await _ExpertiseService.GetAllAsync());
            return View(info);
        }
        public async Task<IActionResult> Details(long Id)
        {
            IEnumerable<ExpertiseViewModel> lists = _mapper.Map<IEnumerable<ExpertiseViewModel>>(await _ExpertiseService.GetAllByIdAsync(Id));
            return View(lists.FirstOrDefault());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(ExpertiseViewModel model)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Status = Enum.GetName(typeof(Status), Status.Active);
                    ExpertiseDTO Result = await _ExpertiseService.AddExpertiseAsync(_mapper.Map<ExpertiseDTO>(model));

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Expertise/Index",
                        message = "Expertise Created Successfully",
                        data = new
                        {
                            CreationDate = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy") : "-",
                            ID = Result.Id,
                            Title = Result.Title,
                            Status = Result.Status,
                            
                        }
                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Admin/Expertise/Index",
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
        public async Task<IActionResult> Edit(ExpertiseViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    ExpertiseDTO Result = await _ExpertiseService.UpdateExpertiseAsync(_mapper.Map<ExpertiseDTO>(model));

                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Status = Result.Status,
                            Title = Result.Title,
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
            IEnumerable<ExpertiseViewModel> ExpertiseDTO = _mapper.Map<IEnumerable<ExpertiseViewModel>>(await _ExpertiseService.GetAllByIdAsync(Id));
            return View(ExpertiseDTO.FirstOrDefault());
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _ExpertiseService.DeleteExpertiseAsync(Id);

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

        public async Task<ActionResult> ToggleActiveStatus(long id)
        {
            try
            {
                ExpertiseViewModel Result = _mapper.Map<ExpertiseViewModel>(await _ExpertiseService.ToggleActiveStatus(id));

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        CreationDate = Result.CreationDate.ToString("dd MMM yyyy"),
                        Status = Result.Status,
                        Title = Result.Title,
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
    }
}
