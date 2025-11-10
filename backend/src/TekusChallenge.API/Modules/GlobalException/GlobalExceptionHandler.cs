using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Net;
using System.Text.Json;
using Tekus.Transversal;
using TekusChallenge.Application.Common.Exceptions;

namespace TekusChallenge.API.Modules.GlobalException;

public class GlobalExceptionHandler : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly ICorsService _corsService;
    private readonly ICorsPolicyProvider _corsPolicyProvider;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        ICorsService corsService,
        ICorsPolicyProvider corsPolicyProvider)
    {
        _logger = logger;
        _corsService = corsService;
        _corsPolicyProvider = corsPolicyProvider;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationExceptionCustom ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode)
    {
        var policy = await _corsPolicyProvider.GetPolicyAsync(context, "EnableCORS");
        if (policy != null)
        {
            var corsResult = _corsService.EvaluatePolicy(context, policy);
            _corsService.ApplyResult(corsResult, context.Response);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        if (ex is ValidationExceptionCustom validationEx)
        {
            List<BaseError> errors = new List<BaseError>();
            if (validationEx.Errors != null && validationEx.Errors.Count() > 0)
            {
                foreach (var error in validationEx.Errors)
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
                errors = new List<BaseError>
                {
                    new BaseError
                    {
                        ErrorMessage = validationEx.Message,
                        PropertyMessage = "ValidationError"
                    }
                };
            }

            var response = new Response<Object>
            {
                Message = "Validation errors",
                Errors = errors
            };
            await JsonSerializer.SerializeAsync(context.Response.Body, response, JsonOptions);
        }
        else
        {
            string message = ex.Message;
            _logger.LogError($"Exception details: {message}");

            var response = new Response<Object>()
            {
                Message = message,
            };
            await JsonSerializer.SerializeAsync(context.Response.Body, response, JsonOptions);
        }
    }
}
