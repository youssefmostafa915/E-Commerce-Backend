using E_Commerce.Api.Middleware;
using Microsoft.AspNetCore.Builder;

namespace E_Commerce.Api.Extensions;

/// <summary>
/// Extension methods for configuring middleware in the application pipeline.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Adds the global exception handling middleware to the application pipeline.
    /// This should be called early in the pipeline, before other middleware.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder with exception middleware added.</returns>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}