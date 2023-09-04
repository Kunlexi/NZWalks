using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleare
    {
        private readonly ILogger<ExceptionHandlerMiddleare> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleare(ILogger<ExceptionHandlerMiddleare> logger,
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
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
                //Log This Eception
                logger.LogError(ex, $"{errorId} : {ex.Message}" );

                //Return A Custom Error Response

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Somethin went wrong we are looking into resolving this."
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
