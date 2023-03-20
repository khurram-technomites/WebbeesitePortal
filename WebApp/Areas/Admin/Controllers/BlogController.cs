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

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly IBlogClient _blogService;
        private readonly IMapper _mapper;
        [BindProperty]
        public BlogViewModel Model { get; set; }
        public BlogController(IBlogClient blogService, IMapper mapper)
        {
            _blogService = blogService;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index()
        {
            var blogs = _mapper.Map<IEnumerable<BlogViewModel>>(await _blogService.GetBlogs());
            return View(blogs);
        }
        public async Task<ActionResult> List()
        {
            return PartialView();
        }
        public async Task<IActionResult> Details(long Id)
        {
            BlogViewModel blogDetails = _mapper.Map<BlogViewModel>(await _blogService.GetBlogByID(Id));
            return View(blogDetails);
        }
        public async Task<ActionResult> Create()
        {
            BlogViewModel blog = new BlogViewModel();
            return View(blog);
        }
        [HttpPost]
        public async Task<ActionResult> Create(BlogViewModel model)
        {
            model.Status = Enum.GetName(typeof(Status), Status.Active);
            model.PostedDate = DateTime.UtcNow;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    BlogDTO Result = await _blogService.Create(_mapper.Map<BlogDTO>(model));

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Blog/Index",
                        message = "Blog Created Successfully",
                        data = new
                        {
                            Date = Result.PostedDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Title = Result.BannerImage + "|" + Result.Title,
                            Status = Result.Status,
                            ID = Result.Id
                        }
                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Admin/Blog/Index",
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
        public async Task<IActionResult> Edit(BlogViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    BlogDTO Result = await _blogService.Edit(_mapper.Map<BlogDTO>(model));

                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        message = "Blog Updated Successfully",
                        data = new
                        {
                            Date = Result.PostedDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Title = Result.BannerImage + "|" + Result.Title,
                            Status = Result.Status,
                            ID = Result.Id
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
            BlogViewModel blog = _mapper.Map<BlogViewModel>(await _blogService.GetBlogByID(Id));
            return View(blog);
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _blogService.Delete(Id);

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
                BlogViewModel Result = _mapper.Map<BlogViewModel>(await _blogService.ToggleActiveStatus(Id));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        Date = Result.PostedDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Title = Result.BannerImage + "|" + Result.Title,
                        Status = Result.Status,
                        ID = Result.Id
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
