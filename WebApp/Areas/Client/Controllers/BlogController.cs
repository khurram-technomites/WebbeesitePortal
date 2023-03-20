using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelperClasses.DTOs;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;
using Fingers10.ExcelExport.ActionResults;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Handlers;
using Microsoft.AspNetCore.Authorization;
using HelperClasses.DTOs.Blog;
using HelperClasses.Classes;
using WebApp.Interfaces;
using HelperClasses.DTOs.GarageCMS;
//using WebAPI.Models;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class BlogController : Controller
    {
        private readonly IGarageBlogClient _blogService;
        private readonly IGarageClient _garageClient;
        private readonly IUserSessionManager _userSession;
        private readonly IBlogCategoryClient _blogCategory;
        private readonly IClientModulesClient _clientModule;
        private readonly IMapper _mapper;
        [BindProperty]
        public GarageBlogViewModel Model { get; set; }
        public BlogController(IGarageBlogClient blogService,
            IMapper mapper, IUserSessionManager userSession,
            IBlogCategoryClient blogCategory
            , IGarageClient garageClient
            , IClientModulesClient clientModule)
        {
            _blogService = blogService;
            _mapper = mapper;
            _userSession = userSession;
            _blogCategory = blogCategory;
            _garageClient = garageClient;
            _clientModule = clientModule;
        }
        public async Task<ActionResult> Index()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            var blogs = _mapper.Map<IEnumerable<GarageBlogViewModel>>(await _blogService.GetAllByGarageIdAsync(GarageId));
            return View(blogs);
        }
        public async Task<ActionResult> List()
        {
            return PartialView();
        }
        public async Task<IActionResult> Details(long Id)
        {
            IEnumerable<GarageBlogViewModel> blogDetails = _mapper.Map<IEnumerable<GarageBlogViewModel>>(await _blogService.GetAllByIdAsync(Id));
            return View(blogDetails.FirstOrDefault());
        }
        public async Task<ActionResult> Create()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            ViewBag.BlogCategory = await _blogCategory.GetAllByGarageIdAsync(GarageId);
            return View(new GarageBlogViewModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(GarageBlogViewModel model)
        {
            GarageBlogDTO BlogDTO = new GarageBlogDTO();
            long GarageId = _userSession.GetGarageStore().Id;
            model.Status = Enum.GetName(typeof(Status), Status.Active);
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    model.GarageId = _userSession.GetGarageStore().Id;
                    var GarageBlogsCount = await _blogService.GetCountByGarageIdAsync(GarageId);
                    GarageBlogsCount++;
                    var clientModules = await _clientModule.GetClientModuleByClientId(GarageId);
                    foreach (var clientModule in clientModules.Where(s => s.Module.ServiceName == ModulesObject.Blogs && s.ClientId == GarageId))
                    {
                        long module = clientModule.Quantity;

                        if (module == 0 || GarageBlogsCount <= module)
                        {

                            GarageBlogDTO Result = await _blogService.AddGarageBlogAsync(_mapper.Map<GarageBlogDTO>(model));

                            return Json(new
                            {
                                success = true,
                                url = "/Client/Blog/Index",
                                message = "Blog Created Successfully",
                                data = new
                                {
                                    Date = Result.CreationDate.ToString("dd MMM yyyy, h: mm tt"),
                                    Title = Result.Title,
                                    Image = Result.ImagePath,
                                    Status = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                                    Id = Result.Id
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
        [HttpPost]
        public async Task<IActionResult> Edit(GarageBlogViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    long GarageId = _userSession.GetGarageStore().Id;
                    ViewBag.BlogCategory = await _blogCategory.GetAllByGarageIdAsync(GarageId);
                    GarageBlogDTO blog = _mapper.Map<GarageBlogDTO>(model);

                    blog.Garage = null;
                    GarageBlogDTO Result = await _blogService.UpdateGarageBlogAsync(blog);

                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        message = "Blog Updated Successfully",
                        data = new
                        {
                            Date = Result.CreationDate.ToString("dd MMM yyyy, h: mm tt"),
                            Title = Result.Title,
                            Image = Result.ImagePath,
                            Status = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                            Id = Result.Id
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
            ViewBag.BlogCategory = await _blogCategory.GetBlogCategoriesByModule(Enum.GetName(typeof(BlogCategory), BlogCategory.Garage));

            IEnumerable<GarageBlogViewModel> blog = _mapper.Map<IEnumerable<GarageBlogViewModel>>(await _blogService.GetAllByIdAsync(Id));
            return View(blog.FirstOrDefault());
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _blogService.DeleteGarageBlogAsync(Id);

                return Json(new
                {
                    success = true,
                    message = "Blog Deleted Successfully"
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

        public async Task<ActionResult> ToggleActiveStatus(long Id)
        {
            try
            {
                long GarageId = _userSession.GetGarageStore().Id;
                GarageViewModel garages = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(GarageId));
                GarageDTO garage = new();
                garage.IsServicesAllowed = garages.IsServicesAllowed;
                garage.IsBlogsAllowed = garages.IsBlogsAllowed;
                garage.IsCareersAllowed = garages.IsCareersAllowed;
                garage.IsFeedbackAllowed = garages.IsFeedbackAllowed;
                garage.IsAppoinmnetsAllowed = garages.IsAppoinmnetsAllowed;
                garage.IsTeamsAllowed = garages.IsTeamsAllowed;
                GarageBlogViewModel Result = _mapper.Map<GarageBlogViewModel>(await _blogService.ToggleStatus(Id));

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        Date = Result.CreationDate.ToString("dd MMM yyyy, h: mm tt"),
                        Title = Result.Title,
                        Image = Result.ImagePath,
                        Status = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                        Id = Result.Id
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
