using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Interfaces.IServices
{
    public interface IUrlHelperService
    {
        string GetFormatedURL(string url);
    }
}
