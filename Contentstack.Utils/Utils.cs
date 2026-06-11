using System.Collections.Generic;
using Contentstack.Utils.Models;
using HtmlAgilityPack;
using Contentstack.Utils.Extensions;
using Contentstack.Utils.Interfaces;
using System;
using Contentstack.Utils.Enums;
using System.Text.Json;
using System.Text.Json.Nodes;

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
            if (!string.IsNullOrEmpty(textNode.classname) || !string.IsNullOrEmpty(textNode.id))
            {
                text = options.RenderMark(MarkType.Class, text: text, textNode.classname, textNode.id);
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

        public static void addEditableTags(EditableEntry entry, string contentTypeUid, bool tagsAsObject, string locale = "en-us")
        {
            if (entry != null)
                entry["$"] = GetTag(entry, $"{contentTypeUid}.{entry.Uid}.{locale}", tagsAsObject, locale);
        }

        private static Dictionary<string, object> GetTag(object content, string prefix, bool tagsAsObject, string locale)
        {
            var tags = new Dictionary<string, object>();
            foreach (var property in (Dictionary<string, object>)content)
            {
                var key = property.Key;
                var value = property.Value;

                if (key == "$")
                    continue;
                    
                switch (value)
                {
                    case object obj when obj is object[] array:
                        for (int index = 0; index < array.Length; index++)
                        {
                            object objValue = array[index]; 
                            string childKey = $"{key}__{index}";
                            string parentKey = $"{key}__parent";

                            tags[childKey] = GetTagsValue($"{prefix}.{key}.{index}", tagsAsObject);
                            tags[parentKey] = GetParentTagsValue($"{prefix}.{key}", tagsAsObject);

                            if (objValue != null &&
                                objValue.GetType().GetProperty("_content_type_uid") != null &&
                                objValue.GetType().GetProperty("Uid") != null)
                            {
                                var typedObj = (EditableEntry)objValue;
                                string locale_ = Convert.ToString(typedObj.GetType().GetProperty("locale").GetValue(typedObj));
                                string ctUid = Convert.ToString(typedObj.GetType().GetProperty("_content_type_uid").GetValue(typedObj));
                                string uid = Convert.ToString(typedObj.GetType().GetProperty("uid").GetValue(typedObj));
                                string localeStr = "";
                                if (locale_ != null)
                                {
                                    localeStr = locale_;
                                } else
                                {
                                    localeStr = locale;
                                }
                                typedObj["$"] = GetTag(typedObj, $"{ctUid}.{uid}.{localeStr}", tagsAsObject, locale);
                            }
                            else if (value is object)
                            {
                                ((EditableEntry)value)["$"] = GetTag(value, $"{prefix}.{key}.{index}", tagsAsObject, locale);
                            }
                        }
                        tags[key] = GetTagsValue($"{prefix}.{key}", tagsAsObject);
                        break;
                    case object obj when obj != null:
                        if (value != null)
                        {
                            ((EditableEntry)value)["$"] = GetTag(value, $"{prefix}.{key}", tagsAsObject, locale);
                        }
                        break;
                }
            }
            return tags;
        }

        private static object GetTagsValue(string dataValue, bool tagsAsObject)
        {
            if (tagsAsObject)
            {
                return new Dictionary<string, object> { { "data-cslp", dataValue } };
            }
            else
            {
                return $"data-cslp={dataValue}";
            }
        }

        private static object GetParentTagsValue(string dataValue, bool tagsAsObject)
        {
            if (tagsAsObject)
            {
                return new Dictionary<string, object> { { "data-cslp-parent-field", dataValue } };
            }
            else
            {
                return $"data-cslp-parent-field={dataValue}";
            }
        }

        private static readonly JsonSerializerOptions CompactJsonOptions = new JsonSerializerOptions { WriteIndented = false };

        private static bool IsJsonNull(JsonNode node)
        {
            if (node == null)
                return true;
            return node is JsonValue jv && jv.GetValueKind() == JsonValueKind.Null;
        }

        public static JsonObject GetVariantAliases(JsonObject entry, string contentTypeUid)
        {
            if (string.IsNullOrEmpty(contentTypeUid))
            {
                throw new ArgumentException("ContentType is required.");
            }
            if (entry == null)
            {
                throw new ArgumentException("Entry must not be null.");
            }
            if (!entry.TryGetPropertyValue("uid", out JsonNode uidNode) || IsJsonNull(uidNode))
            {
                throw new ArgumentException("Entry must contain uid.");
            }

            string entryUid = uidNode?.ToString() ?? "";
            JsonArray variantsArray = ExtractVariantAliasesFromEntry(entry);
            var result = new JsonObject
            {
                ["entry_uid"] = entryUid,
                ["contenttype_uid"] = contentTypeUid,
                ["variants"] = variantsArray
            };
            return result;
        }

        public static JsonArray GetVariantAliases(JsonArray entries, string contentTypeUid)
        {
            if (string.IsNullOrEmpty(contentTypeUid))
            {
                throw new ArgumentException("ContentType is required.");
            }
            if (entries == null)
            {
                return new JsonArray();
            }
            var variantResults = new JsonArray();
            foreach (JsonNode token in entries)
            {
                if (token is JsonObject entry &&
                    entry.TryGetPropertyValue("uid", out JsonNode uidNode) &&
                    uidNode != null &&
                    !IsJsonNull(uidNode))
                {
                    variantResults.Add(GetVariantAliases(entry, contentTypeUid));
                }
            }
            return variantResults;
        }

        /// <summary>
        /// Builds the JSON object used for the <c>data-csvariants</c> HTML attribute payload from a single entry.
        /// </summary>
        /// <param name="entry">Entry JSON (e.g. from the Delivery API), or <c>null</c> to produce an empty payload.</param>
        /// <param name="contentTypeUid">Content type UID for the entry.</param>
        /// <returns>A <see cref="JsonObject"/> with a <c>data-csvariants</c> key whose value is a compact JSON array string.</returns>
        public static JsonObject GetVariantMetadataTags(JsonObject entry, string contentTypeUid)
        {
            if (entry == null)
            {
                var empty = new JsonObject();
                empty["data-csvariants"] = "[]";
                return empty;
            }
            var entries = new JsonArray();
            entries.Add(entry.DeepClone());
            return GetVariantMetadataTags(entries, contentTypeUid);
        }

        /// <summary>
        /// Builds the JSON object used for the <c>data-csvariants</c> HTML attribute payload from multiple entries.
        /// </summary>
        /// <param name="entries">Array of entry JSON objects, or <c>null</c> to produce an empty payload.</param>
        /// <param name="contentTypeUid">Content type UID shared by these entries.</param>
        /// <returns>A <see cref="JsonObject"/> with a <c>data-csvariants</c> key whose value is a compact JSON array string.</returns>
        public static JsonObject GetVariantMetadataTags(JsonArray entries, string contentTypeUid)
        {
            var result = new JsonObject();
            if (entries == null)
            {
                result["data-csvariants"] = "[]";
                return result;
            }
            if (string.IsNullOrEmpty(contentTypeUid))
            {
                throw new ArgumentException("ContentType is required.");
            }

            JsonArray variantResults = GetVariantAliases(entries, contentTypeUid);
            result["data-csvariants"] = variantResults.ToJsonString(CompactJsonOptions);
            return result;
        }

        /// <summary>
        /// Prefer <see cref="GetVariantMetadataTags(JsonObject, string)"/>. This alias exists for backward compatibility and will be removed in a future major release.
        /// </summary>
        [Obsolete("Use GetVariantMetadataTags instead. This method will be removed in a future major release.")]
        public static JsonObject GetDataCsvariantsAttribute(JsonObject entry, string contentTypeUid)
        {
            return GetVariantMetadataTags(entry, contentTypeUid);
        }

        /// <summary>
        /// Prefer <see cref="GetVariantMetadataTags(JsonArray, string)"/>. This alias exists for backward compatibility and will be removed in a future major release.
        /// </summary>
        [Obsolete("Use GetVariantMetadataTags instead. This method will be removed in a future major release.")]
        public static JsonObject GetDataCsvariantsAttribute(JsonArray entries, string contentTypeUid)
        {
            return GetVariantMetadataTags(entries, contentTypeUid);
        }

        private static JsonArray ExtractVariantAliasesFromEntry(JsonObject entry)
        {
            var variantArray = new JsonArray();
            if (!entry.TryGetPropertyValue("publish_details", out JsonNode pdNode) || pdNode is not JsonObject publishDetails)
            {
                return variantArray;
            }
            if (!publishDetails.TryGetPropertyValue("variants", out JsonNode vNode) || vNode is not JsonObject variants)
            {
                return variantArray;
            }

            foreach (KeyValuePair<string, JsonNode> kvp in variants)
            {
                if (kvp.Value is JsonObject valueObj)
                {
                    string alias = valueObj["alias"]?.ToString();
                    if (!string.IsNullOrEmpty(alias))
                    {
                        variantArray.Add(alias.Trim());
                    }
                }
            }
            return variantArray;
        }

        /// <summary>
        /// Resolves a single Contentstack service endpoint URL for the given region.
        /// Proxy for <see cref="Endpoints.Endpoint.GetContentstackEndpoint(string, string, bool)"/>.
        /// </summary>
        public static string GetContentstackEndpoint(string region, string service, bool omitHttps = false)
            => Endpoints.Endpoint.GetContentstackEndpoint(region, service, omitHttps);

        /// <summary>
        /// Returns all service endpoint URLs for the given region.
        /// Proxy for <see cref="Endpoints.Endpoint.GetContentstackEndpoint(string, bool)"/>.
        /// </summary>
        public static Dictionary<string, string> GetContentstackEndpoint(string region, bool omitHttps = false)
            => Endpoints.Endpoint.GetContentstackEndpoint(region, omitHttps);
    }
}
