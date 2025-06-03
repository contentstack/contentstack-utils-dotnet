using System.Text.Json;
using System.Text.Json.Serialization;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;
using Contentstack.Utils.Tests.Constants;
using Contentstack.Utils.Tests.Mocks;

namespace Contentstack.Utils.Tests.Helpers
{
    public class NodeParser
    {
        public static Node parse(string jsonNode)
        {
            // Remove trailing commas before closing brackets/braces
            string cleanedJson = System.Text.RegularExpressions.Regex.Replace(
                jsonNode,
                @",(\s*[}\]])",
                "$1"
            );
            return JsonSerializer.Deserialize<Node>(cleanedJson);
        }
    }
    public class GQLParser
    {
        public static GQLModel<T> parse<T>(string jsonNode, string embedConnection = null) where T: IEmbeddedObject
        {
            var data = JsonToHtmlConstants.KGQLModel(jsonNode, embedConnection);
            // Remove trailing commas before closing brackets/braces
            string cleanedJson = System.Text.RegularExpressions.Regex.Replace(
                data,
                @",(\s*[}\]])",
                "$1"
            );
            return JsonSerializer.Deserialize<GQLModel<T>>(cleanedJson);
        }
    }

}

