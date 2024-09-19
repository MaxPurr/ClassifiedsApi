using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using ClassifiedsApi.Contracts.Common;

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

    private static string GetJson(ApiError apiError)
    {
        return JsonSerializer.Serialize(apiError, JsonSerializerOptions);
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
            var apiError = CreateApiErrorByEnvironment(exception, environment);
            await context.Response.WriteAsync(GetJson(apiError));
        }
    }

    private ApiError CreateApiErrorByEnvironment(Exception exception, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            return new ApiError()
            {
                Message = exception.Message,
                Description = exception.StackTrace,
                Code = ((int)(HttpStatusCode.InternalServerError)).ToString(),
            };
        }
        return CreateApiError(exception);
    }

    private ApiError CreateApiError(Exception exception)
    {
        return exception switch
        {
            _ => new ApiError()
            {
                Message = "Произошла непредвиденая ошибка.",
                Code = ((int)(HttpStatusCode.InternalServerError)).ToString()
            }
        };
    }
    
    private int GetStatusCode(Exception exception)
    {
        HttpStatusCode statusCode = exception switch
        {
            _ => HttpStatusCode.InternalServerError,
        };
        return (int)statusCode;
    }
}