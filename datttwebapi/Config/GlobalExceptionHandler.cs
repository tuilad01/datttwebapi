using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace datttwebapi.Config
{
    public class GlobalExceptionHandler
    {
        private readonly ILogger _logger;

        public GlobalExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(HttpContext context)
        {
            var exception = context.Features
            .Get<IExceptionHandlerFeature>()?
            .Error;

            if (exception == null)
                return;

            _logger.LogError(exception, exception.Message);

            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case BusinessValidationException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = ex.Code,
                        message = ex.Message
                    });
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "server_error",
                        message = "An unexpected error occurred"
                    });
                    break;
            }
        }

    }
}
