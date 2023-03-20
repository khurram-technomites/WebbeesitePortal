using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Blog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Customers
{
    [Route("api/Customer/Blog")]
    [ApiController]
    public class CustomerBlogController : ControllerBase
    {
        private readonly IBlogService _service;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public CustomerBlogController(IBlogService service, IMapper mapper, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(PagingParameters paging)
        {
            AppUser admin = await _userManager.FindByNameAsync("admin@fougito.com");
            IEnumerable<BlogCardResponse> BlogList = _mapper.Map<IEnumerable<BlogCardResponse>>(await _service.GetAllBlogsAsync(paging));
            foreach (var item in BlogList)
                item.AuthorImage = admin.Logo;

            return Ok(BlogList);
        }

        [HttpGet("Count")]
        public async Task<IActionResult> getCount()
        {
            return Ok(new
            {
                Message = "",
                Result = await _service.GetTotalCount()
            });
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            IEnumerable<BlogDTO> List = _mapper.Map<IEnumerable<BlogDTO>>(await _service.GetBlogBySlugAsync(slug));
            return Ok(List.FirstOrDefault());
        }
    }
}
