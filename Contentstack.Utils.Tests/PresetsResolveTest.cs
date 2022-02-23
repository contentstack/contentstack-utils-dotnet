using Xunit;
using Contentstack.Utils.Tests.Constants;
using Contentstack.Utils.Tests.Helpers;
using Contentstack.Utils.Tests.Mocks;

namespace Contentstack.Utils.Tests
{
    public class PresetsResolveTest
    {

        [Fact]
        public void testRenderBlank()
        {
            AssetMetaModel asset = AssetParser.parse(JsonToHtmlConstants.kAsset);
            string resultUrl = Utils.ResolvePreset(asset.Url, asset.assetMetadata, "extension_uid", "Global Preset");
            Assert.Equal(asset.Url, resultUrl);
        }

        [Fact]
        public void testRenderBlankExtension()
        {
            AssetMetaModel asset = AssetParser.parse(JsonToHtmlConstants.kAssetBlankExtension);
            string resultUrl = Utils.ResolvePreset(asset.Url, asset.assetMetadata, "extension_uid", "Global Preset");
            Assert.Equal(asset.Url, resultUrl);
        }

        [Fact]
        public void testRenderBlankPreset()
        {
            AssetMetaModel asset = AssetParser.parse(JsonToHtmlConstants.kAssetPresets);
            string resultUrl = Utils.ResolvePreset(asset.Url, asset.assetMetadata, "extension_uid", "Local Preset");
            Assert.Equal(asset.Url, resultUrl);
        }
        
        [Fact]
        public void testRenderGlobalPreset()
        {
            AssetMetaModel asset = AssetParser.parse(JsonToHtmlConstants.kAssetMeta);
            string resultUrl = Utils.ResolvePreset(asset.Url, asset.assetMetadata, "extension_uid", "Global Preset");
            Assert.Equal(Constants.Constants.kAssetMetaGlobalPreset, resultUrl);
        }

        [Fact]
        public void testRenderWithCropPreset()
        {
            AssetMetaModel asset = AssetParser.parse(JsonToHtmlConstants.kAssetMeta);
            string resultUrl = Utils.ResolvePreset(asset.Url, asset.assetMetadata, "extension_uid", "WithCrop");
            Assert.Equal(Constants.Constants.kAssetMetaWithCropPreset, resultUrl);
        }

        [Fact]
        public void testRenderLocalPreset()
        {
            AssetMetaModel asset = AssetParser.parse(JsonToHtmlConstants.kAssetMeta);
            string resultUrl = Utils.ResolvePreset(asset.Url, asset.assetMetadata, "extension_uid", "Local Preset");
            Assert.Equal(Constants.Constants.kAssetMetaLocalPreset, resultUrl);
        }

        [Fact]
        public void testRenderFilterPreset()
        {
            AssetMetaModel asset = AssetParser.parse(JsonToHtmlConstants.kAssetMeta);
            string resultUrl = Utils.ResolvePreset(asset.Url, asset.assetMetadata, "extension_uid", "Filter");
            Assert.Equal(Constants.Constants.kAssetMetaFilterPreset, resultUrl);
        }

        [Fact]
        public void testRenderPresetUID_04()
        {
            AssetMetaModel asset = AssetParser.parse(JsonToHtmlConstants.kAssetMeta);
            string resultUrl = Utils.ResolvePresetbyUid(asset.Url, asset.assetMetadata, "extension_uid", "preset_04");
            Assert.Equal(Constants.Constants.kAssetMetaGlobalPreset, resultUrl);
        }

        [Fact]
        public void testRenderPresetUID_01()
        {
            AssetMetaModel asset = AssetParser.parse(JsonToHtmlConstants.kAssetMeta);
            string resultUrl = Utils.ResolvePresetbyUid(asset.Url, asset.assetMetadata, "extension_uid", "preset_01");
            Assert.Equal(Constants.Constants.kAssetMetaLocalPreset, resultUrl);
        }

        [Fact]
        public void testRenderPresetUID_03()
        {
            AssetMetaModel asset = AssetParser.parse(JsonToHtmlConstants.kAssetMeta);
            string resultUrl = Utils.ResolvePresetbyUid(asset.Url, asset.assetMetadata, "extension_uid", "preset_03");
            Assert.Equal(Constants.Constants.kAssetMetaFilterPreset, resultUrl);
        }

        [Fact]
        public void testRenderBlankExtenstionPresetUID_01()
        {
            AssetMetaModel asset = AssetParser.parse(JsonToHtmlConstants.kAssetBlankExtension);
            string resultUrl = Utils.ResolvePresetbyUid(asset.Url, asset.assetMetadata, "extension_uid", "preset_01");
            Assert.Equal(asset.Url, resultUrl);
        }
        [Fact]
        public void testRenderGlobalPresetWithQueryParamUrl()
        {
            AssetMetaModel asset = AssetParser.parse(JsonToHtmlConstants.kAssetMetaURLQuery);
            string resultUrl = Utils.ResolvePreset(asset.Url, asset.assetMetadata, "extension_uid", "Global Preset");
            Assert.Equal(Constants.Constants.kAssetMetaQueryParam, resultUrl);
        }
        
    }
}
//preset_01 "Local Preset"
//preset_02 WithCrop
//preset_03 Filter
//preset_04 "Global Preset"
