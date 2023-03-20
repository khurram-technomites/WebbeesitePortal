using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;

namespace WebApp.Handlers
{
    public class PortalDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<PortalDelegatingHandler> _logger;
        private readonly IJSONHelper _jsonHelper;
        private readonly ITokenManager _tokenManager;

        public PortalDelegatingHandler(ILogger<PortalDelegatingHandler> logger, IJSONHelper jsonHelper, ITokenManager tokenManager)
        {
            _logger = logger;
            _jsonHelper = jsonHelper;
            _tokenManager = tokenManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                             CancellationToken cancellationToken)
        {
            HttpResponseMessage Response;

            try
            {
                Response = await base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to run http query {RequestUri}", request.RequestUri);

                ErrorDetails Err = new ErrorDetails(HttpStatusCode.InternalServerError, ex.Message,
                                                    (ex.InnerException != null) ? ex.InnerException.Message : "");

                throw new ApiException(Err.ToString());
            }

            if (Response.IsSuccessStatusCode)
                return Response;
            else
            {
                // 404 error for endpoint not found coming here.

                ErrorDetails Err = new ErrorDetails();

                var data = await Response.Content.ReadAsStringAsync();

                switch (Response.StatusCode)
                {
                    // All unhandled exceptions. Error will be of type ErrorDetails
                    case HttpStatusCode.InternalServerError:
                        {
                            Err = JsonConvert.DeserializeObject<ErrorDetails>(data);
                            break;
                        }

                    case HttpStatusCode.BadRequest:
                        {
                            //if (_jsonHelper.IsValidJSON(data))
                            //{
                            //    BadRequestError BadError = JsonConvert.DeserializeObject<BadRequestError>(data);

                            //    Err.Message = BadError.Errors.Items.FirstOrDefault();
                            //}
                            //else
                            //{
                            Err = new ErrorDetails(Response.StatusCode, data, "");
                            //}

                            break;
                        }

                    case HttpStatusCode.Unauthorized:
                        {
                            Err = new ErrorDetails(Response.StatusCode, "API Unauthorized", "");
                            break;
                        }

                    default:
                        {
                            Err = new ErrorDetails(Response.StatusCode, (String.IsNullOrEmpty(data)) ? Response.ReasonPhrase : data, "");
                            break;
                        }
                }

                // Pages are dependant on Err.ToString(). Don't change this.
                throw new ApiException(Err.ToString());
            }
        }

    }
}
