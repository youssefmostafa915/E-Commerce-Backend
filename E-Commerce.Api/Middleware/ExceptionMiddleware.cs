using System.Net;
using System.Text.Json;
using E_Commerce.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Api.Middleware;

/// <summary>
/// Global exception handling middleware.
/// Catches unhandled exceptions and returns standardized error responses.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    /// <summary>
    /// Initializes the exception middleware.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">Logger for recording exceptions.</param>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Processes the HTTP request and handles any exceptions.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Continue processing the request
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "An unhandled exception occurred while processing the request.");

            // Handle the exception and return appropriate response
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles the exception by determining the appropriate HTTP status code and message.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="exception">The exception that was thrown.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Default values for unknown exceptions
        var statusCode = HttpStatusCode.InternalServerError;
        var errorCode = "INTERNAL_ERROR";
        var message = "An unexpected error occurred. Please try again later.";

        // Handle specific exception types
        switch (exception)
        {
            case InsufficientStockException stockEx:
                // Insufficient stock errors (must come before DomainException)
                statusCode = HttpStatusCode.BadRequest;
                errorCode = "INSUFFICIENT_STOCK";
                message = stockEx.Message;
                break;

            case DomainException domainEx:
                // Domain-specific business logic errors
                statusCode = HttpStatusCode.BadRequest;
                errorCode = "DOMAIN_ERROR";
                message = domainEx.Message;
                break;

            case ArgumentException argEx:
                // Invalid argument errors
                statusCode = HttpStatusCode.BadRequest;
                errorCode = "INVALID_ARGUMENT";
                message = argEx.Message;
                break;

            case UnauthorizedAccessException:
                // Authorization errors
                statusCode = HttpStatusCode.Unauthorized;
                errorCode = "UNAUTHORIZED";
                message = "You are not authorized to perform this action.";
                break;

            case KeyNotFoundException:
                // Resource not found errors
                statusCode = HttpStatusCode.NotFound;
                errorCode = "NOT_FOUND";
                message = "The requested resource was not found.";
                break;

            // Add more specific exception types as needed
        }

        // Set the response
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        // Create the error response
        var errorResponse = new
        {
            IsSuccess = false,
            Message = message,
            ErrorCode = errorCode,
            Timestamp = DateTime.UtcNow,
            // In development, include stack trace for debugging
            StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                ? exception.StackTrace
                : null
        };

        // Serialize and write the response
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}