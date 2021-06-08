using System;
using Contentstack.Utils.Models;
using Contentstack.Utils.Tests.Constants;
using Newtonsoft.Json;

namespace Contentstack.Utils.Tests.Helpers
{
    public class NodeParser
    {
        public static Node parse(string jsonNode)
        {
            JsonSerializerSettings SerializerSettings = new JsonSerializerSettings();
            JsonSerializer Serializer = JsonSerializer.Create(SerializerSettings);

            return JsonConvert.DeserializeObject<Node>(jsonNode, SerializerSettings);
        }
    }
}
