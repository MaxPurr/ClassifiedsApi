using System;
using System.Net;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Common;
using ClassifiedsApi.AppServices.Extensions;
using ClassifiedsApi.Contracts.Common.Errors;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Context;

namespace ClassifiedsApi.Api.Middlewares;

/// <summary>
/// Промежуточное ПО для обработки ошибок.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    private static readonly ApiError DefaultApiError = new ApiError{
        Message = "Произошла непредвиденная ошибка.",
        Code = StatusCodes.Status500InternalServerError.ToString(),
    };
    
    private const string LogTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode}";
    
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="ExceptionHandlingMiddleware"/>.
    /// </summary>
    /// <param name="next">Делегат запроса.</param>
    /// <param name="logger">Логгер.</param>
    /// <exception cref="ArgumentNullException">Выбрасывает исключение если параметр next равен null.</exception>
    public ExceptionHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger;
    }
    
    public async Task InvokeAsync(
        HttpContext context, 
        IHostEnvironment environment, 
        IServiceProvider serviceProvider)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            LogError(exception, context, statusCode);
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            var apiError = CreateApiErrorByEnvironment(exception, context, environment);
            await context.Response.WriteAsync(Serialize(apiError));
        }
    }

    private void LogError(Exception exception, HttpContext context, HttpStatusCode statusCode)
    {
        var traceId = context.TraceIdentifier;
        var userName = context.User.Identity?.Name ?? string.Empty;
        var connection = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var displayUrl = context.Request.GetDisplayUrl();
        
        using (LogContext.PushProperty("Request.TraceId", traceId))
        using (LogContext.PushProperty("Request.UserName", userName))
        using (LogContext.PushProperty("Request.Connection", connection))
        using (LogContext.PushProperty("Request.DisplayUrl", displayUrl))
        {
            _logger.LogError(
                exception, 
                LogTemplate,
                context.Request.Method,
                context.Request.Path.ToString(),
                (int)statusCode);
        }
    }

    private static string Serialize(ApiError apiError)
    {
        return JsonConvert.SerializeObject(apiError, JsonSerializerSettings);
    }
    
    private static HttpStatusCode GetStatusCode(Exception exception)
    {
        var statusCode = exception switch
        {
            ApiException apiException => apiException.StatusCode,
            ValidationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };
        return statusCode;
    }
    
    private static ApiError CreateApiErrorByEnvironment(Exception exception, HttpContext context, IHostEnvironment environment)
    {
        var apiError = environment.IsDevelopment()
            ? CreateDevelopmentApiError(exception) 
            : CreateProductionApiError(exception);
        apiError.TraceId = context.TraceIdentifier;
        return apiError;
    }

    private static ApiError CreateDevelopmentApiError(Exception exception)
    {
        return new ApiError
        {
            Message = exception.Message,
            Code = StatusCodes.Status500InternalServerError.ToString(),
            Description = exception.StackTrace
        };
    }

    private static ApiError CreateProductionApiError(Exception exception)
    {
        return exception switch
        {
            ApiException ex => ex.ToApiError(),
            ValidationException ex => ex.ToValidationApiError(),
            _ => DefaultApiError
        };
    }
}