using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories;
using WebAPI.Interfaces.IServices;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class TestTableService : ITestTableService
    {
        private readonly ITestTableRepo _repo;

        public TestTableService(ITestTableRepo repo)
        {
            _repo = repo;
        }

        public async Task<TestTable> AddAsync(TestTable Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        //public async Task<IEnumerable<TestTable>> GetAllAsync()
        //{
        //    //return await _repo.GetAllAsync();
        //}

        public async Task<IEnumerable<TestTable>> GetByIdAsync(int Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, null);
        }

        public async Task<TestTable> UpdateAsync(TestTable Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }

        public async Task ArchiveAsync(int Id)
        {
            await _repo.ArchiveAsync(Id);
        }
    }
}
