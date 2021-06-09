using System.Collections.Generic;
using Contentstack.Utils.Models;
using HtmlAgilityPack;
using Contentstack.Utils.Extensions;
using Contentstack.Utils.Interfaces;
using System;
using Contentstack.Utils.Enums;

namespace Contentstack.Utils
{
    public class Utils
    {
        public static List<string> RenderContent(List<string> contents, Options options)
        {
            List<string> result = new List<string>();

            contents.ForEach((content) =>
            {
                result.Add(RenderContent(content, options));
            });

            return result;
        }

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

        public static List<string> JsonToHtml(List<Node> nodes, Options options)
        {

            List<string> result = new List<string>();

            nodes.ForEach((node) =>
            {
                result.Add(JsonToHtml(node, options));
            });

            return result;
        }

        public static string JsonToHtml(Node node, Options options)
        {
            return nodeChildrenToHtml(node.children, options);
        }

        private static string nodeChildrenToHtml(List<Node> nodes, Options options)
        {
            return string.Join("", nodes.ConvertAll<string>(node => nodeToHtml(node, options)));
            
        }

        private static string nodeToHtml(Node node, Options options)
        {
            switch (node.type)
            {
                case Enums.NodeType.Text:
                    return textToHtml((TextNode)node, options);
                case Enums.NodeType.Reference:
                    return referenceToHtml(node, options);
            }
            return options.RenderNode(node.type, node, (nodes) => { return nodeChildrenToHtml(nodes, options); });
        }

        private static string referenceToHtml(Node node, Options options)
        {
            Metadata metadata = node;
            IEmbeddedObject embeddedObject = findEmbeddedObject(metadata, options.entry);
            if (embeddedObject != null)
            {
                return options.RenderOption(embeddedObject, metadata);
            }
            return "";
        }

        private static string textToHtml(TextNode textNode, Options options)
        {
            var text = textNode.text;
            if (textNode.superscript)
            {
                text = options.RenderMark(MarkType.Superscript, text: text);
            }
            if (textNode.subscript)
            {
                text = options.RenderMark(MarkType.Subscript, text: text);
            }
            if (textNode.inlineCode)
            {
                text = options.RenderMark(MarkType.InlineCode, text: text);
            }
            if (textNode.strikethrough)
            {
                text = options.RenderMark(MarkType.Strikethrough, text: text);
            }
            if (textNode.underline)
            {
                text = options.RenderMark(MarkType.Underline, text: text);
            }
            if (textNode.italic)
            {
                text = options.RenderMark(MarkType.Italic, text: text);
            }
            if (textNode.bold)
            {
                text = options.RenderMark(MarkType.Bold, text: text);
            }
            return text;
        }

        private static IEmbeddedObject findEmbeddedObject(Metadata metadata, IEntryEmbedable entryEmbedable)
        {
        
            if (entryEmbedable != null && entryEmbedable.embeddedItems.Count > 0)
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
