using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Contentstack.Utils
{
    /// <summary>
    /// Newtonsoft.Json deserialized scalar attributes as <see cref="string"/>.
    /// System.Text.Json stores dynamic values as <see cref="JsonElement"/> — unwrap for the same runtime behavior.
    /// </summary>
    internal static class JsonAttrValue
    {
        internal static string AsString(object value)
        {
            if (value == null)
                return null;

            if (value is string s)
                return s;

            if (value is JsonElement je)
            {
                switch (je.ValueKind)
                {
                    case JsonValueKind.String:
                        return je.GetString();
                    case JsonValueKind.Number:
                        return je.GetRawText();
                    case JsonValueKind.True:
                        return "true";
                    case JsonValueKind.False:
                        return "false";
                    case JsonValueKind.Null:
                        return null;
                    default:
                        return je.ToString();
                }
            }

            return Convert.ToString(value);
        }

        internal static bool TryGetStyleObject(object styleVal, out Dictionary<string, string> styles)
        {
            styles = null;
            if (styleVal == null)
                return false;

            if (styleVal is JsonObject jo)
            {
                styles = JsonSerializer.Deserialize<Dictionary<string, string>>(jo.ToJsonString());
                return styles != null;
            }

            if (styleVal is JsonElement je && je.ValueKind == JsonValueKind.Object)
            {
                styles = JsonSerializer.Deserialize<Dictionary<string, string>>(je.GetRawText());
                return styles != null;
            }

            return false;
        }
    }
}
