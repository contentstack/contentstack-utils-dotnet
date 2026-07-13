using System;
using System.Collections.Generic;
using System.IO;
using Contentstack.Utils.Endpoints;
using Xunit;

namespace Contentstack.Utils.Tests
{
    public class EndpointTest : IDisposable
    {
        // Reset cache before each test so tests are isolated.
        public EndpointTest()
        {
            Endpoint.ResetCache();
        }

        public void Dispose()
        {
            Endpoint.ResetCache();
        }

        // ------------------------------------------------------------------
        // Basic resolution
        // ------------------------------------------------------------------

        [Fact]
        public void GetContentstackEndpoint_Na_ReturnsCorrectCdnUrl()
        {
            string url = Endpoint.GetContentstackEndpoint("na", "contentDelivery");
            Assert.Equal("https://cdn.contentstack.io", url);
        }

        [Theory]
        [InlineData("na")]
        [InlineData("eu")]
        [InlineData("au")]
        [InlineData("azure-na")]
        [InlineData("azure-eu")]
        [InlineData("gcp-na")]
        [InlineData("gcp-eu")]
        public void GetContentstackEndpoint_AllRegionIds_Resolve(string regionId)
        {
            string url = Endpoint.GetContentstackEndpoint(regionId, "contentDelivery");
            Assert.False(string.IsNullOrEmpty(url));
            Assert.StartsWith("https://", url);
        }

        // ------------------------------------------------------------------
        // Alias resolution (case-insensitive, dash/underscore variants)
        // ------------------------------------------------------------------

        [Theory]
        [InlineData("na")]
        [InlineData("us")]
        [InlineData("NA")]
        [InlineData("US")]
        [InlineData("AWS-NA")]
        [InlineData("aws_na")]
        [InlineData("AWS_NA")]
        public void GetContentstackEndpoint_NaAliasVariants_AllResolveToSameCdn(string alias)
        {
            string url = Endpoint.GetContentstackEndpoint(alias, "contentDelivery");
            Assert.Equal("https://cdn.contentstack.io", url);
        }

        [Theory]
        [InlineData("azure-na")]
        [InlineData("azure_na")]
        [InlineData("AZURE-NA")]
        [InlineData("AZURE_NA")]
        public void GetContentstackEndpoint_AzureNaAliasVariants_AllResolveToSameUrl(string alias)
        {
            string expected = Endpoint.GetContentstackEndpoint("azure-na", "contentDelivery");
            string result = Endpoint.GetContentstackEndpoint(alias, "contentDelivery");
            Assert.Equal(expected, result);
        }

        // ------------------------------------------------------------------
        // omitHttps flag
        // ------------------------------------------------------------------

        [Fact]
        public void GetContentstackEndpoint_OmitHttps_StripsScheme()
        {
            string host = Endpoint.GetContentstackEndpoint("na", "contentDelivery", omitHttps: true);
            Assert.Equal("cdn.contentstack.io", host);
        }

        [Fact]
        public void GetContentstackEndpoint_OmitHttps_EuRegion_StripsScheme()
        {
            string host = Endpoint.GetContentstackEndpoint("eu", "contentDelivery", omitHttps: true);
            Assert.Equal("eu-cdn.contentstack.com", host);
        }

        [Fact]
        public void GetContentstackEndpoint_OmitHttpsFalse_ReturnsFullUrl()
        {
            string url = Endpoint.GetContentstackEndpoint("na", "contentDelivery", omitHttps: false);
            Assert.StartsWith("https://", url);
        }

        // ------------------------------------------------------------------
        // All-endpoints (dict) overload
        // ------------------------------------------------------------------

        [Theory]
        [InlineData("na")]
        [InlineData("eu")]
        [InlineData("au")]
        [InlineData("azure-na")]
        [InlineData("azure-eu")]
        [InlineData("gcp-na")]
        [InlineData("gcp-eu")]
        public void GetContentstackEndpoint_AllEndpoints_ContainsExpectedKeys(string regionId)
        {
            var endpoints = Endpoint.GetContentstackEndpoint(regionId);
            Assert.True(endpoints.Count > 0);
            Assert.True(endpoints.ContainsKey("contentDelivery"));
            Assert.True(endpoints.ContainsKey("contentManagement"));
            Assert.True(endpoints.ContainsKey("auth"));
        }

        [Fact]
        public void GetContentstackEndpoint_NaAllEndpoints_Has18Keys()
        {
            var endpoints = Endpoint.GetContentstackEndpoint("na");
            Assert.Equal(18, endpoints.Count);
        }

        [Fact]
        public void GetContentstackEndpoint_AllEndpoints_OmitHttps_AllValuesStripped()
        {
            var endpoints = Endpoint.GetContentstackEndpoint("na", omitHttps: true);
            foreach (var url in endpoints.Values)
                Assert.DoesNotContain("https://", url);
        }

        // ------------------------------------------------------------------
        // Error handling
        // ------------------------------------------------------------------

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void GetContentstackEndpoint_EmptyRegion_ThrowsArgumentException(string emptyRegion)
        {
            Assert.Throws<ArgumentException>(() =>
                Endpoint.GetContentstackEndpoint(emptyRegion, "contentDelivery"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void GetContentstackEndpoint_EmptyRegion_DictOverload_ThrowsArgumentException(string emptyRegion)
        {
            Assert.Throws<ArgumentException>(() =>
                Endpoint.GetContentstackEndpoint(emptyRegion));
        }

        [Fact]
        public void GetContentstackEndpoint_UnknownRegion_ThrowsKeyNotFoundException()
        {
            Assert.Throws<KeyNotFoundException>(() =>
                Endpoint.GetContentstackEndpoint("xyz", "contentDelivery"));
        }

        [Fact]
        public void GetContentstackEndpoint_UnknownService_ThrowsKeyNotFoundException()
        {
            var ex = Assert.Throws<KeyNotFoundException>(() =>
                Endpoint.GetContentstackEndpoint("na", "nonExistentService"));
            Assert.Contains("nonExistentService", ex.Message);
        }

        // ------------------------------------------------------------------
        // Cache behaviour
        // ------------------------------------------------------------------

        [Fact]
        public void ResetCache_AllowsReload_NoError()
        {
            // First call populates cache
            Endpoint.GetContentstackEndpoint("na", "contentDelivery");

            // Reset and call again — should reload without error
            Endpoint.ResetCache();
            string url = Endpoint.GetContentstackEndpoint("na", "contentDelivery");
            Assert.Equal("https://cdn.contentstack.io", url);
        }

        [Fact]
        public void GetContentstackEndpoint_ConsecutiveCalls_ReturnConsistentResults()
        {
            string first = Endpoint.GetContentstackEndpoint("eu", "contentManagement");
            string second = Endpoint.GetContentstackEndpoint("eu", "contentManagement");
            Assert.Equal(first, second);
        }

        // ------------------------------------------------------------------
        // Utils proxy parity
        // ------------------------------------------------------------------

        [Fact]
        public void Utils_Proxy_String_MatchesEndpoint()
        {
            string direct = Endpoint.GetContentstackEndpoint("na", "contentDelivery");
            string proxy = Utils.GetContentstackEndpoint("na", "contentDelivery");
            Assert.Equal(direct, proxy);
        }

        [Fact]
        public void Utils_Proxy_Dict_MatchesEndpoint()
        {
            var direct = Endpoint.GetContentstackEndpoint("eu");
            var proxy = Utils.GetContentstackEndpoint("eu");
            Assert.Equal(direct.Count, proxy.Count);
            foreach (var kvp in direct)
                Assert.Equal(kvp.Value, proxy[kvp.Key]);
        }

        [Fact]
        public void Utils_Proxy_OmitHttps_MatchesEndpoint()
        {
            string direct = Endpoint.GetContentstackEndpoint("eu", "contentDelivery", omitHttps: true);
            string proxy = Utils.GetContentstackEndpoint("eu", "contentDelivery", omitHttps: true);
            Assert.Equal(direct, proxy);
        }

        // ------------------------------------------------------------------
        // Local file self-heal.
        // ------------------------------------------------------------------

        [Fact]
        public void LocalFile_WhenWritten_IsReadOnNextCacheReset()
        {
            // First call: either reads from disk or downloads from CDN and writes to disk.
            Endpoint.GetContentstackEndpoint("na", "contentDelivery");

            string localPath = Endpoint.GetLocalFilePath();

            // If CDN download failed (no network in CI), skip this assertion — the
            // self-heal path is covered by the CDN download succeeding at call time.
            if (!File.Exists(localPath))
                return;

            // Reset cache — next call must read from local file (step 2 in LoadRegions),
            // not re-download. Mirrors Python: os.path.exists(_REGIONS_FILE) → open().
            Endpoint.ResetCache();
            string url = Endpoint.GetContentstackEndpoint("na", "contentDelivery");

            Assert.Equal("https://cdn.contentstack.io", url);
        }

        [Fact]
        public void LocalFilePath_IsNextToDll_NotSourceDirectory()
        {
            string localPath = Endpoint.GetLocalFilePath();
            Assert.Contains("Assets", localPath);
            Assert.EndsWith("regions.json", localPath);
        }
    }
}
