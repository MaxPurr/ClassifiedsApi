using System;
using System.Net;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Common;
using ClassifiedsApi.AppServices.Extensions;
using ClassifiedsApi.Contracts.Common.Errors;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
    
    private readonly RequestDelegate _next;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="ExceptionHandlingMiddleware"/>.
    /// </summary>
    /// <param name="next">Делегат запроса.</param>
    /// <exception cref="ArgumentNullException">Выбрасывает исключение если параметр next равен null.</exception>
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }
    
    public async Task InvokeAsync(HttpContext context, IHostEnvironment environment, IServiceProvider serviceProvider)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)GetStatusCode(exception);
            var apiError = CreateApiErrorByEnvironment(exception, context, environment);
            await context.Response.WriteAsync(Serialize(apiError));
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
        var apiError = environment.IsDevelopment() && false
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