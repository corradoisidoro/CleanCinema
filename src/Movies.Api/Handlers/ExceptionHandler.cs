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
            _ => CreateProblemDetails(StatusCodes.Status500InternalServerError, "Internal Server Error",
                "An unexpected error occured")
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