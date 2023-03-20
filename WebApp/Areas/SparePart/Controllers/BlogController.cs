using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.SparePartCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.SparePart.Controllers
{
    [Area("SparePart")]
    [Authorize(Roles = "SparePartDealer")]
    public class BlogController : Controller
    {
        private readonly ISparePartBlogClient _blogService;
        private readonly IUserSessionManager _userSession;
        private readonly IMapper _mapper;
        [BindProperty]
        public SparePartBlogViewModel Model { get; set; }
        public BlogController(ISparePartBlogClient blogService, IMapper mapper, IUserSessionManager userSession)
        {
            _blogService = blogService;
            _mapper = mapper;
            _userSession = userSession;
        }
        public async Task<ActionResult> Index()
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            var blogs = _mapper.Map<IEnumerable<SparePartBlogViewModel>>(await _blogService.GetAllBySparePartDealerIdAsync(SparePartId));
            return View(blogs);
        }
        public async Task<ActionResult> List()
        {
            return PartialView();
        }
        public async Task<IActionResult> Details(long Id)
        {
            IEnumerable<SparePartBlogViewModel> blogDetails = _mapper.Map<IEnumerable<SparePartBlogViewModel>>(await _blogService.GetAllByIdAsync(Id));
            return View(blogDetails.FirstOrDefault());
        }
        public async Task<ActionResult> Create()
        {
            SparePartBlogViewModel blog = new SparePartBlogViewModel();
            return View(blog);
        }
        [HttpPost]
        public async Task<ActionResult> Create(SparePartBlogViewModel model)
        {
            model.Status = Enum.GetName(typeof(Status), Status.Active);
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    model.SparePartDealerId = _userSession.GetSparePartDealerStore().Id;
                    SparePartBlogDTO Result = await _blogService.AddSparePartBlogAsync(_mapper.Map<SparePartBlogDTO>(model));

                    return Json(new
                    {
                        success = true,
                        url = "/SparePart/Blog/Index",
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
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/SparePart/Blog/Index",
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
        public async Task<IActionResult> Edit(SparePartBlogViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SparePartBlogDTO Result = await _blogService.UpdateSparePartBlogAsync(_mapper.Map<SparePartBlogDTO>(model));

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
            IEnumerable<SparePartBlogViewModel> blog = _mapper.Map<IEnumerable<SparePartBlogViewModel>>(await _blogService.GetAllByIdAsync(Id));
            return View(blog.FirstOrDefault());
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _blogService.DeleteSparePartBlogAsync(Id);

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
                SparePartBlogViewModel Result = _mapper.Map<SparePartBlogViewModel>(await _blogService.ToggleStatus(Id));
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
