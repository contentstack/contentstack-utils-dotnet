using System.Text.Json;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;
using Contentstack.Utils.Tests.Constants;
using Contentstack.Utils.Tests.Mocks;

namespace Contentstack.Utils.Tests.Helpers
{
    public class NodeParser
    {
        private static readonly JsonSerializerOptions SerializerSettings = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
        };

        public static Node parse(string jsonNode)
        {
            return JsonSerializer.Deserialize<Node>(jsonNode, SerializerSettings);
        }
    }

    public class GQLParser
    {
        private static readonly JsonSerializerOptions SerializerSettings = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
        };

        public static GQLModel<T> parse<T>(string jsonNode, string embedConnection = null) where T : IEmbeddedObject
        {
            var data = JsonToHtmlConstants.KGQLModel(jsonNode, embedConnection);
            return JsonSerializer.Deserialize<GQLModel<T>>(data, SerializerSettings);
        }
    }
}
