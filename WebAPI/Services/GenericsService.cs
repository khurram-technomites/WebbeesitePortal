using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories;
using WebAPI.Interfaces.IServices;

namespace WebAPI.Services
{
    public class GenericsService<T> : IGenericsService<T>
    {
        private readonly IGenericsRepo<T> _genericsRepo;

        public GenericsService(IGenericsRepo<T> genericsRepo)
        {
            _genericsRepo = genericsRepo;
        }
        public async Task<IEnumerable<T>> GetTs(string PropertyName, string value)
        {
            return await _genericsRepo.GetGenericAsync(PropertyName, value);
        }
    }
}
