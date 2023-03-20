using AutoMapper;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;
using HelperClasses.DTOs.GarageCMS;
using System.Linq;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using System;
using WebApp.Services.TypedClients;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class TestimonialsController : Controller
    {
        private readonly IGarageTestimonialsClient _serviceTestimonialsClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;
        private readonly IClientModulesClient _clientModule;
        public TestimonialsController(IGarageTestimonialsClient serviceTestimonialsClient, IMapper mapper,
            IUserSessionManager userSession
            , IClientModulesClient clientModule)
        {
            _serviceTestimonialsClient = serviceTestimonialsClient;
            _mapper = mapper;
            _userSession = userSession;
            _clientModule = clientModule;
        }

        public async Task<IActionResult> Index()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            return View(_mapper.Map<IEnumerable<GarageTestimonialsViewModel>>(await _serviceTestimonialsClient.GetAllByGarageIdAsync(GarageId)));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GarageTestimonialsViewModel Model)
        {
            long GarageId = _userSession.GetGarageStore().Id;

            Model.GarageId = GarageId;
            if (ModelState.IsValid)
            {
                try
                {
                    var GarageTestoimonialCount = await _serviceTestimonialsClient.GetAllCountByGarageIdAsync(GarageId);
                    GarageTestoimonialCount++;
                    var ClientModules = await _clientModule.GetClientModuleByClientId(GarageId);
                    foreach (var ClientModule in ClientModules.Where(s => s.Module.ServiceName == ModulesObject.Testimonial && s.ClientId == GarageId))
                    {
                        long module = ClientModule.Quantity;
                        if (module == 0 || GarageTestoimonialCount <= module)
                        {
                            GarageTestimonialsDTO Result = await _serviceTestimonialsClient.AddGarageTestimonialsAsync(_mapper.Map<GarageTestimonialsDTO>(Model));

                            return Json(new
                            {
                                success = true,
                                url = "/Garage/GarageTestimonials/Index",
                                message = "Record Added Successfully",
                                data = new
                                {
                                    Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                                    Name = Result.CustomerName,
                                    Testimonial = Result.Testimonial,
                                    Rating = Result.Rating,
                                    Id = Result.Id,
                                }
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                success = false,
                                url = "/Client/Blog/Index",
                                message = $"You can't add more than {module} Blogs",
                            });
                        }
                    }



                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Client/Blog/Index",
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
            IEnumerable<GarageTestimonialsViewModel> result = _mapper.Map<IEnumerable<GarageTestimonialsViewModel>>(await _serviceTestimonialsClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Edit(GarageTestimonialsViewModel Model)
        {
            long GarageId = _userSession.GetGarageStore().Id;

            Model.GarageId = GarageId;
            GarageTestimonialsDTO result = await _serviceTestimonialsClient.UpdateGarageTestimonialsAsync(_mapper.Map<GarageTestimonialsDTO>(Model));

            return Json(new
            {
                success = true,
                url = "/Garage/GarageTestimonials/Index",
                message = "Record Updated Successfully",
                data = new
                {
                    Date = result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                    Name = result.CustomerName,
                    Testimonial = result.Testimonial,
                    Rating = result.Rating,
                    Image = result.CustomerImage,
                    Show = result.ShowOnWebsite,
                    Id = result.Id,
                }
            });
        }
        public async Task<IActionResult> Delete(long Id)
        {
            try
            {
                await _serviceTestimonialsClient.DeleteGarageTestimonialsAsync(Id);

                return Json(new
                {
                    success = true,
                    message = "Team Deleted Successfully"
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
                GarageTestimonialsViewModel result = _mapper.Map<GarageTestimonialsViewModel>(await _serviceTestimonialsClient.ToggleActiveStatus(id));
                return Json(new
                {
                    success = true,
                    url = "/Garage/GarageTestimonials/Index",
                    message = "Record Added Successfully",
                    data = new
                    {
                        Date = result.CreationDate,
                        Name = result.CustomerName,
                        Testimonial = result.Testimonial,
                        Rating = result.Rating,
                        Id = result.Id,
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
        public async Task<IActionResult> Detail(long Id)
        {
            IEnumerable<GarageTestimonialsViewModel> result = _mapper.Map<IEnumerable<GarageTestimonialsViewModel>>(await _serviceTestimonialsClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }
    }
}
