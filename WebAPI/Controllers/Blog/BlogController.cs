using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Blog
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;

        public BlogController(IBlogService blogService, IMapper mapper, IFTPUpload fTPUpload)
        {
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogsAsync()
        {
            IEnumerable<BlogDTO> blog = _mapper.Map<IEnumerable<BlogDTO>>(await _blogService.GetAllBlogsAsync());
            return Ok(blog);
        }

        [HttpGet("Master")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMasterAsync()
        {
            return Ok(new SuccessResponse<IEnumerable<BlogDTO>>("", _mapper.Map<IEnumerable<BlogDTO>>(await _blogService.GetAllBlogsAsync())));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            IEnumerable<BlogDTO> blogs = _mapper.Map<IEnumerable<BlogDTO>>(await _blogService.GetBlogByIdAsync(Id));
            return Ok(blogs.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Post(BlogDTO Model)
        {
            Model.Slug = Slugify.GenerateSlug(Model.Title);
            string LogoPath = "/Images/CarMake/";
            if (_fTPUpload.MoveFile(Model.BannerImage, ref LogoPath))
            {
                Model.BannerImage = LogoPath;
            }
            return Ok(_mapper.Map<BlogDTO>(await _blogService.AddBlogAsync(_mapper.Map<Blogs>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Put(BlogDTO Model)
        {
            if (!string.IsNullOrEmpty(Model.Title))
                Model.Slug = Slugify.GenerateSlug(Model.Title);

            string LogoPath = "/Images/Blog/";

            IEnumerable<Blogs> List = await _blogService.GetBlogByIdAsync(Model.Id);
            Blogs blog = List.FirstOrDefault();

            if (Model.BannerImage.Contains("Draft"))
            {
                if (_fTPUpload.MoveFile(Model.BannerImage, ref LogoPath))
                {
                    Model.BannerImage = LogoPath;
                }
            }


            Blogs model = _mapper.Map(Model, blog);

            return Ok(_mapper.Map<BlogDTO>(await _blogService.UpdateBlogAsync(_mapper.Map<Blogs>(blog))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<BlogDTO>(await _blogService.ArchiveBlogAsync(Id)));
        }

        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            /*IEnumerable<Blogs> blogList = await _blogService.GetBlogByIdAsync(Id);
            Blogs blogs = blogList.FirstOrDefault();
            return Ok(_mapper.Map<BlogDTO>(await _blogService.UpdateBlogAsync(blogs)));*/

           
                IEnumerable<Blogs> bloglist = await _blogService.GetBlogByIdAsync(Id);
                  Blogs blog = bloglist.FirstOrDefault();

                if (blog.Status == Enum.GetName(typeof(Status), Status.Active))
                blog.Status = Enum.GetName(typeof(Status), Status.Inactive);
                else
                blog.Status = Enum.GetName(typeof(Status), Status.Active);

                return Ok(_mapper.Map<BlogDTO>(await _blogService.UpdateBlogAsync(blog)));
           
        }
    }
}
