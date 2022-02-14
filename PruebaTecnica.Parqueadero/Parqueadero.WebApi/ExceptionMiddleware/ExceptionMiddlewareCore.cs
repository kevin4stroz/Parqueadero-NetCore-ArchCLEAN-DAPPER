using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Parqueadero.WebApi.ExceptionMiddleware
{
    public class ExceptionMiddlewareCore
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddlewareCore> _logger;

        public ExceptionMiddlewareCore(RequestDelegate next , ILogger<ExceptionMiddlewareCore> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Algo ocurrio mal : {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await httpContext.Response.WriteAsync(new ErrorDetails() { 
                StatusCode = httpContext.Response.StatusCode,
                Message = "Manejador global de errores | ocurrio algo en el servidor"
            }.ToString());
        }
    }

}
