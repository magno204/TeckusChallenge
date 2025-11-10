using System.Net;
using System.Text.Json;
using Tekus.Transversal;
using TekusChallenge.Application.Common.Exceptions;

namespace TekusChallenge.API.Modules.GlobalException;

public class GlobalExceptionHandler : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationExceptionCustom ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            List<BaseError> errors = new List<BaseError>();
            if (ex.Errors != null && ex.Errors.Count() > 0)
            {
                foreach (var error in ex.Errors)
                {
                    errors.Add(new BaseError
                    {
                        PropertyMessage = error.PropertyMessage,
                        ErrorMessage = error.ErrorMessage
                    });
                }
            }
            else
            {
                errors =
                [
                    new BaseError
                    {
                        ErrorMessage = ex.Message,
                        PropertyMessage = "ValidationError"
                    },
                    ];
            }

            var response = new Response<Object>
            {
                Message = "Validation errors",
                Errors = errors
            };
            await JsonSerializer.SerializeAsync(context.Response.Body,
                response,
                JsonOptions);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            _logger.LogError($"Exception details: {message}");

            var response = new Response<Object>()
            {
                Message = message,
            };
            await JsonSerializer.SerializeAsync(context.Response.Body, response, JsonOptions);
        }
    }
}
