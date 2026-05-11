using System;
using System.Collections.Generic;
using System.Linq;
using Contentstack.Utils.Models;
using HtmlAgilityPack;
using Contentstack.Utils.Extensions;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Enums;
using Newtonsoft.Json;
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

        /// <summary>
        /// Adds Live Preview editable tags to an entry for CMS editing capability.
        /// This is the main public API method with enhanced options support.
        /// </summary>
        /// <param name="entry">The entry to add tags to</param>
        /// <param name="contentTypeUid">Content type UID (will be lowercased)</param>
        /// <param name="tagsAsObject">Whether to return tags as objects or strings</param>
        /// <param name="locale">Locale for the entry (default: "en-us")</param>
        /// <param name="options">Options controlling tag generation behavior</param>
        public static void addEditableTags(EditableEntry entry, string contentTypeUid, bool tagsAsObject, string locale = "en-us", AddEditableTagsOptions options = null)
        {
            if (entry == null) return;

            // Apply default options if not provided
            options = options ?? new AddEditableTagsOptions();

            // Normalize inputs according to JavaScript SDK behavior
            contentTypeUid = contentTypeUid?.ToLowerInvariant() ?? "";
            locale = options.UseLowerCaseLocale ? (locale?.ToLowerInvariant() ?? "en-us") : (locale ?? "en-us");

            // Extract applied variants from entry
            var appliedVariants = ExtractAppliedVariants(entry);

            // Generate tags and assign to entry
            entry["$"] = GetTag(entry, $"{contentTypeUid}.{entry.Uid}.{locale}", tagsAsObject, locale, appliedVariants);
        }


        /// <summary>
        /// Alias for addEditableTags to match JavaScript SDK naming.
        /// </summary>
        public static void addTags(EditableEntry entry, string contentTypeUid, bool tagsAsObject, string locale = "en-us", AddEditableTagsOptions options = null)
        {
            addEditableTags(entry, contentTypeUid, tagsAsObject, locale, options);
        }

        /// <summary>
        /// Enhanced GetTag method with comprehensive variant support and proper null handling.
        /// </summary>
        private static Dictionary<string, object> GetTag(object content, string prefix, bool tagsAsObject, string locale, AppliedVariants appliedVariants)
        {
            // Null safety - return empty tags if content is null or undefined
            if (content == null) return new Dictionary<string, object>();

            var tags = new Dictionary<string, object>();
            
            // Handle Dictionary<string, object> directly
            if (content is Dictionary<string, object> contentDict)
            {
                foreach (var property in contentDict)
                {
                    ProcessContentProperty(property.Key, property.Value, prefix, tagsAsObject, locale, appliedVariants, tags);
                }
            }
            // Handle EditableEntry interface
            else if (content is EditableEntry editableEntry)
            {
                // For EditableEntry, we need to get the underlying data
                // Use reflection or casting to get the data
                try
                {
                    // Try to get the underlying dictionary if it's our mock class
                    var getDataMethod = content.GetType().GetMethod("GetData");
                    if (getDataMethod != null)
                    {
                        var data = (Dictionary<string, object>)getDataMethod.Invoke(content, null);
                        foreach (var property in data)
                        {
                            ProcessContentProperty(property.Key, property.Value, prefix, tagsAsObject, locale, appliedVariants, tags);
                        }
                    }
                    else
                    {
                        // Fallback: try to cast to dictionary
                        var dict = (Dictionary<string, object>)content;
                        foreach (var property in dict)
                        {
                            ProcessContentProperty(property.Key, property.Value, prefix, tagsAsObject, locale, appliedVariants, tags);
                        }
                    }
                }
                catch
                {
                    // If all else fails, return empty tags
                    return tags;
                }
            }

            return tags;
        }

        /// <summary>
        /// Processes a single content property for tag generation.
        /// </summary>
        private static void ProcessContentProperty(string key, object value, string prefix, bool tagsAsObject, 
            string locale, AppliedVariants appliedVariants, Dictionary<string, object> tags)
        {
            // Skip the $ key to avoid recursive processing
            if (key == "$") return;

            // Extract metadata UID for variant path building
            string metaUID = ExtractMetadataUid(value);
            
            // Build updated meta key for variant processing
            string updatedMetakey = BuildUpdatedMetaKey(appliedVariants, key, metaUID);

            // Process based on value type
            if (IsArrayType(value))
            {
                ProcessArrayField(value, key, prefix, tagsAsObject, locale, appliedVariants, updatedMetakey, tags);
            }
            else if (value != null)
            {
                ProcessObjectField(value, key, prefix, tagsAsObject, locale, appliedVariants, updatedMetakey, tags);
            }

            // Always emit a tag for the field itself (even if value is null)
            var fieldPath = $"{prefix}.{key}";
            var fieldVariants = new AppliedVariants(appliedVariants._applied_variants, updatedMetakey);
            tags[key] = GetTagsValue(ApplyVariantToDataValue(fieldPath, fieldVariants), tagsAsObject);
        }

        /// <summary>
        /// Legacy GetTag overload for backward compatibility.
        /// </summary>
        private static Dictionary<string, object> GetTag(object content, string prefix, bool tagsAsObject, string locale)
        {
            var emptyVariants = new AppliedVariants();
            return GetTag(content, prefix, tagsAsObject, locale, emptyVariants);
        }

        /// <summary>
        /// Extracts applied variants from an entry, checking both _applied_variants and system.applied_variants.
        /// </summary>
        private static AppliedVariants ExtractAppliedVariants(EditableEntry entry)
        {
            Dictionary<string, string> variants = null;

            // Try to get _applied_variants first (direct property)
            if (entry.ContainsKey("_applied_variants") && entry["_applied_variants"] != null)
            {
                variants = ConvertToStringDictionary(entry["_applied_variants"]);
            }
            // Fallback to system.applied_variants
            else if (entry.ContainsKey("system") && entry["system"] != null)
            {
                try
                {
                    var system = (Dictionary<string, object>)entry["system"];
                    if (system.ContainsKey("applied_variants") && system["applied_variants"] != null)
                    {
                        variants = ConvertToStringDictionary(system["applied_variants"]);
                    }
                }
                catch { /* Ignore conversion errors */ }
            }

            return new AppliedVariants(variants);
        }

        /// <summary>
        /// Safely converts an object to a string dictionary for variant processing.
        /// </summary>
        private static Dictionary<string, string> ConvertToStringDictionary(object obj)
        {
            try
            {
                if (obj is Dictionary<string, object> dict)
                {
                    var result = new Dictionary<string, string>();
                    foreach (var kvp in dict)
                    {
                        if (kvp.Value != null)
                        {
                            result[kvp.Key] = kvp.Value.ToString();
                        }
                    }
                    return result;
                }
                else if (obj is Dictionary<string, string> strDict)
                {
                    return new Dictionary<string, string>(strDict);
                }
            }
            catch { /* Ignore conversion errors */ }

            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Checks if a value is an array type that needs array processing.
        /// </summary>
        private static bool IsArrayType(object value)
        {
            return value is object[] || 
                   value is List<object> || 
                   value is IEnumerable<object>;
        }

        /// <summary>
        /// Extracts metadata UID from a value object for variant path building.
        /// </summary>
        private static string ExtractMetadataUid(object value)
        {
            try
            {
                if (value is Dictionary<string, object> dict &&
                    dict.ContainsKey("_metadata") && 
                    dict["_metadata"] is Dictionary<string, object> metadata &&
                    metadata.ContainsKey("uid"))
                {
                    return metadata["uid"]?.ToString();
                }
            }
            catch { /* Ignore extraction errors */ }

            return null;
        }

        /// <summary>
        /// Builds the updated meta key for variant processing, including metadata UID when applicable.
        /// </summary>
        private static string BuildUpdatedMetaKey(AppliedVariants appliedVariants, string key, string metaUID)
        {
            var metaKeyPrefix = string.IsNullOrEmpty(appliedVariants.metaKey) ? "" : appliedVariants.metaKey + ".";
            var baseMetaKey = metaKeyPrefix + key;
            
            // Append metadata UID if variants are applied and metaUID exists
            if (appliedVariants.shouldApplyVariant && !string.IsNullOrEmpty(metaUID) && !string.IsNullOrEmpty(baseMetaKey))
            {
                return baseMetaKey + "." + metaUID;
            }
            
            return baseMetaKey;
        }

        /// <summary>
        /// Processes array fields with proper null handling, reference detection, and variant support.
        /// </summary>
        private static void ProcessArrayField(object value, string key, string prefix, bool tagsAsObject, 
            string locale, AppliedVariants appliedVariants, string updatedMetakey, Dictionary<string, object> tags)
        {
            // Convert to object array for processing
            object[] array;
            try
            {
                if (value is object[] objArray)
                    array = objArray;
                else if (value is List<object> list)
                    array = list.ToArray();
                else
                    return; // Cannot process this array type
            }
            catch
            {
                return; // Conversion failed
            }

            // Process each array element
            for (int index = 0; index < array.Length; index++)
            {
                object objValue = array[index];
                
                // Skip null and undefined elements (matching JavaScript SDK behavior)
                if (objValue == null) continue;

                string childKey = $"{key}__{index}";
                string parentKey = $"{key}__parent";

                // Generate field__index and field__parent tags
                var indexPath = $"{prefix}.{key}.{index}";
                var parentPath = $"{prefix}.{key}";
                
                var indexVariants = new AppliedVariants(appliedVariants._applied_variants, updatedMetakey);
                var parentVariants = new AppliedVariants(appliedVariants._applied_variants, appliedVariants.metaKey + (string.IsNullOrEmpty(appliedVariants.metaKey) ? "" : ".") + key);

                tags[childKey] = GetTagsValue(ApplyVariantToDataValue(indexPath, indexVariants), tagsAsObject);
                tags[parentKey] = GetParentTagsValue(ApplyVariantToDataValue(parentPath, parentVariants), tagsAsObject);

                // Handle reference entries vs regular objects
                if (IsReferenceEntry(objValue))
                {
                    ProcessReferenceEntry(objValue, tagsAsObject, locale);
                }
                else if (objValue is Dictionary<string, object>)
                {
                    var elementVariants = new AppliedVariants(appliedVariants._applied_variants, updatedMetakey);
                    ((Dictionary<string, object>)objValue)["$"] = GetTag(objValue, indexPath, tagsAsObject, locale, elementVariants);
                }
            }
        }

        /// <summary>
        /// Processes object fields with variant support.
        /// </summary>
        private static void ProcessObjectField(object value, string key, string prefix, bool tagsAsObject,
            string locale, AppliedVariants appliedVariants, string updatedMetakey, Dictionary<string, object> tags)
        {
            try
            {
                if (value is Dictionary<string, object> dict)
                {
                    var fieldVariants = new AppliedVariants(appliedVariants._applied_variants, updatedMetakey);
                    dict["$"] = GetTag(value, $"{prefix}.{key}", tagsAsObject, locale, fieldVariants);
                }
            }
            catch { /* Ignore processing errors for malformed objects */ }
        }

        /// <summary>
        /// Checks if an object is a reference entry (has both _content_type_uid and uid properties).
        /// </summary>
        private static bool IsReferenceEntry(object obj)
        {
            try
            {
                if (obj is Dictionary<string, object> dict)
                {
                    return dict.ContainsKey("_content_type_uid") && 
                           dict.ContainsKey("uid") &&
                           dict["_content_type_uid"] != null && 
                           dict["uid"] != null;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// Processes reference entries with independent variant handling.
        /// </summary>
        private static void ProcessReferenceEntry(object referenceObj, bool tagsAsObject, string parentLocale)
        {
            try
            {
                var refDict = (Dictionary<string, object>)referenceObj;
                
                // Extract reference properties
                string refContentTypeUid = refDict["_content_type_uid"]?.ToString();
                string refUid = refDict["uid"]?.ToString();
                string refLocale = refDict.ContainsKey("locale") ? refDict["locale"]?.ToString() : null;
                
                if (string.IsNullOrEmpty(refContentTypeUid) || string.IsNullOrEmpty(refUid))
                    return;

                // Use reference locale or fallback to parent locale
                string effectiveLocale = !string.IsNullOrEmpty(refLocale) ? refLocale : parentLocale;
                
                // Extract reference-specific variants (do not inherit parent variants)
                var refVariants = ExtractAppliedVariantsFromObject(refDict);
                
                // Generate tags for reference using its own content type, UID, and locale
                string refPrefix = $"{refContentTypeUid.ToLowerInvariant()}.{refUid}.{effectiveLocale.ToLowerInvariant()}";
                refDict["$"] = GetTag(referenceObj, refPrefix, tagsAsObject, effectiveLocale, refVariants);
            }
            catch { /* Ignore processing errors for malformed references */ }
        }

        /// <summary>
        /// Extracts applied variants from a generic object (used for reference entries).
        /// </summary>
        private static AppliedVariants ExtractAppliedVariantsFromObject(Dictionary<string, object> obj)
        {
            Dictionary<string, string> variants = null;

            // Try _applied_variants first
            if (obj.ContainsKey("_applied_variants") && obj["_applied_variants"] != null)
            {
                variants = ConvertToStringDictionary(obj["_applied_variants"]);
            }
            // Fallback to system.applied_variants
            else if (obj.ContainsKey("system") && obj["system"] is Dictionary<string, object> system)
            {
                if (system.ContainsKey("applied_variants") && system["applied_variants"] != null)
                {
                    variants = ConvertToStringDictionary(system["applied_variants"]);
                }
            }

            return new AppliedVariants(variants);
        }

        /// <summary>
        /// Applies variant processing to a data value, adding v2: prefix and variant suffix when applicable.
        /// </summary>
        private static string ApplyVariantToDataValue(string dataValue, AppliedVariants appliedVariants)
        {
            if (!appliedVariants.shouldApplyVariant || appliedVariants._applied_variants == null)
            {
                return dataValue;
            }

            string variant = null;

            // Check for direct field match
            if (appliedVariants._applied_variants.ContainsKey(appliedVariants.metaKey))
            {
                variant = appliedVariants._applied_variants[appliedVariants.metaKey];
            }
            else
            {
                // Find parent variantised path
                string parentPath = GetParentVariantisedPath(appliedVariants);
                if (!string.IsNullOrEmpty(parentPath))
                {
                    variant = appliedVariants._applied_variants[parentPath];
                }
            }

            if (string.IsNullOrEmpty(variant))
            {
                return dataValue;
            }

            // Apply v2: prefix and variant suffix to UID segment
            try
            {
                var segments = ("v2:" + dataValue).Split('.');
                if (segments.Length >= 2)
                {
                    // Modify the UID segment (index 1 after v2: prefix)
                    segments[1] = segments[1] + "_" + variant;
                    return string.Join(".", segments);
                }
            }
            catch { }

            return dataValue;
        }

        /// <summary>
        /// Finds the longest matching parent path for variant inheritance.
        /// </summary>
        private static string GetParentVariantisedPath(AppliedVariants appliedVariants)
        {
            try
            {
                if (appliedVariants._applied_variants == null || string.IsNullOrEmpty(appliedVariants.metaKey))
                {
                    return "";
                }

                var childPathFragments = appliedVariants.metaKey.Split('.');
                
                // Sort keys by length descending for longest match preference
                var sortedKeys = new List<string>(appliedVariants._applied_variants.Keys);
                sortedKeys.Sort((a, b) => b.Length.CompareTo(a.Length));

                foreach (var path in sortedKeys)
                {
                    var parentFragments = path.Split('.');
                    
                    // Check if this path is a parent of the current meta key
                    if (parentFragments.Length <= childPathFragments.Length)
                    {
                        bool isParent = true;
                        for (int i = 0; i < parentFragments.Length; i++)
                        {
                            if (parentFragments[i] != childPathFragments[i])
                            {
                                isParent = false;
                                break;
                            }
                        }
                        
                        if (isParent)
                        {
                            return path;
                        }
                    }
                }
            }
            catch { }

            return "";
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

        public static JObject GetVariantAliases(JObject entry, string contentTypeUid)
        {
            if (string.IsNullOrEmpty(contentTypeUid))
            {
                throw new ArgumentException("ContentType is required.");
            }
            if (entry == null)
            {
                throw new ArgumentException("Entry must not be null.");
            }
            if (!entry.ContainsKey("uid") || entry["uid"] == null || entry["uid"].Type == JTokenType.Null)
            {
                throw new ArgumentException("Entry must contain uid.");
            }

            string entryUid = entry["uid"]?.ToString() ?? "";
            JArray variantsArray = ExtractVariantAliasesFromEntry(entry);
            JObject result = new JObject
            {
                ["entry_uid"] = entryUid,
                ["contenttype_uid"] = contentTypeUid,
                ["variants"] = variantsArray
            };
            return result;
        }

        public static JArray GetVariantAliases(JArray entries, string contentTypeUid)
        {
            if (string.IsNullOrEmpty(contentTypeUid))
            {
                throw new ArgumentException("ContentType is required.");
            }
            if (entries == null)
            {
                return new JArray();
            }
            JArray variantResults = new JArray();
            foreach (JToken token in entries)
            {
                JObject entry = token as JObject;
                if (entry != null && entry.ContainsKey("uid") && entry["uid"] != null && entry["uid"].Type != JTokenType.Null)
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
        /// <returns>A <see cref="JObject"/> with a <c>data-csvariants</c> key whose value is a compact JSON array string.</returns>
        public static JObject GetVariantMetadataTags(JObject entry, string contentTypeUid)
        {
            if (entry == null)
            {
                JObject result = new JObject();
                result["data-csvariants"] = "[]";
                return result;
            }
            JArray entries = new JArray();
            entries.Add(entry);
            return GetVariantMetadataTags(entries, contentTypeUid);
        }

        /// <summary>
        /// Builds the JSON object used for the <c>data-csvariants</c> HTML attribute payload from multiple entries.
        /// </summary>
        /// <param name="entries">Array of entry JSON objects, or <c>null</c> to produce an empty payload.</param>
        /// <param name="contentTypeUid">Content type UID shared by these entries.</param>
        /// <returns>A <see cref="JObject"/> with a <c>data-csvariants</c> key whose value is a compact JSON array string.</returns>
        public static JObject GetVariantMetadataTags(JArray entries, string contentTypeUid)
        {
            JObject result = new JObject();
            if (entries == null)
            {
                result["data-csvariants"] = "[]";
                return result;
            }
            if (string.IsNullOrEmpty(contentTypeUid))
            {
                throw new ArgumentException("ContentType is required.");
            }

            JArray variantResults = GetVariantAliases(entries, contentTypeUid);
            result["data-csvariants"] = variantResults.ToString(Formatting.None);
            return result;
        }

        /// <summary>
        /// Prefer <see cref="GetVariantMetadataTags(JObject, string)"/>. This alias exists for backward compatibility and will be removed in a future major release.
        /// </summary>
        [Obsolete("Use GetVariantMetadataTags instead. This method will be removed in a future major release.")]
        public static JObject GetDataCsvariantsAttribute(JObject entry, string contentTypeUid)
        {
            return GetVariantMetadataTags(entry, contentTypeUid);
        }

        /// <summary>
        /// Prefer <see cref="GetVariantMetadataTags(JArray, string)"/>. This alias exists for backward compatibility and will be removed in a future major release.
        /// </summary>
        [Obsolete("Use GetVariantMetadataTags instead. This method will be removed in a future major release.")]
        public static JObject GetDataCsvariantsAttribute(JArray entries, string contentTypeUid)
        {
            return GetVariantMetadataTags(entries, contentTypeUid);
        }

        private static JArray ExtractVariantAliasesFromEntry(JObject entry)
        {
            JArray variantArray = new JArray();
            JObject publishDetails = entry["publish_details"] as JObject;
            if (publishDetails == null)
            {
                return variantArray;
            }
            JObject variants = publishDetails["variants"] as JObject;
            if (variants == null)
            {
                return variantArray;
            }

            foreach (JProperty prop in variants.Properties())
            {
                if (prop.Value is JObject valueObj)
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
    }
}
