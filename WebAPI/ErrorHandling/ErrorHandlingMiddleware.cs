using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Helpers;

namespace WebAPI.ErrorHandling
{
    [AllowAnonymous]
    public static class ErrorHandlingMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        if (contextFeature.Error is APIException)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Conflict;

                            await context.Response.WriteAsync(JsonConvert.SerializeObject(contextFeature.Error.Message));
                        }
                        else
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                            ErrorDetails ErrDetails = new()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = contextFeature.Error.Message
                                //Message = "Something went wrong"
                            };

                            if (contextFeature.Error.InnerException != null)
                                ErrDetails.InnerMessage = contextFeature.Error.InnerException.Message;

                            await context.Response.WriteAsync(ErrDetails.ToString());
                        }
                    }
                });
            });
        }
    }
}
