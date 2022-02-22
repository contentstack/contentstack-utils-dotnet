using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Contentstack.Utils.Models;
using System.Collections.Generic;

namespace Contentstack.Utils.Converters
{
    public class AssetMetadataConverter : JsonConverter<AssetMetadata>
    {
        public override AssetMetadata ReadJson(JsonReader reader, Type objectType, AssetMetadata existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            AssetMetadata metadata = new AssetMetadata();
            metadata.extensions = new List<AssetExtension>();
            JObject jObject = JObject.Load(reader);
            if (jObject.GetValue("extensions") != null && jObject.GetValue("extensions").GetType() == typeof(JObject))
            {
                foreach (JProperty jProperty in (jObject.GetValue("extensions") as JObject).Properties())
                {
                    AssetExtension extension = new AssetExtension();
                    extension.Uid = jProperty.Name;
                    serializer.Populate(jProperty.Value.CreateReader(), extension);
                    metadata.extensions.Add(extension);
                }
            }
            
            return metadata;
        }

        public override void WriteJson(JsonWriter writer, AssetMetadata value, JsonSerializer serializer)
        {

        }
    }
}
