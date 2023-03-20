using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Interfaces.IServices
{
    public interface IGenericsService<T>
    {
        Task<IEnumerable<T>> GetTs(string PropertyName, string value);
    }
}
