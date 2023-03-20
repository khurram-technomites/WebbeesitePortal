using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestTableService _service;
        private readonly FougitoContext _context;

        public TestController(ITestTableService service, FougitoContext context)
        {
            _service = service;
            _context = context;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    return Ok(await _context.TestModel.ToListAsync());
        //}

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            return Ok(await _service.GetByIdAsync(Id));
        }

        [HttpPost]
        public async Task<IActionResult> Insert(TestTable Entity)
        {
            return Ok(await _service.AddAsync(Entity));
        }

        [HttpPut]
        public async Task<IActionResult> Update(TestTable Entity)
        {
            return Ok(await _service.UpdateAsync(Entity));
        }

        [HttpDelete]
        public async Task<IActionResult> Delate(int Id)
        {
            await _service.ArchiveAsync(Id);
            return Ok();
        }
    }
}
