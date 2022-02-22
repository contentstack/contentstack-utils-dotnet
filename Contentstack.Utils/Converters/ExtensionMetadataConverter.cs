using System;
using System.Collections.Generic;
using Contentstack.Utils.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Contentstack.Utils.Converters
{
    public class ExtensionMetadataConverter : JsonConverter<ExtensionMetadata>
    {
        public override ExtensionMetadata ReadJson(JsonReader reader, Type objectType, ExtensionMetadata existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            ExtensionMetadata metadata = new ExtensionMetadata();
            
            JObject jObject = JObject.Load(reader);
            serializer.Populate(jObject.CreateReader(), metadata);
            metadata.Properties = jObject.ToObject<Dictionary<string, object>>();
           
            return metadata;
        }

        public override void WriteJson(JsonWriter writer, ExtensionMetadata value, JsonSerializer serializer)
        {

        }
    }
}
