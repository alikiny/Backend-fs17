using Microsoft.EntityFrameworkCore;
using System.Net;
using Ecommerce.Core.src.Common;
using System.Text.Json;

namespace Ecommerce.WebApi.src.middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unexpected error occurred.");

                // Handle known application-specific exceptions
                if (ex is AppException appEx)
                {
                    // Set the HTTP status code and error message from the AppException
                    context.Response.StatusCode = (int)appEx.StatusCode;
                    var errorResponse = JsonSerializer.Serialize(new { StatusCode = context.Response.StatusCode, Message = appEx.ErrorMessage });

                    // Serialize the error response as JSON and write it to the response body
                    await context.Response.WriteAsync(errorResponse);
                }
                else
                {
                    // For other unhandled exceptions, return a generic 500 Internal Server Error response
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var errorResponse = JsonSerializer.Serialize(new { StatusCode = context.Response.StatusCode, Message = "An unexpected error occurred. Please try again later." });

                    // Serialize the error response as JSON and write it to the response body
                    await context.Response.WriteAsync(errorResponse);
                }
            }
        }
    }
}
