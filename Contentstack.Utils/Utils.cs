using System.Collections.Generic;
using Contentstack.Utils.Models;
using HtmlAgilityPack;
using Contentstack.Utils.Extensions;
using Contentstack.Utils.Interfaces;
using System;

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
        
            if (entryEmbedable.embeddedItems.Count > 0)
            {
                foreach (var embed in entryEmbedable.embeddedItems)
                {
                    if (embed.Value.Count > 0)
                    {
                        IEmbeddedObject embeddedObject = embed.Value.Find(entry => {
                            Console.WriteLine(entry);
                            return entry.Uid == metadata.ItemUid && entry.ContentTypeUid == metadata.ContentTypeUid;
                         });
                        if (embeddedObject != null)
                        {
                            return embeddedObject;
                        }
                    }
                }
            }
            return null;
        }
    }
}
