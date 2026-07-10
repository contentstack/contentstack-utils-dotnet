using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;

namespace Contentstack.Utils.Converters
{
    // Resolves IEmbeddedObject to EmbeddedObject during deserialization.
    // JsonConverter<IEmbeddedObject> is selected only for the bare IEmbeddedObject type
    // (CanConvert matches typeof(IEmbeddedObject) exactly), so the concrete EmbeddedObject
    // and customer-defined subclasses are not intercepted.
    public class EmbeddedObjectConverter : JsonConverter<IEmbeddedObject>
    {
        public override IEmbeddedObject Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            // Deserialize into the concrete type. EmbeddedObject != IEmbeddedObject, so this
            // converter is not re-entered and the default object converter handles the mapping
            // (including [JsonExtensionData] for custom fields).
            return JsonSerializer.Deserialize<EmbeddedObject>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, IEmbeddedObject value,
            JsonSerializerOptions options)
            => throw new NotSupportedException(
                "EmbeddedObjectConverter is read-only. Serialize EmbeddedObject directly.");
    }
}
