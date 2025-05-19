using System.Net;
using System.Text.Json;

namespace PruebaTecnicaMakers.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex is Domain.Exceptions.BusinessException
                    ? (int)HttpStatusCode.BadRequest
                    : (int)HttpStatusCode.InternalServerError;
                var response = new { error = ex.Message };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
