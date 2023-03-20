using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Helpers
{
    public class SwaggerAuthMiddleware
    {
        private readonly IConfiguration _config;
        private string _username = string.Empty, _password = string.Empty;
        private readonly RequestDelegate _next;
        public SwaggerAuthMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;

            _username = _config.GetValue<string>("SwaggerUsername");
            _password = _config.GetValue<string>("SwaggerPassword");
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    // Get the credentials from request header
                    var header = AuthenticationHeaderValue.Parse(authHeader);
                    var inBytes = Convert.FromBase64String(header.Parameter);
                    var credentials = Encoding.UTF8.GetString(inBytes).Split(':');
                    var username = credentials[0];
                    var password = credentials[1];
                    // validate credentials
                    if (username.Equals(_username)
                      && password.Equals(_password))
                    {
                        await _next.Invoke(context).ConfigureAwait(false);
                        return;
                    }
                }
                context.Response.Headers["WWW-Authenticate"] = "Basic";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await _next.Invoke(context).ConfigureAwait(false);
            }
        }
    }
}
