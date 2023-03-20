using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Middleware
{
    public class RedirectingMiddleware
    {
        private readonly RequestDelegate _next;
        public RedirectingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext, IAntiforgery antiforgery)
        {

            // Redirect to login if user is not authenticated. This instruction is neccessary for JS async calls, otherwise everycall will return unauthorized without explaining why
            if (!httpContext.User.Identity.IsAuthenticated && httpContext.Request.Path.Value != "/Account/Login")
            {
                httpContext.Response.Redirect("/Account/Login");
            }

            // Move forward into the pipeline
            await _next(httpContext);
        }
    }
}
