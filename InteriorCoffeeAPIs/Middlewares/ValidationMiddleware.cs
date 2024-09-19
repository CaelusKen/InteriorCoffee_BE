using InteriorCoffeeAPIs.Validate;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace InteriorCoffeeAPIs.Middlewares
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationMiddleware> _logger;
        private readonly IDictionary<string, JsonValidationService> _validationServices;
        private readonly IDictionary<string, string> _schemaPathMappings;

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
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;

                    var schemaFilePath = GetSchemaFilePath(context.Request.Path);
                    if (schemaFilePath != null && _validationServices.TryGetValue(schemaFilePath, out var jsonValidationService))
                    {
                        var (isValid, errors) = jsonValidationService.ValidateJson(body);

                        if (!isValid)
                        {
                            _logger.LogError("Validation failed for path {Path}: {Errors}", context.Request.Path, string.Join(", ", errors));
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsJsonAsync(new { Errors = errors });
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }

        private string GetSchemaFilePath(string requestPath)
        {
            foreach (var mapping in _schemaPathMappings)
            {
                if (requestPath.Contains(mapping.Key, StringComparison.OrdinalIgnoreCase))
                {
                    return mapping.Value;
                }
            }
            return null;
        }
    }

}
