using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;
using Contentstack.Utils.Tests.Constants;
using Contentstack.Utils.Tests.Mocks;
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
    public class GQLParser
    {
        public static GQLModel<T> parse<T>(string jsonNode, string embedConnection = null) where T: IEmbeddedObject
        {
            var data = JsonToHtmlConstants.KGQLModel(jsonNode, embedConnection);
            JsonSerializerSettings SerializerSettings = new JsonSerializerSettings();
            JsonSerializer Serializer = JsonSerializer.Create(SerializerSettings);
            return JsonConvert.DeserializeObject<GQLModel<T>>(data, SerializerSettings);
        }
    }

    public class AssetParser
    {
        public static AssetMetaModel parse(string jsonNode)
        {
            JsonSerializerSettings SerializerSettings = new JsonSerializerSettings();
            JsonSerializer Serializer = JsonSerializer.Create(SerializerSettings);
            return JsonConvert.DeserializeObject<AssetMetaModel>(jsonNode, SerializerSettings);
        }
    }
}

