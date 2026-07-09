using System;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Contentstack.Utils.Converters
{
    // Resolves IEmbeddedObject to EmbeddedObject during deserialization.
    // CanConvert uses exact type match so customer-defined subclasses are not intercepted.
    public class EmbeddedObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(IEmbeddedObject);

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var jo = JObject.Load(reader);
            var result = new EmbeddedObject();
            serializer.Populate(jo.CreateReader(), result);
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotSupportedException(
                "EmbeddedObjectConverter is read-only. Serialize EmbeddedObject directly.");
    }
}
