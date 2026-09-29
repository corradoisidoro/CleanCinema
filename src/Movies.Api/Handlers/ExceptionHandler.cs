using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Movies.Contracts.Exceptions;

namespace Movies.Api.Handlers;

public class ExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // The client hung up before the response was written. There is nobody
        // left to answer, and writing to the socket would only throw again.
        // Reported as handled so the middleware does not log it as a crash.
        if (exception is OperationCanceledException && httpContext.RequestAborted.IsCancellationRequested)
        {
            _logger.LogDebug("Request aborted by the client for {Method} {Path}",
                httpContext.Request.Method, httpContext.Request.Path);
            return true;
        }

        var problemDetails = CreateProblemDetails(exception);
        
        if (problemDetails.Status >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception while processing {Method} {Path}",
                httpContext.Request.Method, httpContext.Request.Path);
        }
        else
        {
            _logger.LogWarning(exception, "Request failed with status {StatusCode} for {Method} {Path}",
                problemDetails.Status, httpContext.Request.Method, httpContext.Request.Path);
        }

        // Bytes are already on the wire, so the status line can no longer be
        // changed. Report as unhandled and let the server tear the connection
        // down rather than writing a second, conflicting response.
        if (httpContext.Response.HasStarted)
        {
            return false;
        }

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
    
    private static ProblemDetails CreateProblemDetails(Exception exception)
    {
        ProblemDetails problemDetails = exception switch
        {
            NotFoundException => CreateProblemDetails(StatusCodes.Status404NotFound, "Not Found", exception.Message),
            CustomValidationException => CreateProblemDetails(StatusCodes.Status400BadRequest, "Validation error",
                "One or more validation errors occurred"),
            // Minimal APIs throw this when the body cannot be bound - malformed
            // JSON, a missing body, or a payload that is too large. It already
            // carries the right status (400/404/413/408), so without this arm a
            // client sending bad JSON would be told the server broke.
            BadHttpRequestException badRequest => CreateProblemDetails(badRequest.StatusCode, "Bad Request",
                "The request could not be read. Check that the body is well-formed JSON matching this endpoint."),
            _ => CreateProblemDetails(StatusCodes.Status500InternalServerError, "Internal Server Error",
                "An unexpected error occurred")
        };

        if (exception is CustomValidationException customValidationException)
        {
            problemDetails.Extensions["errors"] = customValidationException.ValidationErrors;
        }

        return problemDetails;
    }

    private static ProblemDetails CreateProblemDetails(int status, string title, string detail)
    {
        return new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
        };
    }
}