using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices
{
    public interface ITestTableService
    {
        Task<TestTable> AddAsync(TestTable Entity);
        Task<TestTable> UpdateAsync(TestTable Entity);
        //Task<IEnumerable<TestTable>> GetAllAsync();
        Task<IEnumerable<TestTable>> GetByIdAsync(int Id);
        Task ArchiveAsync(int Id);
    }
}
