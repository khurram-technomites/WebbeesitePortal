using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Handlers
{
    public class CookieRedirectionEvents : CookieAuthenticationEvents
    {
        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {

            RouteValueDictionary RouteValues = context.HttpContext.Request.RouteValues;

            string Area = RouteValues.GetValueOrDefault("area").ToString();

            return base.RedirectToAccessDenied(context);
        }

    }
}
