using System;
using System.Collections.Generic;
using System.IO;
using Contentstack.Utils.Converters;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        private static JsonSerializerSettings SettingsWithConverter()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new EmbeddedObjectConverter());
            return settings;
        }

        private static EmbeddedObject DeserializeSingle(string json)
        {
            return (EmbeddedObject)JsonConvert.DeserializeObject<IEmbeddedObject>(
                json, SettingsWithConverter());
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
            // Concrete class must fall through to Newtonsoft default property mapping.
            Assert.False(_converter.CanConvert(typeof(EmbeddedObject)));
        }

        [Fact]
        public void CanConvert_CustomerSubclass_ReturnsFalse()
        {
            // Customer-defined subclasses must not be intercepted.
            Assert.False(_converter.CanConvert(typeof(CustomerDefinedEmbeddedObject)));
        }

        [Fact]
        public void CanWrite_IsFalse()
        {
            Assert.False(_converter.CanWrite);
        }

        // ── EmbeddedObjectConverter.WriteJson ─────────────────────────────────

        [Fact]
        public void WriteJson_ThrowsNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() =>
                _converter.WriteJson(null, new EmbeddedObject(), new JsonSerializer()));
        }

        // ── ReadJson — null token ─────────────────────────────────────────────

        [Fact]
        public void ReadJson_NullToken_ReturnsNull()
        {
            var result = JsonConvert.DeserializeObject<IEmbeddedObject>("null", SettingsWithConverter());
            Assert.Null(result);
        }

        // ── ReadJson — single embedded entry (embeddedEntry.json) ────────────

        [Fact]
        public void ReadJson_EntryJson_ReturnsEmbeddedObject()
        {
            var result = JsonConvert.DeserializeObject<IEmbeddedObject>(
                ReadJson("embeddedEntry.json"), SettingsWithConverter());
            Assert.NotNull(result);
            Assert.IsType<EmbeddedObject>(result);
        }

        [Fact]
        public void ReadJson_EntryJson_PopulatesUid()
        {
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.Equal("sample_author_uid", result.Uid);
        }

        [Fact]
        public void ReadJson_EntryJson_PopulatesContentTypeUid()
        {
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.Equal("author", result.ContentTypeUid);
        }

        [Fact]
        public void ReadJson_EntryJson_PopulatesTitle_ViaIEmbeddedEntry()
        {
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.Equal("Dummy User", (result as IEmbeddedEntry)?.Title);
        }

        [Fact]
        public void ReadJson_EntryJson_CustomFields_CapturedInFields()
        {
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.True(result.Fields.ContainsKey("bio"));
            Assert.True(result.Fields.ContainsKey("email"));
            Assert.True(result.Fields.ContainsKey("avatar_url"));
            Assert.Equal(
                "This is a dummy bio used for testing purposes.",
                result.Fields["bio"].ToString());
            Assert.Equal("dummy.user@example.com", result.Fields["email"].ToString());
        }

        [Fact]
        public void ReadJson_EntryJson_NestedObjectField_CapturedInFields()
        {
            // social is a nested object — must land in Fields as a JObject, not be dropped.
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.True(result.Fields.ContainsKey("social"));
            var social = result.Fields["social"] as JObject;
            Assert.NotNull(social);
            Assert.Equal("@dummyuser", social["twitter"]?.ToString());
            Assert.Equal("linkedin.com/in/dummyuser", social["linkedin"]?.ToString());
        }

        [Fact]
        public void ReadJson_EntryJson_ArrayField_CapturedInFields()
        {
            // tags is a JSON array — must land in Fields as a JArray.
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.True(result.Fields.ContainsKey("tags"));
            var tags = result.Fields["tags"] as JArray;
            Assert.NotNull(tags);
            Assert.Equal(3, tags.Count);
            Assert.Contains("sample", tags.ToObject<List<string>>());
        }

        [Fact]
        public void ReadJson_EntryJson_KnownFields_NotDuplicatedInExtensionData()
        {
            // uid, _content_type_uid, title are declared properties — must NOT appear in Fields.
            var result = DeserializeSingle(ReadJson("embeddedEntry.json"));
            Assert.False(result.Fields.ContainsKey("uid"));
            Assert.False(result.Fields.ContainsKey("_content_type_uid"));
            Assert.False(result.Fields.ContainsKey("title"));
        }

        // ── ReadJson — single embedded asset (embeddedAsset.json) ────────────

        [Fact]
        public void ReadJson_AssetJson_PopulatesFileName_ViaIEmbeddedAsset()
        {
            var result = DeserializeSingle(ReadJson("embeddedAsset.json"));
            var asAsset = result as IEmbeddedAsset;
            Assert.NotNull(asAsset);
            Assert.Equal("dummy-image.jpg", asAsset.FileName);
        }

        [Fact]
        public void ReadJson_AssetJson_PopulatesUrl()
        {
            var result = DeserializeSingle(ReadJson("embeddedAsset.json"));
            Assert.Contains("dummy-image.jpg", result.Url);
        }

        [Fact]
        public void ReadJson_AssetJson_ContentTypeUid_IsSysAssets()
        {
            var result = DeserializeSingle(ReadJson("embeddedAsset.json"));
            Assert.Equal("sys_assets", result.ContentTypeUid);
        }

        [Fact]
        public void ReadJson_AssetJson_DimensionObject_CapturedInFields()
        {
            var result = DeserializeSingle(ReadJson("embeddedAsset.json"));
            Assert.True(result.Fields.ContainsKey("dimension"));
            var dimension = result.Fields["dimension"] as JObject;
            Assert.NotNull(dimension);
            Assert.Equal(100, dimension["height"]?.Value<int>());
            Assert.Equal(100, dimension["width"]?.Value<int>());
        }

        [Fact]
        public void ReadJson_AssetJson_FileSize_CapturedInFields()
        {
            var result = DeserializeSingle(ReadJson("embeddedAsset.json"));
            Assert.True(result.Fields.ContainsKey("file_size"));
            Assert.Equal("10000", result.Fields["file_size"].ToString());
        }

        // ── Integration — List<IEmbeddedObject> (embeddedItems.json) ─────────

        [Fact]
        public void Deserialize_ListOfIEmbeddedObject_WithConverter_Succeeds()
        {
            var result = JsonConvert.DeserializeObject<List<IEmbeddedObject>>(
                ReadJson("embeddedItems.json"), SettingsWithConverter());
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, item => Assert.IsType<EmbeddedObject>(item));
        }

        [Fact]
        public void Deserialize_ListOfIEmbeddedObject_WithoutConverter_ThrowsJsonSerializationException()
        {
            // Reproduces the customer crash on contentstack.csharp v2.27.0.
            Assert.Throws<JsonSerializationException>(() =>
                JsonConvert.DeserializeObject<List<IEmbeddedObject>>(ReadJson("embeddedItems.json")));
        }

        [Fact]
        public void Deserialize_ListItem_Entry_HasCorrectKnownFields()
        {
            var result = JsonConvert.DeserializeObject<List<IEmbeddedObject>>(
                ReadJson("embeddedItems.json"), SettingsWithConverter());
            var entry = result[0] as EmbeddedObject;
            Assert.Equal("sample_author_uid", entry.Uid);
            Assert.Equal("author", entry.ContentTypeUid);
            Assert.Equal("Dummy User", entry.Title);
        }

        [Fact]
        public void Deserialize_ListItem_Entry_HasCustomFields()
        {
            var result = JsonConvert.DeserializeObject<List<IEmbeddedObject>>(
                ReadJson("embeddedItems.json"), SettingsWithConverter());
            var entry = result[0] as EmbeddedObject;
            Assert.Equal("dummy.user@example.com", entry.Fields["email"].ToString());
            Assert.Equal("https://example.com/dummy-avatar.jpg",
                entry.Fields["avatar_url"].ToString());
        }

        [Fact]
        public void Deserialize_ListItem_FirstAsset_HasCorrectFields()
        {
            var result = JsonConvert.DeserializeObject<List<IEmbeddedObject>>(
                ReadJson("embeddedItems.json"), SettingsWithConverter());
            var asset = result[1] as EmbeddedObject;
            Assert.Equal("sample_asset_uid", asset.Uid);
            Assert.Equal("sys_assets", asset.ContentTypeUid);
            Assert.Equal("dummy-image.jpg", asset.FileName);
        }

        [Fact]
        public void Deserialize_ListItem_SecondAsset_HasCorrectFields()
        {
            var result = JsonConvert.DeserializeObject<List<IEmbeddedObject>>(
                ReadJson("embeddedItems.json"), SettingsWithConverter());
            var asset = result[2] as EmbeddedObject;
            Assert.Equal("sample_asset_uid_2", asset.Uid);
            Assert.Equal("dummy-image.png", asset.FileName);
        }

        // ── Full entry deserialization (rteEntryWithEmbeddedItems.json) ───────
        // Mirrors the actual Delivery API response shape: { "entry": { ..., "_embedded_items": { ... } } }

        [Fact]
        public void FullEntry_EmbeddedItems_DeserializesAllThreeItems()
        {
            var root = JObject.Parse(ReadJson("rteEntryWithEmbeddedItems.json"));
            var embeddedItemsToken = root["entry"]["_embedded_items"]["rte_json"].ToString();

            var result = JsonConvert.DeserializeObject<List<IEmbeddedObject>>(
                embeddedItemsToken, SettingsWithConverter());

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void FullEntry_EmbeddedItems_FirstItem_IsAuthorEntry()
        {
            var root = JObject.Parse(ReadJson("rteEntryWithEmbeddedItems.json"));
            var embeddedItemsToken = root["entry"]["_embedded_items"]["rte_json"].ToString();
            var result = JsonConvert.DeserializeObject<List<IEmbeddedObject>>(
                embeddedItemsToken, SettingsWithConverter());

            var author = result[0] as EmbeddedObject;
            Assert.Equal("author", author.ContentTypeUid);
            Assert.Equal("Dummy User", author.Title);
            Assert.Equal("dummy.user@example.com", author.Fields["email"].ToString());
        }

        [Fact]
        public void FullEntry_EmbeddedItems_SecondAndThirdItems_AreAssets()
        {
            var root = JObject.Parse(ReadJson("rteEntryWithEmbeddedItems.json"));
            var embeddedItemsToken = root["entry"]["_embedded_items"]["rte_json"].ToString();
            var result = JsonConvert.DeserializeObject<List<IEmbeddedObject>>(
                embeddedItemsToken, SettingsWithConverter());

            Assert.Equal("sys_assets", (result[1] as EmbeddedObject)?.ContentTypeUid);
            Assert.Equal("sys_assets", (result[2] as EmbeddedObject)?.ContentTypeUid);
        }

        [Fact]
        public void FullEntry_EmbeddedItems_AssetDimension_CapturedInFields()
        {
            var root = JObject.Parse(ReadJson("rteEntryWithEmbeddedItems.json"));
            var embeddedItemsToken = root["entry"]["_embedded_items"]["rte_json"].ToString();
            var result = JsonConvert.DeserializeObject<List<IEmbeddedObject>>(
                embeddedItemsToken, SettingsWithConverter());

            var asset = result[1] as EmbeddedObject;
            var dimension = asset.Fields["dimension"] as JObject;
            Assert.NotNull(dimension);
            Assert.Equal(100, dimension["height"]?.Value<int>());
            Assert.Equal(100, dimension["width"]?.Value<int>());
        }

        [Fact]
        public void FullEntry_EmbeddedItems_AuthorSocialObject_CapturedInFields()
        {
            var root = JObject.Parse(ReadJson("rteEntryWithEmbeddedItems.json"));
            var embeddedItemsToken = root["entry"]["_embedded_items"]["rte_json"].ToString();
            var result = JsonConvert.DeserializeObject<List<IEmbeddedObject>>(
                embeddedItemsToken, SettingsWithConverter());

            var author = result[0] as EmbeddedObject;
            var social = author.Fields["social"] as JObject;
            Assert.NotNull(social);
            Assert.Equal("@dummyuser", social["twitter"]?.ToString());
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
