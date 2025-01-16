using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text.Json;

namespace InteriorCoffeeAPIs.Validate
{
    public class JsonValidationService
    {
        private static readonly ConcurrentDictionary<string, JSchema> _schemaCache = new ConcurrentDictionary<string, JSchema>();
        private readonly JSchema _schema;
        private readonly ILogger<JsonValidationService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonValidationService(string schemaFilePath, ILogger<JsonValidationService> logger)
        {
            _logger = logger;
            _schema = _schemaCache.GetOrAdd(schemaFilePath, LoadSchema);
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
            };
            _logger.LogInformation($"Schema loaded: {_schema.ToString()}");
        }

        private JSchema LoadSchema(string schemaFilePath)
        {
            using (var textReader = File.OpenText(schemaFilePath))
            using (var jsonReader = new JsonTextReader(textReader))
            {
                return JSchema.Load(jsonReader);
            }
        }

        public (bool IsValid, IList<string> Errors) ValidateJson(string jsonString, bool isUpdate = false)
        {
            var json = JToken.Parse(jsonString);
            IList<ValidationError> errors;
            bool isValid;

            var schemaToUse = _schema;

            if (isUpdate)
            {
                // Create a copy of the schema without required fields for updates
                var updateSchema = JSchema.Parse(_schema.ToString());
                updateSchema.Required.Clear();
                schemaToUse = updateSchema;
            }

            isValid = json.IsValid(schemaToUse, out errors);

            IList<string> errorMessages = new List<string>();
            if (!isValid)
            {
                foreach (var error in errors)
                {
                    string errorMessage = $"Field {error.Path} is invalid: {error.Message}";
                    errorMessages.Add(errorMessage);
                    _logger.LogError(errorMessage); // Log the error message
                }
            }

            _logger.LogInformation($"Validation result: {isValid}");
            if (errorMessages.Count > 0)
            {
                _logger.LogInformation($"Validation errors: {string.Join(", ", errorMessages)}");
            }

            return (isValid, errorMessages);
        }


    }
}
