using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleare
    {
        private readonly ILogger<ExceptionHandlerMiddleare> logger;

        public ExceptionHandlerMiddleare(ILogger<ExceptionHandlerMiddleare> logger,
            RequestDelegate next)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                // Log This Exception
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                // Return A custom Error Response

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong! We are looking into resolving this."
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }

        private Task next(HttpContext httpContext)
        {
            throw new NotImplementedException();
        }
    }
}
