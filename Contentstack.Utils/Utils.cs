using System.Collections.Generic;
using Contentstack.Utils.Models;
using HtmlAgilityPack;
using Contentstack.Utils.Extensions;
using Contentstack.Utils.Interfaces;

namespace Contentstack.Utils
{
    public class Utils
    {
        public static string RenderContent(string content, Options options)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);

            var resultContent = htmlDocument.DocumentNode.OuterHtml;

            htmlDocument.FindEmbeddedObject((Metadata metadata) =>
            {
                var replaceString = "";
                IEmbeddedObject embeddedObject = findEmbeddedObject(metadata, options.entry);
                if (embeddedObject != null)
                {
                    replaceString = options.RenderOption(embeddedObject, metadata);
                }
                resultContent = resultContent.Replace(metadata.OuterHTML, replaceString);
            });
            return resultContent;
        }

        public static List<string> RenderContent(List<string> contents, Options options)
        {
            List<string> result = new List<string>();

            contents.ForEach((content) =>
            {
                result.Add(RenderContent(content, options));
            });

            return result;
        }

        private static IEmbeddedObject findEmbeddedObject(Metadata metadata, IEntryEmbedable entryEmbedable)
        {
            switch (metadata.ItemType)
            {
                case Enums.EmbedItemType.Asset:
                    if (entryEmbedable.embeddedAssets.Count > 0)
                    {
                        foreach (var embed in entryEmbedable.embeddedAssets)
                        {
                            if (embed.Value.Count > 0)
                            {
                                IEmbeddedAsset embeddedAsset = embed.Value.Find(asset => asset.Uid == metadata.ItemUid);
                                if (embeddedAsset != null)
                                {
                                    return embeddedAsset;
                                }
                            }
                        }
                    }
                    break;
                case Enums.EmbedItemType.Entry:
                    if (entryEmbedable.embeddedAssets.Count > 0)
                    {
                        foreach (var embed in entryEmbedable.embeddedEntries)
                        {
                            if (embed.Value.Count > 0)
                            {
                                IEmbeddedObject embeddedObject = embed.Value.Find(entry => entry.Uid == metadata.ItemUid);
                                if (embeddedObject != null)
                                {
                                    return embeddedObject;
                                }
                            }
                        }
                    }
                    break;
            }
            return null;
        }
    }
}
