using System;
using ClassifiedsApi.AppServices.Common.Services;
using Serilog.Context;

namespace ClassifiedsApi.Infrastructure.Services.Logging;

/// <inheritdoc />
public class StructuralLoggingService : IStructuralLoggingService
{
    /// <inheritdoc />
    public IDisposable PushProperty(string name, object value, bool destructureObjects = false)
    {
        return LogContext.PushProperty(name, value, destructureObjects);
    }
}