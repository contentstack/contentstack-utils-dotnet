using Contentstack.Utils.Models;
using HtmlAgilityPack;

namespace Contentstack.Utils.Extensions
{
    public delegate void MetadataCallBack(Metadata metadata);

    public static class HtmlDocumentExtension
    {
        public static void FindEmbeddedObject(this HtmlDocument htmlDocument, MetadataCallBack callBack)
        {
            var htmlNode = htmlDocument.DocumentNode.SelectNodes("//*[contains(@class, 'embedded-asset') or contains(@class, 'embedded-entry')]");
            if (htmlNode != null)
            {
                foreach (var node in htmlNode)
                {
                    callBack(node);
                }
            }
        }
    }
}
