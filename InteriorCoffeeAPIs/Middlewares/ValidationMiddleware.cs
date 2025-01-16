using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs;
using InteriorCoffeeAPIs.Validate;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

namespace InteriorCoffeeAPIs.Middlewares
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationMiddleware> _logger;
        private readonly IDictionary<string, JsonValidationService> _validationServices;
        private readonly IDictionary<string, string> _schemaPathMappings;
        private readonly List<string> _softDeleteEndpoints = new List<string>
        {
            ApiEndPointConstant.Account.SoftDeleteAccountEndpoint,
            ApiEndPointConstant.Product.SoftDeleteProductEndpoint,
            // Add other soft delete endpoints here
        };

        public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger, IDictionary<string, JsonValidationService> validationServices)
        {
            _next = next;
            _logger = logger;
            _validationServices = validationServices;
            _schemaPathMappings = new Dictionary<string, string>
            {
                { "account", "AccountValidate" },
                { "product", "ProductValidate" }
            };
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Patch)
            {
                context.Request.EnableBuffering();
                var body = await ReadRequestBodyAsync(context);

                var schemaFilePath = GetSchemaFilePath(context.Request.Path);
                if (schemaFilePath != null && _validationServices.TryGetValue(schemaFilePath, out var jsonValidationService))
                {
                    bool isUpdate = context.Request.Method == HttpMethods.Patch;

                    if (ShouldSkipValidation(context, body))
                    {
                        await _next(context);
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        if (!await ValidateRequestBodyAsync(context, body, jsonValidationService, isUpdate))
                        {
                            return;
                        }
                    }
                    else
                    {
                        await HandleEmptyBodyAsync(context);
                        return;
                    }
                }
            }

            await _next(context);
        }

        private async Task<string> ReadRequestBodyAsync(HttpContext context)
        {
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
                return body;
            }
        }

        private bool ShouldSkipValidation(HttpContext context, string body)
        {
            return string.IsNullOrWhiteSpace(body) && _softDeleteEndpoints.Any(endpoint => context.Request.Path.Equals(endpoint, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<bool> ValidateRequestBodyAsync(HttpContext context, string body, JsonValidationService jsonValidationService, bool isUpdate)
        {
            try
            {
                var (isValid, errors) = jsonValidationService.ValidateJson(body, isUpdate);

                if (!isValid)
                {
                    _logger.LogError("Validation failed for path {Path}: {Errors}", context.Request.Path, string.Join(", ", errors));
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    ErrorDTO errorModel = new ErrorDTO()
                    {
                        Error = (List<string>)errors,
                        StatusCode = StatusCodes.Status400BadRequest,
                        TimeStamp = DateTime.UtcNow
                    };
                    await context.Response.WriteAsJsonAsync(errorModel);
                    //await context.Response.WriteAsJsonAsync(new { Errors = errors });
                    return false;
                }
            }
            catch (JsonReaderException ex)
            {
                _logger.LogError(ex, "Invalid JSON format");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                ErrorDTO errorModel = new ErrorDTO()
                {
                    Error = new List<string> { "Invalid JSON format" },
                    StatusCode = StatusCodes.Status400BadRequest,
                    TimeStamp = DateTime.UtcNow
                };
                await context.Response.WriteAsJsonAsync(errorModel);
                //await context.Response.WriteAsJsonAsync(new { Errors = new[] { "Invalid JSON format" } });


                return false;
            }

            return true;
        }

        private async Task HandleEmptyBodyAsync(HttpContext context)
        {
            if (_softDeleteEndpoints.Any(endpoint => context.Request.Path.Equals(endpoint, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
            }
            else
            {
                _logger.LogError("Request body is empty for path {Path}", context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                ErrorDTO errorModel = new ErrorDTO()
                {
                    Error = new List<string> { "Request body cannot be empty" },
                    StatusCode = StatusCodes.Status400BadRequest,
                    TimeStamp = DateTime.UtcNow
                };
                await context.Response.WriteAsJsonAsync(errorModel);
                //await context.Response.WriteAsJsonAsync(new { Errors = new[] { "Request body cannot be empty" } });
            }
        }


        private string GetSchemaFilePath(string requestPath)
        {
            foreach (var mapping in _schemaPathMappings)
            {
                if (requestPath.Contains(mapping.Key, StringComparison.OrdinalIgnoreCase) && !requestPath.Contains("categories"))
                {
                    return mapping.Value;
                }
            }
            return null;
        }
    }
}
