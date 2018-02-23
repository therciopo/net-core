using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DutchTreat
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentException(nameof(next));
            _logger = loggerFactory?.CreateLogger<ExceptionMiddleware>() ?? throw new ArgumentException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(HttpStatusCodeException ex)
            {
                if(context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the http status code middleware will not be executed");
                    throw;
                }
                context.Response.Clear();
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = ex.ContenType;

                await context.Response.WriteAsync(ex.Message);
            }
        }            
    }

    [Serializable]
    internal class HttpStatusCodeException : Exception
    {
        public int StatusCode { get; set; }
        public string ContenType { get; set; } = @"text/plain";
        public HttpStatusCodeException(int statusCode)
        {
            StatusCode = statusCode;
        }
        public HttpStatusCodeException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCodeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpStatusCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class ExceptionMiddlewareExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<ExceptionMiddleware>();
    }
}
