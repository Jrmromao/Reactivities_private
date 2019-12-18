using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Appication.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace API.Middleware {
    public class ErrorHandlingMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware (RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger) {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke (HttpContext context) {
            try {

                await _next (context);

            } catch (Exception ex) {
                await HandleExceptionAsync (context, ex, _logger);
            }
        }

        private async Task HandleExceptionAsync (HttpContext context,
            Exception ex,
            ILogger<ErrorHandlingMiddleware> logger) {

            object errors = null;

            switch (ex) {
                // HTTP exceptions
                case RestException re:
                    logger.LogError (ex, "TEST ERROR");
                    errors = re.Error;
                    context.Response.StatusCode = (int) re.Code;
                    break;
                    // normal exceptiona 
                case Exception e:
                    logger.LogError (ex, "SERVER EROOR");
                    errors = string.IsNullOrWhiteSpace (e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;

            }
            context.Response.ContentType = "application/json";
            if (errors != null) {
                var result = JsonSerializer.Serialize (new {
                    errors
                });

                await context.Response.WriteAsync (result);
            }
        }
    }
}