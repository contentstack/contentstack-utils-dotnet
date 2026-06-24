using System;
using System.Collections.Generic;
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

        // ── JSON fixtures ─────────────────────────────────────────────────────

        private const string EntryJson = @"{
            ""uid"": ""blt123"",
            ""_content_type_uid"": ""author"",
            ""title"": ""John Smith"",
            ""bio"": ""Senior engineer"",
            ""email"": ""john@example.com""
        }";

        private const string AssetJson = @"{
            ""uid"": ""bltasset456"",
            ""_content_type_uid"": ""sys_assets"",
            ""filename"": ""photo.jpg"",
            ""url"": ""https://cdn.example.com/photo.jpg""
        }";

        private const string EmbeddedItemsJson = @"[
            {
                ""uid"": ""blt111"",
                ""_content_type_uid"": ""author"",
                ""title"": ""Alice"",
                ""bio"": ""Tech lead""
            },
            {
                ""uid"": ""blt222"",
                ""_content_type_uid"": ""sys_assets"",
                ""filename"": ""logo.png"",
                ""url"": ""https://cdn.example.com/logo.png""
            }
        ]";

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
            // Concrete class must fall through to Newtonsoft default mapping.
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

        // ── EmbeddedObjectConverter.ReadJson ─────────────────────────────────

        [Fact]
        public void ReadJson_NullToken_ReturnsNull()
        {
            var settings = SettingsWithConverter();
            var result = JsonConvert.DeserializeObject<IEmbeddedObject>("null", settings);
            Assert.Null(result);
        }

        [Fact]
        public void ReadJson_ValidEntryJson_ReturnsEmbeddedObject()
        {
            var settings = SettingsWithConverter();
            var result = JsonConvert.DeserializeObject<IEmbeddedObject>(EntryJson, settings);
            Assert.NotNull(result);
            Assert.IsType<EmbeddedObject>(result);
        }

        [Fact]
        public void ReadJson_PopulatesUid()
        {
            var result = DeserializeEntry(EntryJson);
            Assert.Equal("blt123", result.Uid);
        }

        [Fact]
        public void ReadJson_PopulatesContentTypeUid()
        {
            var result = DeserializeEntry(EntryJson);
            Assert.Equal("author", result.ContentTypeUid);
        }

        [Fact]
        public void ReadJson_PopulatesTitle_ViaIEmbeddedEntry()
        {
            var result = DeserializeEntry(EntryJson);
            var asEntry = result as IEmbeddedEntry;
            Assert.NotNull(asEntry);
            Assert.Equal("John Smith", asEntry.Title);
        }

        [Fact]
        public void ReadJson_PopulatesFileName_ViaIEmbeddedAsset()
        {
            var result = DeserializeEntry(AssetJson);
            var asAsset = result as IEmbeddedAsset;
            Assert.NotNull(asAsset);
            Assert.Equal("photo.jpg", asAsset.FileName);
            Assert.Equal("https://cdn.example.com/photo.jpg", asAsset.Url);
        }

        [Fact]
        public void ReadJson_CustomFields_CapturedViaExtensionData()
        {
            // bio and email are not declared on EmbeddedObject — they must land in Fields.
            var result = DeserializeEntry(EntryJson);
            Assert.NotNull(result.Fields);
            Assert.True(result.Fields.ContainsKey("bio"));
            Assert.True(result.Fields.ContainsKey("email"));
            Assert.Equal("Senior engineer", result.Fields["bio"].ToString());
            Assert.Equal("john@example.com", result.Fields["email"].ToString());
        }

        [Fact]
        public void ReadJson_KnownFields_NotDuplicatedInExtensionData()
        {
            // uid, _content_type_uid, title are declared — they must NOT appear in Fields.
            var result = DeserializeEntry(EntryJson);
            Assert.False(result.Fields.ContainsKey("uid"));
            Assert.False(result.Fields.ContainsKey("_content_type_uid"));
            Assert.False(result.Fields.ContainsKey("title"));
        }

        [Fact]
        public void ReadJson_NoCustomFields_FieldsIsEmpty()
        {
            var json = @"{ ""uid"": ""blt1"", ""_content_type_uid"": ""author"", ""title"": ""T"" }";
            var result = DeserializeEntry(json);
            Assert.Empty(result.Fields);
        }

        // ── Integration — List<IEmbeddedObject> deserialization ───────────────

        [Fact]
        public void Deserialize_ListOfIEmbeddedObject_WithConverter_Succeeds()
        {
            var settings = SettingsWithConverter();
            var result = JsonConvert.DeserializeObject<List<IEmbeddedObject>>(EmbeddedItemsJson, settings);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.IsType<EmbeddedObject>(item));
        }

        [Fact]
        public void Deserialize_ListOfIEmbeddedObject_WithoutConverter_ThrowsJsonSerializationException()
        {
            // Reproduces the customer crash on contentstack.csharp v2.27.0.
            Assert.Throws<JsonSerializationException>(() =>
                JsonConvert.DeserializeObject<List<IEmbeddedObject>>(EmbeddedItemsJson));
        }

        [Fact]
        public void Deserialize_ListOfIEmbeddedObject_FirstItem_HasCorrectFields()
        {
            var settings = SettingsWithConverter();
            var result = JsonConvert.DeserializeObject<List<IEmbeddedObject>>(EmbeddedItemsJson, settings);
            var first = result[0] as EmbeddedObject;
            Assert.Equal("blt111", first.Uid);
            Assert.Equal("author", first.ContentTypeUid);
            Assert.Equal("Alice", first.Title);
            Assert.Equal("Tech lead", first.Fields["bio"].ToString());
        }

        [Fact]
        public void Deserialize_ListOfIEmbeddedObject_SecondItem_IsAsset()
        {
            var settings = SettingsWithConverter();
            var result = JsonConvert.DeserializeObject<List<IEmbeddedObject>>(EmbeddedItemsJson, settings);
            var second = result[1] as EmbeddedObject;
            Assert.Equal("blt222", second.Uid);
            Assert.Equal("logo.png", second.FileName);
            Assert.Equal("https://cdn.example.com/logo.png", second.Url);
        }

        // ── EmbeddedObject defaults ───────────────────────────────────────────

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

        // ── Helpers ───────────────────────────────────────────────────────────

        private static JsonSerializerSettings SettingsWithConverter()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new EmbeddedObjectConverter());
            return settings;
        }

        private static EmbeddedObject DeserializeEntry(string json)
        {
            var settings = SettingsWithConverter();
            return (EmbeddedObject)JsonConvert.DeserializeObject<IEmbeddedObject>(json, settings);
        }

        // Customer-defined subclass — used to verify CanConvert does not over-intercept.
        private class CustomerDefinedEmbeddedObject : IEmbeddedObject
        {
            public string Uid { get; set; }
            public string ContentTypeUid { get; set; }
        }
    }
}
