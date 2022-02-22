using System.Collections.Generic;
using Contentstack.Utils.Models;
using HtmlAgilityPack;
using Contentstack.Utils.Extensions;
using Contentstack.Utils.Interfaces;
using System;
using Contentstack.Utils.Enums;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Contentstack.Utils
{
    public class Utils
    {
        public class GQL
        {
            public static string JsonToHtml<T>(JsonRTENode<T> jsonRTE, Options options) where T: IEmbeddedObject
            {
                return Utils.nodeChildrenToHtml(jsonRTE.Json.children, options, GQL.refernceToHtml(options, jsonRTE.Edges));
            }

            public static List<string> JsonToHtml<T>(JsonRTENodes<T> jsonRTE, Options options) where T : IEmbeddedObject
            {
                List<string> result = new List<string>();
                jsonRTE.Json.ForEach((content) =>
                {
                    result.Add(nodeChildrenToHtml(content.children, options, GQL.refernceToHtml(options, jsonRTE.Edges)));
                });
                return result;
            }

            private static Func<Node, string> refernceToHtml<T>(Options options, List<IEdges<T>> edges) where T : IEmbeddedObject
            {
                return (n) =>
                {
                    Metadata metadata = n;
                    if (edges != null && edges.Count > 0)
                    {
                        IEdges<T> edge = edges.Find(entry => {
                            return entry.Node.Uid == metadata.ItemUid && entry.Node.ContentTypeUid == metadata.ContentTypeUid;
                        });

                        if (edge != null && edge.Node != null)
                        {
                            return options.RenderOption(edge.Node, metadata);
                        }
                    }
                    return "";
                };
            }
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

        public static string RenderContent(string content, Options options)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);

            var resultContent = htmlDocument.DocumentNode.OuterHtml;

            htmlDocument.FindEmbeddedObject((Metadata metadata) =>
            {
                var replaceString = "";
                IEmbeddedObject embeddedObject = findEmbeddedObject(metadata, options.entry.embeddedItems);
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
            Func<Node, string> referenceToHtml = (n) =>
            {
                Metadata metadata = n;
                if (options.entry != null)
                {
                    IEmbeddedObject embeddedObject = findEmbeddedObject(metadata, options.entry.embeddedItems);
                    if (embeddedObject != null)
                    {
                        return options.RenderOption(embeddedObject, metadata);
                    }
                }
                return "";
            };
            return nodeChildrenToHtml(node.children, options, referenceToHtml);
        }

        private static string nodeChildrenToHtml(List<Node> nodes, Options options, Func<Node, string> referenceToHtml)
        {
            return string.Join("", nodes.ConvertAll<string>(node => nodeToHtml(node, options, referenceToHtml)));
            
        }

        private static string nodeToHtml(Node node, Options options, Func<Node, string> referenceToHtml)
        {
            switch (node.type)
            {
                case "text":
                    return textToHtml((TextNode)node, options);
                case "reference":
                    return referenceToHtml(node);
            }
            return options.RenderNode(node.type, node, (nodes) => { return nodeChildrenToHtml(nodes, options, referenceToHtml); });
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

        private static IEmbeddedObject findEmbeddedObject(Metadata metadata, Dictionary<string, List<IEmbeddedObject>> embeddedItems)
        {
        
            if (embeddedItems != null && embeddedItems.Count > 0)
            {
                foreach (var embed in embeddedItems)
                {
                    if (embed.Value.Count > 0)
                    {
                        return findEmbedded(metadata, embed.Value);
                    }
                }
            }
            return null;
        }

        private static IEmbeddedObject findEmbedded(Metadata metadata, List<IEmbeddedObject> items)
        {

            if (items != null && items.Count > 0)
            {
                IEmbeddedObject embeddedObject = items.Find(entry => {
                    return entry.Uid == metadata.ItemUid && entry.ContentTypeUid == metadata.ContentTypeUid;
                });
                if (embeddedObject != null)
                {
                    return embeddedObject;
                }
                    
            }
            return null;
        }


        public static string ResolvePreset(string url, AssetMetadata assetMetadata, string extenionUid, string presetName)
        {
            Preset preset = getPreset(assetMetadata, extenionUid, presetName: presetName);
            if (preset != null)
            {
                return getImageUrl(url, preset);
            }
            return url;
            
        }
        public static string ResolvePresetbyUid(string url, AssetMetadata assetMetadata, string extenionUid, string presetUid)
        {
            Preset preset = getPreset(assetMetadata, extenionUid, presetUid: presetUid);
            if (preset != null)
            {
                return getImageUrl(url, preset);
            }
            return url;
        }
        public static NameValueCollection ParseQueryString(string s)
        {
            NameValueCollection nvc = new NameValueCollection();

            if (s.Contains("?"))
            {
                s = s.Substring(s.IndexOf('?') + 1);
                foreach (string vp in Regex.Split(s, "&"))
                {
                    string[] singlePair = Regex.Split(vp, "=");
                    if (singlePair.Length == 2)
                    {
                        nvc.Add(singlePair[0], singlePair[1]);
                    }
                    else
                    {
                        // only one key with no value specified in query string
                        nvc.Add(singlePair[0], string.Empty);
                    }
                }
            }
            return nvc;
        }

        public static string ConstructQueryString(NameValueCollection parameters)
        {
            List<string> items = new List<string>();

            foreach (string name in parameters)
                items.Add(string.Concat(name, "=", parameters[name]));

            return string.Join("&", items.ToArray());
        }

        private static string getImageUrl(string url, Preset preset)
        {
            if (preset.Options != null)
            {
                Uri uri = new Uri(url);
                NameValueCollection queryString = ParseQueryString(url);
                if (preset.Options.ContainsKey("transform"))
                {
                    addTransform(ref queryString, (JObject)preset.Options["transform"]);
                }
                if (preset.Options.ContainsKey("image-type"))
                {
                    queryString.Add("format", (string)preset.Options["image-type"]);
                }
                if (preset.Options.ContainsKey("quality"))
                {
                    queryString.Add("quality", (string)preset.Options["quality"]);
                }
                if (preset.Options.ContainsKey("effects"))
                {
                    addEffects(ref queryString, (JObject)preset.Options["effects"]);
                }
                var queryParams = ConstructQueryString(queryString);
                if (queryParams.Length > 0) {
                    return $"{String.Format("{0}{1}{2}{3}", uri.Scheme, Uri.SchemeDelimiter, uri.Authority, uri.AbsolutePath)}?{ConstructQueryString(queryString)}";
                }
            }
            return url;
        }

        private static void addEffects(ref NameValueCollection queryString, JObject effects)
        {
            if (effects.ContainsKey("brightness"))
            {
                queryString.Add("brightness", (string)effects["brightness"]);
            }
            if (effects.ContainsKey("contrast"))
            {
                queryString.Add("contrast", (string)effects["contrast"]);
            }
            if (effects.ContainsKey("saturate"))
            {
                queryString.Add("saturation", (string)effects["saturate"]);
            }
            if (effects.ContainsKey("blur"))
            {
                queryString.Add("blur", (string)effects["blur"]);
            }
            if (effects.ContainsKey("sharpen") && effects["sharpen"].GetType() == typeof(JObject))
            {
                JObject sharpen = (JObject)effects["sharpen"];
                var amount = sharpen["amount"] ?? 0;
                var radius = sharpen["radius"] ?? 1;
                var threshold = sharpen["threshold"] ?? 0;
                queryString.Add("sharpen", $"a{amount},r{radius},t{threshold}");
            }
        }

        private static void addTransform(ref NameValueCollection queryString, JObject transform)
        {
            if (transform.ContainsKey("height"))
            {
                queryString.Add("height", (string)transform["height"]);
            }
            if (transform.ContainsKey("width"))
            {
                queryString.Add("width", (string)transform["width"]);
            }
            if (transform.ContainsKey("flip-mode"))
            {
                string flipMode = (string)transform["flip-mode"];
                if (flipMode == "both")
                {
                    queryString.Add("orient", "3");
                }
                else
                if (flipMode == "horiz")
                {
                    queryString.Add("orient", "2");
                }
                else
                if (flipMode == "verti")
                {
                    queryString.Add("orient", "4");
                }
            }
        }

        private static Preset getPreset(AssetMetadata assetMetadata, string extenionUid, string presetName = null, string presetUid = null)
        {
            Preset preset = null;
            if (assetMetadata.extensions != null)
            {
                AssetExtension assetExtension = assetMetadata.extensions.Find(extention =>
                {
                    return extention.Uid == extenionUid;
                });
                if (assetExtension != null)
                {
                    if (assetExtension.LocalMetadata != null && assetExtension.LocalMetadata.Presets != null)
                    {
                        preset = assetExtension.LocalMetadata.Presets.Find(item =>
                        {
                            return item.Uid == presetUid || item.Name == presetName;
                        });
                    }

                    if (preset == null && assetExtension.GlobalMetadata != null && assetExtension.GlobalMetadata.Presets != null)
                    {
                        preset = assetExtension.GlobalMetadata.Presets.Find(item =>
                        {
                            return item.Uid == presetUid || item.Name == presetName;
                        });
                    }
                }
            }
           
            return preset;
        }
    }
}
