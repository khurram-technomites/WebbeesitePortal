using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApp.ErrorHandling
{

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
                        if (contextFeature.Error is ApiException)
                        {
                            await context.Response.WriteAsync(contextFeature.Error.Message);
                        }
                        else
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                            ErrorDetails ErrDetails = new ErrorDetails()
                            {
                                StatusCode = HttpStatusCode.InternalServerError,
                                Message = contextFeature.Error.Message
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
