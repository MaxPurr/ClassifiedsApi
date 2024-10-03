using System;
using System.IO;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Accounts;
using ClassifiedsApi.AppServices.Exceptions.Common;
using ClassifiedsApi.Contracts.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ClassifiedsApi.Api.Middlewares;

/// <summary>
/// Промежуточное ПО для обработки ошибок.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };
    
    private readonly RequestDelegate _next;
    
    /// <summary>
    /// Инициализирует экземпляр <see cref="ExceptionHandlingMiddleware"/>.
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
            context.Response.StatusCode = GetStatusCode(exception);
            var apiError = CreateApiErrorByEnvironment(exception, context, environment);
            await context.Response.WriteAsync(Serialize(apiError));
        }
    }

    private static string Serialize(ApiError apiError)
    {
        return JsonSerializer.Serialize(apiError, JsonSerializerOptions);
    }
    
    private static int GetStatusCode(Exception exception)
    {
        var statusCode = exception switch
        {
            EntityNotFoundException => HttpStatusCode.NotFound,
            IncorrectCredentialsException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError,
        };
        return (int)statusCode;
    }
    
    private static ApiError CreateApiErrorByEnvironment(Exception exception, HttpContext context, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            return new ApiError()
            {
                Message = exception.Message,
                Code = ((int)(HttpStatusCode.InternalServerError)).ToString(),
                Description = exception.StackTrace,
                TraceId = context.TraceIdentifier
            };
        }
        return CreateApiError(exception, context);
    }

    private static ApiError CreateApiError(Exception exception, HttpContext context)
    {
        return exception switch
        {
            EntityNotFoundException e => new ApiError()
            {
                Message = e.Message,
                Code = ((int)HttpStatusCode.NotFound).ToString(),
                TraceId = context.TraceIdentifier
            },
            IncorrectCredentialsException => new ApiError()
            {
                Message = "Неверное имя пользователя или пароль",
                Code = ((int)HttpStatusCode.Unauthorized).ToString(),
                TraceId = context.TraceIdentifier
            },
            _ => new ApiError()
            {
                Message = "Произошла непредвиденая ошибка.",
                Code = ((int)HttpStatusCode.InternalServerError).ToString(),
                TraceId = context.TraceIdentifier
            }
        };
    }
}