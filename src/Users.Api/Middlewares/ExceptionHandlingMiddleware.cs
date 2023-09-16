using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Users.Api.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var problem = new ValidationProblemDetails(ex.Errors.ToDictionary(
                x => x.PropertyName,
                x => new List<string> { x.ErrorMessage }.ToArray()));
            problem.Status = (int)HttpStatusCode.BadRequest;
            problem.Type = "Validation exception";
            problem.Title = "One or more validation errors occurred.";

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problem = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = "Server error",
                Title = "Server error",
                Detail = "An internal server error has occurred."
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(problem);

        }
    }
}
