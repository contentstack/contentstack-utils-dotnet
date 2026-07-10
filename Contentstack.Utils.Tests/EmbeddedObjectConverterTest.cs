using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Contentstack.Utils.Converters;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;
using Xunit;

namespace Contentstack.Utils.Tests
{
    public class EmbeddedObjectConverterTest
    {
        private readonly EmbeddedObjectConverter _converter = new EmbeddedObjectConverter();

        // ── JSON file loader (mirrors VariantAliasesTest pattern) ─────────────

        private static string ReadJson(string fileName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Resources", fileName);
            return File.ReadAllText(path);
        }

        private static JsonSerializerOptions OptionsWithConverter()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EmbeddedObjectConverter());
            return options;
        }

        private static EmbeddedObject DeserializeSingle(string json)
        {
            return (EmbeddedObject)JsonSerializer.Deserialize<IEmbeddedObject>(
                json, OptionsWithConverter());
        }

        // ── EmbeddedObjectConverter.CanConvert ────────────────────────────────

        [Fact]
        public void CanConvert_IEmbeddedObject_ReturnsTrue()
        {
            Assert.True(_converter.CanConvert(typeof(IEmbeddedObject)));
        }

        [Fact]
        public void CanConvert_IEmbeddedEntry_ReturnsFalse()
        {
            // Converter must not intercept the sub-interfaces — only the bare IEmbeddedObject.
            Assert.False(_converter.CanConvert(typeof(IEmbeddedEntry)));
        }

        [Fact]
        public void CanConvert_IEmbeddedAsset_ReturnsFalse()
        {
            Assert.False(_converter.CanConvert(typeof(IEmbeddedAsset)));
        }

        [Fact]
        public void CanConvert_EmbeddedObject_ReturnsFalse()
        {
            // Concrete class must fall through to System.Text.Json default property mapping.
            Assert.False(_converter.CanConvert(typeof(EmbeddedObject)));
        }

        [Fact]
        public void CanConvert_CustomerSubclass_ReturnsFalse()
        {
            // Customer-defined subclasses must not be intercepted.
            Assert.False(_converter.CanConvert(typeof(CustomerDefinedEmbeddedObject)));
        }

        // ── EmbeddedObjectConverter.Write (read-only converter) ───────────────

        [Fact]
        public void Write_ThrowsNotSupportedException()
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);
            Assert.Throws<NotSupportedException>(() =>
                _converter.Write(writer, new EmbeddedObject(), new JsonSerializerOptions()));
        }

        // ── Read — null token ─────────────────────────────────────────────────

        [Fact]
        public void Read_NullToken_ReturnsNull()
        {
            var result = JsonSerializer.Deserialize<IEmbeddedObject>("null", OptionsWithConverter());
            Assert.Null(result);
        }

        // ── Read — single embedded entry (embeddedEntry.json) ────────────────

        [Fact]
        public void Read_EntryJson_ReturnsEmbeddedObject()
        {
            var result = JsonSerializer.Deserialize<IEmbeddedObject>(
                ReadJson("embeddedEntry.json"), OptionsWithConverter());
            Assert.NotNull(result);
            Assert.IsType<EmbeddedObject>(result);
        }

        [Fact]
        public void Read_EntryJson_PopulatesUid()
        {
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.Equal("sample_author_uid", result.Uid);
        }

        [Fact]
        public void Read_EntryJson_PopulatesContentTypeUid()
        {
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.Equal("author", result.ContentTypeUid);
        }

        [Fact]
        public void Read_EntryJson_PopulatesTitle_ViaIEmbeddedEntry()
        {
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.Equal("Dummy User", (result as IEmbeddedEntry)?.Title);
        }

        [Fact]
        public void Read_EntryJson_CustomFields_CapturedInFields()
        {
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.True(result.Fields.ContainsKey("bio"));
            Assert.True(result.Fields.ContainsKey("email"));
            Assert.True(result.Fields.ContainsKey("avatar_url"));
            Assert.Equal(
                "This is a dummy bio used for testing purposes.",
                result.Fields["bio"].GetString());
            Assert.Equal("dummy.user@example.com", result.Fields["email"].GetString());
        }

        [Fact]
        public void Read_EntryJson_NestedObjectField_CapturedInFields()
        {
            // social is a nested object — must land in Fields as an object element, not be dropped.
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.True(result.Fields.ContainsKey("social"));
            var social = result.Fields["social"];
            Assert.Equal(JsonValueKind.Object, social.ValueKind);
            Assert.Equal("@dummyuser", social.GetProperty("twitter").GetString());
            Assert.Equal("linkedin.com/in/dummyuser", social.GetProperty("linkedin").GetString());
        }

        [Fact]
        public void Read_EntryJson_ArrayField_CapturedInFields()
        {
            // tags is a JSON array — must land in Fields as an array element.
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.True(result.Fields.ContainsKey("tags"));
            var tags = result.Fields["tags"];
            Assert.Equal(JsonValueKind.Array, tags.ValueKind);
            Assert.Equal(3, tags.GetArrayLength());
            var tagValues = tags.EnumerateArray().Select(e => e.GetString()).ToList();
            Assert.Contains("sample", tagValues);
        }

        [Fact]
        public void Read_EntryJson_KnownFields_NotDuplicatedInExtensionData()
        {
            // uid, _content_type_uid, title are declared properties — must NOT appear in Fields.
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.False(result.Fields.ContainsKey("uid"));
            Assert.False(result.Fields.ContainsKey("_content_type_uid"));
            Assert.False(result.Fields.ContainsKey("title"));
        }

        // ── Read — single embedded asset (embeddedAsset.json) ────────────────

        [Fact]
        public void Read_AssetJson_PopulatesFileName_ViaIEmbeddedAsset()
        {
            var result = DeserializeSingle(ReadJson("embeddedAsset.json"));
            var asAsset = result as IEmbeddedAsset;
            Assert.NotNull(asAsset);
            Assert.Equal("dummy-image.jpg", asAsset.FileName);
        }

        [Fact]
        public void Read_AssetJson_PopulatesUrl()
        {
            var result = DeserializeSingle(ReadJson("embeddedAsset.json"));
            Assert.Contains("dummy-image.jpg", result.Url);
        }

        [Fact]
        public void Read_AssetJson_ContentTypeUid_IsSysAssets()
        {
            var result = DeserializeSingle(ReadJson("embeddedAsset.json"));
            Assert.Equal("sys_assets", result.ContentTypeUid);
        }

        [Fact]
        public void Read_AssetJson_DimensionObject_CapturedInFields()
        {
            var result = DeserializeSingle(ReadJson("embeddedAsset.json"));
            Assert.True(result.Fields.ContainsKey("dimension"));
            var dimension = result.Fields["dimension"];
            Assert.Equal(JsonValueKind.Object, dimension.ValueKind);
            Assert.Equal(100, dimension.GetProperty("height").GetInt32());
            Assert.Equal(100, dimension.GetProperty("width").GetInt32());
        }

        [Fact]
        public void Read_AssetJson_FileSize_CapturedInFields()
        {
            var result = DeserializeSingle(ReadJson("embeddedAsset.json"));
            Assert.True(result.Fields.ContainsKey("file_size"));
            Assert.Equal("10000", result.Fields["file_size"].GetString());
        }

        // ── Integration — List<IEmbeddedObject> (embeddedItems.json) ─────────

        [Fact]
        public void Deserialize_ListOfIEmbeddedObject_WithConverter_Succeeds()
        {
            var result = JsonSerializer.Deserialize<List<IEmbeddedObject>>(
                ReadJson("embeddedItems.json"), OptionsWithConverter());
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, item => Assert.IsType<EmbeddedObject>(item));
        }

        [Fact]
        public void Deserialize_ListOfIEmbeddedObject_WithoutConverter_Throws()
        {
            // Reproduces the customer crash: System.Text.Json cannot instantiate the
            // IEmbeddedObject interface without the converter registered.
            Assert.Throws<NotSupportedException>(() =>
                JsonSerializer.Deserialize<List<IEmbeddedObject>>(ReadJson("embeddedItems.json")));
        }

        [Fact]
        public void Deserialize_ListItem_Entry_HasCorrectKnownFields()
        {
            var result = JsonSerializer.Deserialize<List<IEmbeddedObject>>(
                ReadJson("embeddedItems.json"), OptionsWithConverter());
            var entry = result[0] as EmbeddedObject;
            Assert.Equal("sample_author_uid", entry.Uid);
            Assert.Equal("author", entry.ContentTypeUid);
            Assert.Equal("Dummy User", entry.Title);
        }

        [Fact]
        public void Deserialize_ListItem_Entry_HasCustomFields()
        {
            var result = JsonSerializer.Deserialize<List<IEmbeddedObject>>(
                ReadJson("embeddedItems.json"), OptionsWithConverter());
            var entry = result[0] as EmbeddedObject;
            Assert.Equal("dummy.user@example.com", entry.Fields["email"].GetString());
            Assert.Equal("https://example.com/dummy-avatar.jpg",
                entry.Fields["avatar_url"].GetString());
        }

        [Fact]
        public void Deserialize_ListItem_FirstAsset_HasCorrectFields()
        {
            var result = JsonSerializer.Deserialize<List<IEmbeddedObject>>(
                ReadJson("embeddedItems.json"), OptionsWithConverter());
            var asset = result[1] as EmbeddedObject;
            Assert.Equal("sample_asset_uid", asset.Uid);
            Assert.Equal("sys_assets", asset.ContentTypeUid);
            Assert.Equal("dummy-image.jpg", asset.FileName);
        }

        [Fact]
        public void Deserialize_ListItem_SecondAsset_HasCorrectFields()
        {
            var result = JsonSerializer.Deserialize<List<IEmbeddedObject>>(
                ReadJson("embeddedItems.json"), OptionsWithConverter());
            var asset = result[2] as EmbeddedObject;
            Assert.Equal("sample_asset_uid_2", asset.Uid);
            Assert.Equal("dummy-image.png", asset.FileName);
        }

        // ── Full entry deserialization (rteEntryWithEmbeddedItems.json) ───────
        // Mirrors the actual Delivery API response shape: { "entry": { ..., "_embedded_items": { ... } } }

        private static string ReadEmbeddedItemsToken(string fileName)
        {
            var root = JsonNode.Parse(ReadJson(fileName));
            return root["entry"]["_embedded_items"]["rte_json"].ToJsonString();
        }

        [Fact]
        public void FullEntry_EmbeddedItems_DeserializesAllThreeItems()
        {
            var result = JsonSerializer.Deserialize<List<IEmbeddedObject>>(
                ReadEmbeddedItemsToken("rteEntryWithEmbeddedItems.json"), OptionsWithConverter());

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void FullEntry_EmbeddedItems_FirstItem_IsAuthorEntry()
        {
            var result = JsonSerializer.Deserialize<List<IEmbeddedObject>>(
                ReadEmbeddedItemsToken("rteEntryWithEmbeddedItems.json"), OptionsWithConverter());

            var author = result[0] as EmbeddedObject;
            Assert.Equal("author", author.ContentTypeUid);
            Assert.Equal("Dummy User", author.Title);
            Assert.Equal("dummy.user@example.com", author.Fields["email"].GetString());
        }

        [Fact]
        public void FullEntry_EmbeddedItems_SecondAndThirdItems_AreAssets()
        {
            var result = JsonSerializer.Deserialize<List<IEmbeddedObject>>(
                ReadEmbeddedItemsToken("rteEntryWithEmbeddedItems.json"), OptionsWithConverter());

            Assert.Equal("sys_assets", (result[1] as EmbeddedObject)?.ContentTypeUid);
            Assert.Equal("sys_assets", (result[2] as EmbeddedObject)?.ContentTypeUid);
        }

        [Fact]
        public void FullEntry_EmbeddedItems_AssetDimension_CapturedInFields()
        {
            var result = JsonSerializer.Deserialize<List<IEmbeddedObject>>(
                ReadEmbeddedItemsToken("rteEntryWithEmbeddedItems.json"), OptionsWithConverter());

            var asset = result[1] as EmbeddedObject;
            var dimension = asset.Fields["dimension"];
            Assert.Equal(JsonValueKind.Object, dimension.ValueKind);
            Assert.Equal(100, dimension.GetProperty("height").GetInt32());
            Assert.Equal(100, dimension.GetProperty("width").GetInt32());
        }

        [Fact]
        public void FullEntry_EmbeddedItems_AuthorSocialObject_CapturedInFields()
        {
            var result = JsonSerializer.Deserialize<List<IEmbeddedObject>>(
                ReadEmbeddedItemsToken("rteEntryWithEmbeddedItems.json"), OptionsWithConverter());

            var author = result[0] as EmbeddedObject;
            var social = author.Fields["social"];
            Assert.Equal(JsonValueKind.Object, social.ValueKind);
            Assert.Equal("@dummyuser", social.GetProperty("twitter").GetString());
        }

        // ── EmbeddedObject default state ──────────────────────────────────────

        [Fact]
        public void EmbeddedObject_DefaultValues_AreEmptyStrings()
        {
            var obj = new EmbeddedObject();
            Assert.Equal(string.Empty, obj.Uid);
            Assert.Equal(string.Empty, obj.ContentTypeUid);
            Assert.Equal(string.Empty, obj.Title);
            Assert.Equal(string.Empty, obj.FileName);
            Assert.Equal(string.Empty, obj.Url);
        }

        [Fact]
        public void EmbeddedObject_Fields_DefaultsToEmptyDictionary()
        {
            var obj = new EmbeddedObject();
            Assert.NotNull(obj.Fields);
            Assert.Empty(obj.Fields);
        }

        [Fact]
        public void EmbeddedObject_ImplementsIEmbeddedEntry()
        {
            Assert.True(typeof(IEmbeddedEntry).IsAssignableFrom(typeof(EmbeddedObject)));
        }

        [Fact]
        public void EmbeddedObject_ImplementsIEmbeddedAsset()
        {
            Assert.True(typeof(IEmbeddedAsset).IsAssignableFrom(typeof(EmbeddedObject)));
        }

        [Fact]
        public void EmbeddedObject_ImplementsIEmbeddedObject()
        {
            Assert.True(typeof(IEmbeddedObject).IsAssignableFrom(typeof(EmbeddedObject)));
        }

        // ── Customer-defined subclass (CanConvert isolation check) ────────────

        private class CustomerDefinedEmbeddedObject : IEmbeddedObject
        {
            public string Uid { get; set; }
            public string ContentTypeUid { get; set; }
        }
    }
}
