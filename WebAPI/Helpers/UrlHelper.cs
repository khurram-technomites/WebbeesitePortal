using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;

namespace WebAPI.Helpers
{
    public class UrlHelper : IUrlHelperService
    {
        private readonly IConfiguration _config;

        public UrlHelper(IConfiguration config)
        {
            _config = config;
        }
        public string GetFormatedURL(string url)
        {
            var ServiceProviderUrl = _config.GetValue<string>("ApiURL");
            return string.Format("{0}{1}", ServiceProviderUrl, url.Replace("~", ""));
        }
    }
}
