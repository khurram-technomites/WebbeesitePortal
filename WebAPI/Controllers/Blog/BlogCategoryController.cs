using AutoMapper;
using HelperClasses.DTOs.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Blog
{
    [Route("api/Blog/Category")]
    [ApiController]

    public class BlogCategoryController : ControllerBase
    {
        private readonly IBlogCategoryService _categoryService;
        private readonly IMapper _mapper;

        public BlogCategoryController(IMapper mapper, IBlogCategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<BlogCategoryDTO> blogs = _mapper.Map<IEnumerable<BlogCategoryDTO>>(await _categoryService.GetAllBlogCategories());
             return Ok(blogs);
        }

        [HttpGet("Module/{Module}")]
        public async Task<IActionResult> GetAll(string Module)
        {
            IEnumerable<BlogCategoryDTO> blogs = _mapper.Map<IEnumerable<BlogCategoryDTO>>(await _categoryService.GetBlogCategoriesByModule(Module));
            return Ok(blogs);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAll(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<BlogCategoryDTO>>(await _categoryService.GetBlogCategoriesById(Id)));
        }
        [HttpGet("GarageId/{GarageId}")]
        public async Task<IActionResult> GetAllByGarageId(long GarageId)
        {
            return Ok(_mapper.Map<IEnumerable<BlogCategoryDTO>>(await _categoryService.GetBlogCategoriesByGarageId(GarageId)));
        }

        [HttpPost]
        public async Task<IActionResult> Post(BlogCategoryDTO Model)
        {
            return Ok(_mapper.Map<BlogCategoryDTO>(await _categoryService.AddBlogCategory(_mapper.Map<BlogCategory>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Put(BlogCategoryDTO Model)
        {
            return Ok(_mapper.Map<BlogCategoryDTO>(await _categoryService.UpdateBlogCategory(_mapper.Map<BlogCategory>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<BlogCategoryDTO>(await _categoryService.ArchiveBlogCategory(Id)));
        }
    }
}
