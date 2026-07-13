using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;
using Xunit;

namespace Contentstack.Utils.Tests
{
    public class VariantAliasesTest
    {
        private static JsonObject ReadJsonRoot(string fileName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Resources", fileName);
            return JsonNode.Parse(File.ReadAllText(path)).AsObject();
        }

        private static HashSet<string> JsonArrayToStringSet(JsonArray arr)
        {
            var set = new HashSet<string>();
            foreach (var t in arr)
            {
                set.Add(t.ToString());
            }
            return set;
        }

        [Fact]
        public void GetVariantAliases_SingleEntry_ReturnsAliases()
        {
            JsonObject full = ReadJsonRoot("variantsSingleEntry.json");
            JsonObject entry = full["entry"].AsObject();
            const string contentTypeUid = "movie";

            JsonObject result = Utils.GetVariantAliases(entry, contentTypeUid);

            Assert.True(result["entry_uid"] != null && !string.IsNullOrEmpty(result["entry_uid"].ToString()));
            Assert.Equal(contentTypeUid, result["contenttype_uid"].ToString());
            JsonArray variants = result["variants"].AsArray();
            Assert.NotNull(variants);
            var aliasSet = JsonArrayToStringSet(variants);
            Assert.Equal(
                new HashSet<string> { "cs_personalize_0_0", "cs_personalize_0_3" },
                aliasSet);
        }

        [Fact]
        public void GetVariantMetadataTags_SingleEntry_ReturnsJsonArrayString()
        {
            JsonObject full = ReadJsonRoot("variantsSingleEntry.json");
            JsonObject entry = full["entry"].AsObject();
            const string contentTypeUid = "movie";

            JsonObject result = Utils.GetVariantMetadataTags(entry, contentTypeUid);

            Assert.True(result["data-csvariants"] != null);
            string dataCsvariantsStr = result["data-csvariants"].ToString();
            JsonArray arr = JsonNode.Parse(dataCsvariantsStr).AsArray();
            Assert.Single(arr);
            JsonObject first = arr[0].AsObject();
            Assert.True(first["entry_uid"] != null && !string.IsNullOrEmpty(first["entry_uid"].ToString()));
            Assert.Equal(contentTypeUid, first["contenttype_uid"].ToString());
            var aliasSet = JsonArrayToStringSet(first["variants"].AsArray());
            Assert.Equal(
                new HashSet<string> { "cs_personalize_0_0", "cs_personalize_0_3" },
                aliasSet);
        }

        [Fact]
        public void GetVariantAliases_MultipleEntries_ReturnsOneResultPerEntryWithUid()
        {
            JsonObject full = ReadJsonRoot("variantsEntries.json");
            JsonArray entries = full["entries"].AsArray();
            const string contentTypeUid = "movie";

            JsonArray result = Utils.GetVariantAliases(entries, contentTypeUid);

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            JsonObject first = result[0].AsObject();
            Assert.True(first["entry_uid"] != null && !string.IsNullOrEmpty(first["entry_uid"].ToString()));
            Assert.Equal(contentTypeUid, first["contenttype_uid"].ToString());
            var firstSet = JsonArrayToStringSet(first["variants"].AsArray());
            Assert.Equal(
                new HashSet<string> { "cs_personalize_0_0", "cs_personalize_0_3" },
                firstSet);

            JsonObject second = result[1].AsObject();
            Assert.True(second["entry_uid"] != null && !string.IsNullOrEmpty(second["entry_uid"].ToString()));
            Assert.Single(second["variants"].AsArray());
            Assert.Equal("cs_personalize_0_0", second["variants"].AsArray()[0].ToString());

            JsonObject third = result[2].AsObject();
            Assert.True(third["entry_uid"] != null && !string.IsNullOrEmpty(third["entry_uid"].ToString()));
            Assert.Empty(third["variants"].AsArray());
        }

        [Fact]
        public void GetVariantMetadataTags_MultipleEntries_ReturnsJsonArrayString()
        {
            JsonObject full = ReadJsonRoot("variantsEntries.json");
            JsonArray entries = full["entries"].AsArray();
            const string contentTypeUid = "movie";

            JsonObject result = Utils.GetVariantMetadataTags(entries, contentTypeUid);

            Assert.True(result["data-csvariants"] != null);
            string dataCsvariantsStr = result["data-csvariants"].ToString();
            JsonArray arr = JsonNode.Parse(dataCsvariantsStr).AsArray();
            Assert.Equal(3, arr.Count);
            Assert.True(arr[0].AsObject()["entry_uid"] != null && !string.IsNullOrEmpty(arr[0].AsObject()["entry_uid"].ToString()));
            Assert.Equal(2, arr[0].AsObject()["variants"].AsArray().Count);
            Assert.True(arr[1].AsObject()["entry_uid"] != null && !string.IsNullOrEmpty(arr[1].AsObject()["entry_uid"].ToString()));
            Assert.Single(arr[1].AsObject()["variants"].AsArray());
            Assert.True(arr[2].AsObject()["entry_uid"] != null && !string.IsNullOrEmpty(arr[2].AsObject()["entry_uid"].ToString()));
            Assert.Empty(arr[2].AsObject()["variants"].AsArray());
        }

        [Fact]
        public void GetVariantAliases_ThrowsWhenEntryNull()
        {
            Assert.Throws<ArgumentException>(() => Utils.GetVariantAliases((JsonObject)null, "landing_page"));
        }

        [Fact]
        public void GetVariantAliases_ThrowsWhenContentTypeUidNull()
        {
            JsonObject full = ReadJsonRoot("variantsSingleEntry.json");
            JsonObject entry = full["entry"].AsObject();
            Assert.Throws<ArgumentException>(() => Utils.GetVariantAliases(entry, null));
        }

        [Fact]
        public void GetVariantAliases_ThrowsWhenContentTypeUidEmpty()
        {
            JsonObject full = ReadJsonRoot("variantsSingleEntry.json");
            JsonObject entry = full["entry"].AsObject();
            Assert.Throws<ArgumentException>(() => Utils.GetVariantAliases(entry, ""));
        }

        [Fact]
        public void GetVariantMetadataTags_WhenEntryNull_ReturnsEmptyArrayString()
        {
            JsonObject result = Utils.GetVariantMetadataTags((JsonObject)null, "landing_page");
            Assert.True(result["data-csvariants"] != null);
            Assert.Equal("[]", result["data-csvariants"].ToString());
        }

        [Fact]
        public void GetVariantAliases_ThrowsWhenUidMissing()
        {
            var entry = new JsonObject { ["title"] = "no-uid" };
            Assert.Throws<ArgumentException>(() => Utils.GetVariantAliases(entry, "movie"));
        }

        [Fact]
        public void GetVariantAliases_ThrowsWhenUidNull()
        {
            var entry = new JsonObject { ["uid"] = JsonNode.Parse("null") };
            Assert.Throws<ArgumentException>(() => Utils.GetVariantAliases(entry, "movie"));
        }

        [Fact]
        public void GetVariantAliases_Batch_ThrowsWhenContentTypeUidNull()
        {
            var entries = new JsonArray { new JsonObject { ["uid"] = "a" } };
            Assert.Throws<ArgumentException>(() => Utils.GetVariantAliases(entries, null));
        }

        [Fact]
        public void GetVariantAliases_Batch_ThrowsWhenContentTypeUidEmpty()
        {
            var entries = new JsonArray { new JsonObject { ["uid"] = "a" } };
            Assert.Throws<ArgumentException>(() => Utils.GetVariantAliases(entries, ""));
        }

        [Fact]
        public void GetVariantMetadataTags_WhenEntriesArrayNull_ReturnsEmptyArrayString()
        {
            JsonObject result = Utils.GetVariantMetadataTags((JsonArray)null, "movie");
            Assert.Equal("[]", result["data-csvariants"].ToString());
        }

        [Fact]
        public void GetVariantMetadataTags_Batch_ThrowsWhenContentTypeUidNull()
        {
            var entries = new JsonArray { new JsonObject { ["uid"] = "a" } };
            Assert.Throws<ArgumentException>(() => Utils.GetVariantMetadataTags(entries, null));
        }

        [Fact]
        public void GetVariantMetadataTags_Batch_ThrowsWhenContentTypeUidEmpty()
        {
            var entries = new JsonArray { new JsonObject { ["uid"] = "a" } };
            Assert.Throws<ArgumentException>(() => Utils.GetVariantMetadataTags(entries, ""));
        }

        [Fact]
        public void GetDataCsvariantsAttribute_DelegatesToGetVariantMetadataTags()
        {
#pragma warning disable CS0618 // Type or member is obsolete — intentional coverage of backward-compatible alias
            JsonObject full = ReadJsonRoot("variantsSingleEntry.json");
            JsonObject entry = full["entry"].AsObject();
            const string contentTypeUid = "movie";

            JsonObject canonical = Utils.GetVariantMetadataTags(entry, contentTypeUid);
            JsonObject legacy = Utils.GetDataCsvariantsAttribute(entry, contentTypeUid);
            Assert.True(JsonNode.DeepEquals(canonical, legacy));

            JsonObject fullMulti = ReadJsonRoot("variantsEntries.json");
            JsonArray entries = fullMulti["entries"].AsArray();
            JsonObject canonicalBatch = Utils.GetVariantMetadataTags(entries, contentTypeUid);
            JsonObject legacyBatch = Utils.GetDataCsvariantsAttribute(entries, contentTypeUid);
            Assert.True(JsonNode.DeepEquals(canonicalBatch, legacyBatch));

            JsonObject nullEntryLegacy = Utils.GetDataCsvariantsAttribute((JsonObject)null, "x");
            JsonObject nullEntryCanonical = Utils.GetVariantMetadataTags((JsonObject)null, "x");
            Assert.True(JsonNode.DeepEquals(nullEntryCanonical, nullEntryLegacy));

            JsonObject nullArrLegacy = Utils.GetDataCsvariantsAttribute((JsonArray)null, "x");
            JsonObject nullArrCanonical = Utils.GetVariantMetadataTags((JsonArray)null, "x");
            Assert.True(JsonNode.DeepEquals(nullArrCanonical, nullArrLegacy));
#pragma warning restore CS0618
        }

        [Fact]
        public void GetVariantAliases_ReturnsEmptyVariantsWhenPublishDetailsMissing()
        {
            var entry = new JsonObject { ["uid"] = "blt_no_pd" };
            JsonObject result = Utils.GetVariantAliases(entry, "movie");
            Assert.Equal("blt_no_pd", result["entry_uid"].ToString());
            Assert.Equal("movie", result["contenttype_uid"].ToString());
            Assert.Empty(result["variants"].AsArray());
        }

        [Fact]
        public void GetVariantAliases_ReturnsEmptyVariantsWhenVariantsObjectEmpty()
        {
            var entry = new JsonObject
            {
                ["uid"] = "blt_empty_v",
                ["publish_details"] = new JsonObject
                {
                    ["variants"] = new JsonObject()
                }
            };
            JsonObject result = Utils.GetVariantAliases(entry, "movie");
            Assert.Empty(result["variants"].AsArray());
        }

        [Fact]
        public void GetVariantAliases_ReturnsEmptyVariantsWhenVariantsKeyMissing()
        {
            var entry = new JsonObject
            {
                ["uid"] = "blt_no_variants_key",
                ["publish_details"] = new JsonObject { ["time"] = "2025-01-01T00:00:00.000Z" }
            };
            JsonObject result = Utils.GetVariantAliases(entry, "movie");
            Assert.Empty(result["variants"].AsArray());
        }

        [Fact]
        public void GetVariantAliases_SkipsVariantWhenAliasMissingOrEmpty()
        {
            var entry = new JsonObject
            {
                ["uid"] = "blt_skip",
                ["publish_details"] = new JsonObject
                {
                    ["variants"] = new JsonObject
                    {
                        ["v1"] = new JsonObject { ["alias"] = "keep_me" },
                        ["v2"] = new JsonObject(),
                        ["v3"] = new JsonObject { ["alias"] = "" }
                    }
                }
            };
            JsonObject result = Utils.GetVariantAliases(entry, "page");
            var variants = result["variants"].AsArray();
            Assert.Single(variants);
            Assert.Equal("keep_me", variants[0].ToString());
        }
    }
}
