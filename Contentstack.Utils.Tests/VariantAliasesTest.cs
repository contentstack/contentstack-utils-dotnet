using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Contentstack.Utils.Tests
{
    public class VariantAliasesTest
    {
        private static JObject ReadJsonRoot(string fileName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Resources", fileName);
            return JObject.Parse(File.ReadAllText(path));
        }

        private static HashSet<string> JsonArrayToStringSet(JArray arr)
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
            JObject full = ReadJsonRoot("variantsSingleEntry.json");
            JObject entry = (JObject)full["entry"];
            const string contentTypeUid = "movie";

            JObject result = Utils.GetVariantAliases(entry, contentTypeUid);

            Assert.True(result["entry_uid"] != null && !string.IsNullOrEmpty(result["entry_uid"].ToString()));
            Assert.Equal(contentTypeUid, result["contenttype_uid"].ToString());
            JArray variants = (JArray)result["variants"];
            Assert.NotNull(variants);
            var aliasSet = JsonArrayToStringSet(variants);
            Assert.Equal(
                new HashSet<string> { "cs_personalize_0_0", "cs_personalize_0_3" },
                aliasSet);
        }

        [Fact]
        public void GetDataCsvariantsAttribute_SingleEntry_ReturnsJsonArrayString()
        {
            JObject full = ReadJsonRoot("variantsSingleEntry.json");
            JObject entry = (JObject)full["entry"];
            const string contentTypeUid = "movie";

            JObject result = Utils.GetDataCsvariantsAttribute(entry, contentTypeUid);

            Assert.True(result["data-csvariants"] != null);
            string dataCsvariantsStr = result["data-csvariants"].ToString();
            JArray arr = JArray.Parse(dataCsvariantsStr);
            Assert.Single(arr);
            JObject first = (JObject)arr[0];
            Assert.True(first["entry_uid"] != null && !string.IsNullOrEmpty(first["entry_uid"].ToString()));
            Assert.Equal(contentTypeUid, first["contenttype_uid"].ToString());
            var aliasSet = JsonArrayToStringSet((JArray)first["variants"]);
            Assert.Equal(
                new HashSet<string> { "cs_personalize_0_0", "cs_personalize_0_3" },
                aliasSet);
        }

        [Fact]
        public void GetVariantAliases_MultipleEntries_ReturnsOneResultPerEntryWithUid()
        {
            JObject full = ReadJsonRoot("variantsEntries.json");
            JArray entries = (JArray)full["entries"];
            const string contentTypeUid = "movie";

            JArray result = Utils.GetVariantAliases(entries, contentTypeUid);

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            JObject first = (JObject)result[0];
            Assert.True(first["entry_uid"] != null && !string.IsNullOrEmpty(first["entry_uid"].ToString()));
            Assert.Equal(contentTypeUid, first["contenttype_uid"].ToString());
            var firstSet = JsonArrayToStringSet((JArray)first["variants"]);
            Assert.Equal(
                new HashSet<string> { "cs_personalize_0_0", "cs_personalize_0_3" },
                firstSet);

            JObject second = (JObject)result[1];
            Assert.True(second["entry_uid"] != null && !string.IsNullOrEmpty(second["entry_uid"].ToString()));
            Assert.Single((JArray)second["variants"]);
            Assert.Equal("cs_personalize_0_0", ((JArray)second["variants"])[0].ToString());

            JObject third = (JObject)result[2];
            Assert.True(third["entry_uid"] != null && !string.IsNullOrEmpty(third["entry_uid"].ToString()));
            Assert.Empty((JArray)third["variants"]);
        }

        [Fact]
        public void GetDataCsvariantsAttribute_MultipleEntries_ReturnsJsonArrayString()
        {
            JObject full = ReadJsonRoot("variantsEntries.json");
            JArray entries = (JArray)full["entries"];
            const string contentTypeUid = "movie";

            JObject result = Utils.GetDataCsvariantsAttribute(entries, contentTypeUid);

            Assert.True(result["data-csvariants"] != null);
            string dataCsvariantsStr = result["data-csvariants"].ToString();
            JArray arr = JArray.Parse(dataCsvariantsStr);
            Assert.Equal(3, arr.Count);
            Assert.True(((JObject)arr[0])["entry_uid"] != null && !string.IsNullOrEmpty(((JObject)arr[0])["entry_uid"].ToString()));
            Assert.Equal(2, ((JArray)((JObject)arr[0])["variants"]).Count);
            Assert.True(((JObject)arr[1])["entry_uid"] != null && !string.IsNullOrEmpty(((JObject)arr[1])["entry_uid"].ToString()));
            Assert.Single((JArray)((JObject)arr[1])["variants"]);
            Assert.True(((JObject)arr[2])["entry_uid"] != null && !string.IsNullOrEmpty(((JObject)arr[2])["entry_uid"].ToString()));
            Assert.Empty((JArray)((JObject)arr[2])["variants"]);
        }

        [Fact]
        public void GetVariantAliases_ThrowsWhenEntryNull()
        {
            Assert.Throws<ArgumentException>(() => Utils.GetVariantAliases((JObject)null, "landing_page"));
        }

        [Fact]
        public void GetVariantAliases_ThrowsWhenContentTypeUidNull()
        {
            JObject full = ReadJsonRoot("variantsSingleEntry.json");
            JObject entry = (JObject)full["entry"];
            Assert.Throws<ArgumentException>(() => Utils.GetVariantAliases(entry, null));
        }

        [Fact]
        public void GetVariantAliases_ThrowsWhenContentTypeUidEmpty()
        {
            JObject full = ReadJsonRoot("variantsSingleEntry.json");
            JObject entry = (JObject)full["entry"];
            Assert.Throws<ArgumentException>(() => Utils.GetVariantAliases(entry, ""));
        }

        [Fact]
        public void GetDataCsvariantsAttribute_WhenEntryNull_ReturnsEmptyArrayString()
        {
            JObject result = Utils.GetDataCsvariantsAttribute((JObject)null, "landing_page");
            Assert.True(result["data-csvariants"] != null);
            Assert.Equal("[]", result["data-csvariants"].ToString());
        }
    }
}
