using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OrderFlow.Application.DTOs.Common;
using OrderFlow.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace OrderFlow.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            NotFoundException => new
            {
                succeeded = false,
                message = exception.Message,
                errors = new[] { exception.Message },
                statusCode = (int)HttpStatusCode.NotFound
            },
            BusinessRuleException => new
            {
                succeeded = false,
                message = exception.Message,
                errors = new[] { exception.Message },
                statusCode = (int)HttpStatusCode.BadRequest
            },
            DomainException => new
            {
                succeeded = false,
                message = exception.Message,
                errors = new[] { exception.Message },
                statusCode = (int)HttpStatusCode.BadRequest
            },
            _ => new
            {
                succeeded = false,
                message = "Ocorreu um erro interno no servidor.",
                errors = new[] { "Erro interno. Tente novamente mais tarde." },
                statusCode = (int)HttpStatusCode.InternalServerError
            }
        };

        context.Response.StatusCode = response.statusCode;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}