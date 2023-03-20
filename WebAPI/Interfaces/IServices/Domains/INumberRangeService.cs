using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface INumberRangeService
    {
        Task<string> GetNextRange(string prefix);
        Task<string> GetNumberRangeByName(string name);
    }
}
