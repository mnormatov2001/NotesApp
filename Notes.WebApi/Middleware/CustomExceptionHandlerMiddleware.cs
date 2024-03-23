using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Common.Exceptions;
using Serilog;

namespace Notes.WebApi.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occured while handling HTTP request");
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        string result;
        switch (exception)
        {
            case ValidationException ex:
                statusCode = StatusCodes.Status400BadRequest;
                var errors = ex.Errors.Select(error => error.ToString()).ToArray();
                var problem = new ValidationProblemDetails(
                    new Dictionary<string, string[]> { { "ValidationErrors", errors } });
                result = JsonSerializer.Serialize(problem);
                break;

            case NotFoundException ex:
                statusCode = StatusCodes.Status404NotFound;
                var details = new ProblemDetails
                {
                    Title = "The requested content was not found.",
                    Detail = ex.Message,
                    Status = statusCode
                };
                result = JsonSerializer.Serialize(details);
                break;

            default:
                var problemDetails = new ProblemDetails
                {
                    Title = "Something went wrong.",
                    Detail = "An unexpected error occurred, we apologize.",
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "InternalServerError",
                };
                result = JsonSerializer.Serialize(problemDetails);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(result);
    }
}