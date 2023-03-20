using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using HelperClasses.Classes;
using WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.Blog;
using WebApp.ErrorHandling;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class BlogCategoryController : Controller
    {
        private readonly IGarageBlogClient _blogService;
        private readonly IUserSessionManager _userSession;
        private readonly IBlogCategoryClient _blogCategory;
        private readonly IMapper _mapper;
        [BindProperty]
        public BlogCategoryViewModel Model { get; set; }
        public BlogCategoryController(IGarageBlogClient blogService, IMapper mapper, IUserSessionManager userSession, IBlogCategoryClient blogCategory)
        {
            _blogService = blogService;
            _mapper = mapper;
            _userSession = userSession;
            _blogCategory = blogCategory;
        }
        public async Task<IActionResult> Index()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            var blogcategory = _mapper.Map<IEnumerable<BlogCategoryViewModel>>(await _blogCategory.GetAllByGarageIdAsync(GarageId));
            return View(blogcategory);
        }
        public async Task<IActionResult> Details(long Id)
        {
            IEnumerable<BlogCategoryViewModel> blogDetails = _mapper.Map<IEnumerable<BlogCategoryViewModel>>(await _blogCategory.GetBlogCategoriesById(Id));
            return View(blogDetails.FirstOrDefault());
        }
        public async Task<ActionResult> Create()
        {
            return View(new BlogCategoryViewModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(BlogCategoryViewModel model)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    model.GarageId = _userSession.GetGarageStore().Id;
                    BlogCategoryDTO Result = await _blogCategory.AddBlogCategory(_mapper.Map<BlogCategoryDTO>(model));

                    return Json(new
                    {
                        success = true,
                        url = "/Client/BlogCategory/Index",
                        message = "Blog Created Successfully",
                        data = new
                        {
                            Date = Result.CreationDate.ToString("dd MMM yyyy, h: mm tt"),
                            Title = Result.Title,
                            Module = Result.Module,
                            Id = Result.Id
                        }
                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Client/BlogCategory/Index",
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
        public async Task<IActionResult> Edit(BlogCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    BlogCategoryDTO Result = await _blogCategory.UpdateBlogCategory(_mapper.Map<BlogCategoryDTO>(model));

                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        message = "Blog Updated Successfully",
                        data = new
                        {
                            Date = Result.CreationDate.ToString("dd MMM yyyy, h: mm tt"),
                            Title = Result.Title,
                            Module = Result.Module,
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
            IEnumerable<BlogCategoryViewModel> blogcategory = _mapper.Map<IEnumerable<BlogCategoryViewModel>>(await _blogCategory.GetBlogCategoriesById(Id));
            return View(blogcategory.FirstOrDefault());
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _blogCategory.ArchiveBlogCategory(Id);

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
    }
}
