using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Utils
{
    public static class JsonUtil
    {
        public static JsonElement MergeJsonElements(JsonElement original, JsonElement update)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();

                    foreach (var property in original.EnumerateObject())
                    {
                        if (update.TryGetProperty(property.Name, out var updatedProperty))
                        {
                            writer.WritePropertyName(property.Name);
                            updatedProperty.WriteTo(writer);
                        }
                        else
                        {
                            property.WriteTo(writer);
                        }
                    }

                    foreach (var property in update.EnumerateObject())
                    {
                        if (!original.TryGetProperty(property.Name, out _))
                        {
                            writer.WritePropertyName(property.Name);
                            property.Value.WriteTo(writer);
                        }
                    }

                    writer.WriteEndObject();
                }

                stream.Position = 0;
                using (var document = JsonDocument.Parse(stream))
                {
                    return document.RootElement.Clone();
                }
            }
        }
    }
}
