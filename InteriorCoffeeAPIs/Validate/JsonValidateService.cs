using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace InteriorCoffeeAPIs.Validate
{
    public class JsonValidationService
    {
        private static readonly ConcurrentDictionary<string, JSchema> _schemaCache = new ConcurrentDictionary<string, JSchema>();
        private readonly JSchema _schema;
        private readonly ILogger<JsonValidationService> _logger;

        public JsonValidationService(string schemaFilePath, ILogger<JsonValidationService> logger)
        {
            _logger = logger;
            _schema = _schemaCache.GetOrAdd(schemaFilePath, LoadSchema);
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

        public (bool IsValid, IList<string> Errors) ValidateJson(string jsonString)
        {
            var json = JToken.Parse(jsonString);
            IList<ValidationError> errors;
            bool isValid = json.IsValid(_schema, out errors);

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

            return (isValid, errorMessages);
        }
    }
}
