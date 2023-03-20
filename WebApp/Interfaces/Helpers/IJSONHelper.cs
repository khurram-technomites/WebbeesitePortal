using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.Helpers
{
    public interface IJSONHelper
    {
        public bool IsValidJSON(string data);
    }
}
