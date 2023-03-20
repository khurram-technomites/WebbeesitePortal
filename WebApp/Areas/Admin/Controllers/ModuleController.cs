using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;
using AutoMapper;
using WebApp.ErrorHandling;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Automobile Manager,GarageOwner")]
    public class ModuleController : Controller
    {
        private readonly IModuleClient _client;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;
        public ModuleController(IModuleClient client, IMapper mapper, IUserSessionManager userSessionManager)
        {
            _client = client;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
        }

        public async Task<IActionResult> Index()
        {
            
            IEnumerable<ModuleViewModel> Info = _mapper.Map<IEnumerable<ModuleViewModel>>(await _client.GetAllAsync());
            return View(Info);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ModuleViewModel model)
        {
            try
            {
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                   
                    ModuleDTO Result = await _client.AddModuleAsync(_mapper.Map<ModuleDTO>(model));


                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Module/Index",
                        message = "Record Added Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            CreatedOn = Result.CreationDate.ToString("dd MMM yyyy"),
                            Name = Result.ServiceName,
                            Price = Result.Price,
                            Status = Result.IsActive == true ? "Active" : "InActive",
                            IsSystem = Result.IsSystem,
                            IsDefault = Result.IsDefault
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
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                   
                    message = err.Message
                });
            }


        }

        public async Task<IActionResult> Edit(long id)
        {
            var Info = _mapper.Map<IEnumerable<ModuleViewModel>>(await _client.GetModuleById(id));
            return View(Info.FirstOrDefault());
        }

        [HttpPost]

        public async Task<IActionResult> Edit(ModuleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ModuleDTO Result = await _client.UpdateModuleAsync(_mapper.Map<ModuleDTO>(model));


                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Module/Index",
                        message = "Record Updated Successfully",
                        data = new
                        {

                            ID = Result.Id,
                            CreatedOn = Result.CreationDate.ToString("dd MMM yyyy"),
                            Name = Result.ServiceName,
                            Price = Result.Price,
                            Status = Result.IsActive == true ? "Active" : "InActive",
                            IsSystem = Result.IsSystem,
                            IsDefault = Result.IsDefault
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _client.DeleteModuleAsync(id);

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
            var Info = _mapper.Map<IEnumerable<ModuleViewModel>>(await _client.GetModuleById(id));
            return View(Info.FirstOrDefault());
        }
        public async Task<ActionResult> ToggleActiveStatus(long Id)
        {
            try
            {
                ModuleViewModel Result = _mapper.Map<ModuleViewModel>(await _client.ToggleActiveStatus(Id));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Name = Result.ServiceName,
                        Price = Result.Price,
                        Status = Result.IsActive == true ? "Active" : "InActive"
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
