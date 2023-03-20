using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Interfaces.IRepositories
{
    public interface IGenericsRepo<T> : IRepository<T>
    {
    }
}
