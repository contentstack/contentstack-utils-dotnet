using Xunit;
using Contentstack.Utils.Tests.Mocks;
using System.Collections.Generic;
using Contentstack.Utils.Interfaces;

namespace Contentstack.Utils.Tests
{
    public class UtilsCustomRenderTest
    {
        CustomRenderOptionMock customRender = new CustomRenderOptionMock(new EmbeddedModel(""));
        [Fact]
        public void testRenderBlankString()
        {
            string result = Utils.RenderContent("", customRender);
            Assert.Equal("", result);
        }

        [Fact]
        public void testRenderString()
        {
            string renderString = "<h1>TEST </h2>";
            string result = Utils.RenderContent(renderString, customRender);
            Assert.Equal("<h1>TEST </h1>", result);
        }

        [Fact]
        public void testNonHtmlString()
        {
            string result = Utils.RenderContent(Constants.Constants.kNoHTML, customRender);
            Assert.Equal(Constants.Constants.kNoHTML, result);
        }

        [Fact]
        public void testHtmlString()
        {
            string result = Utils.RenderContent(Constants.Constants.kSimpleHTML, customRender);
            Assert.Equal(Constants.Constants.kSimpleHTML, result);
        }

        [Fact]
        public void testUnexpectedClose()
        {
            string result = Utils.RenderContent(Constants.Constants.kUnexpectedClose, customRender);
            Assert.Equal("<span  class=\"embedded-entry\"  type=\"entry\"  data-sys-entry-uid=\"uid\"  data-sys-content-type-uid=\"data-sys-content-type-uid\"  style=\"display:inline;\"  sys-style-type=\"inline\"><b>uid</b></span>", result);
        }

        [Fact]
        public void testNoChildmodel()
        {
            string result = Utils.RenderContent(Constants.Constants.kNoChildNode, customRender);
            Assert.Equal("<span  class=\"embedded-entry\"  type=\"entry\"  data-sys-entry-uid=\"uid\"  data-sys-content-type-uid=\"data-sys-content-type-uid\"  style=\"display:inline;\"  sys-style-type=\"inline\"><b>uid</b></span>", result);
        }

        [Fact]
        public void testAssetDisplay()
        {
            string result = Utils.RenderContent(Constants.Constants.kAssetDisplay, new CustomRenderOptionMock(new EmbeddedModel("", embedAssetUID: "UID_01")));
            Assert.Equal("<b>title</b><p>filename image: <img  class=\"embedded-asset\"  type=\"asset\"  data-sys-asset-uid=\"UID_01\"  style=\"display:inline;\"  data-sys-content-type-uid=\"sys_assets\"  sys-style-type=\"display\" /></p>", result);
        }

        [Fact]
        public void testEntryBlock()
        {
            string result = Utils.RenderContent(Constants.Constants.kEntryBlock, new CustomRenderOptionMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article")));
            Assert.Equal("<div  class=\"embedded-entry block-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  sys-style-type=\"block\"> <b>UID_01</b></div>", result);
        }

        [Fact]
        public void testEntryInline()
        {
            string result = Utils.RenderContent(Constants.Constants.kEntryInline, new CustomRenderOptionMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article")));
            Assert.Equal("<span  class=\"embedded-entry inline-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  style=\"display:inline;\"  sys-style-type=\"inline\"><b>UID_01</b></span>", result);
        }

        [Fact]
        public void testEntryLink()
        {
            string result = Utils.RenderContent(Constants.Constants.kEntryLink, new CustomRenderOptionMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article")));
            Assert.Equal("<span> Please find link to: <a  class=\"embedded-entry link-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  style=\"display:inline;\"  sys-style-type=\"link\"><b>{{title}}</b></a></span>", result);
        }

        [Fact]
        public void testAssetAll()
        {
            string result = Utils.RenderContent(Constants.Constants.kAssetEmbed, new CustomRenderOptionMock(new EmbeddedModel("", "UID_01")));
            Assert.Equal("<p></p><p></p>", result);

            var embModel = new EmbeddedModel("");
            embModel.embeddedItems = new Dictionary<string, List<IEmbeddedObject>>()
            {
                ["rte"] = new List<IEmbeddedObject> {
                    new EmbeddedAssetModel { Uid = "UID_02" },
                    new EmbeddedAssetModel { Uid = "UID_03" }
                }
            };

            result = Utils.RenderContent(Constants.Constants.kAssetEmbed, new CustomRenderOptionMock(embModel));
            Assert.Equal("<b>title</b><p>filename image: <img  class=\"embedded-asset\"  data-sys-asset-filelink=\"https://image.contentstack.com/v3/5f74813386c10/clitud.jpeg\"  data-sys-asset-uid=\"UID_02\"  data-sys-asset-filename=\"Cuvier-67_Autruche_d_Afrique.jpg\"  data-sys-asset-contenttype=\"image/jpeg\"  data-sys-asset-alt=\"Cuvier-67_Autruche_d_Afrique.jpg\"  data-sys-asset-caption=\"somecaption\"  data-sys-asset-link=\"http://abc.com\"  data-sys-asset-position=\"center\"  data-sys-asset-isnewtab=\"true\"  data-sys-content-type-uid=\"sys_assets\"  type=\"asset\"  sys-style-type=\"display\" /></p><p></p><p></p><b>title</b><p>filename image: <img  class=\"embedded-asset\"  data-redactor-type=\"embed\"  data-widget-code=\"\"  data-sys-asset-filelink=\"https://images.contentstack.com/v3/assets/iphone-mockup.png\"  data-sys-asset-uid=\"UID_03\"  data-sys-asset-filename=\"iphone-mockup.png\"  data-sys-asset-contenttype=\"image/png\"  data-sys-content-type-uid=\"sys_assets\"  type=\"asset\"  sys-style-type=\"display\" /></p>", result);
        }

        [Fact]
        public void testEntryBlockLink()
        {
            string result = Utils.RenderContent($"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}", customRender);
            Assert.Equal("", result);

            result = Utils.RenderContent($"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}", new CustomRenderOptionMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article")));
            Assert.Equal("<div  class=\"embedded-entry block-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  sys-style-type=\"block\"> <b>UID_01</b></div><span> Please find link to: <a  class=\"embedded-entry link-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  style=\"display:inline;\"  sys-style-type=\"link\"><b>{{title}}</b></a></span>", result);
        }

        [Fact]
        public void testEntryBlockLinkInline()
        {
            string result = Utils.RenderContent($"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", customRender);
            Assert.Equal(" ", result);

            result = Utils.RenderContent($"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}  {Constants.Constants.kEntryInline}", new CustomRenderOptionMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article")));
            Assert.Equal("<div  class=\"embedded-entry block-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  sys-style-type=\"block\"> <b>UID_01</b></div><span> Please find link to: <a  class=\"embedded-entry link-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  style=\"display:inline;\"  sys-style-type=\"link\"><b>{{title}}</b></a></span>  <span  class=\"embedded-entry inline-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  style=\"display:inline;\"  sys-style-type=\"inline\"><b>UID_01</b></span>", result);
        }

        [Fact]
        public void testAllEmbedStyles()
        {
            string result = Utils.RenderContent($"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", customRender);
            Assert.Equal(" ", result);

            result = Utils.RenderContent($"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", new CustomRenderOptionMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article")));
            Assert.Equal("<div  class=\"embedded-entry block-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  sys-style-type=\"block\"> <b>UID_01</b></div><span> Please find link to: <a  class=\"embedded-entry link-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  style=\"display:inline;\"  sys-style-type=\"link\"><b>{{title}}</b></a></span> <span  class=\"embedded-entry inline-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  style=\"display:inline;\"  sys-style-type=\"inline\"><b>UID_01</b></span>", result);
        }

        [Fact]
        public void testAllEmbedStylesWithAllEmbedObject()
        {
            string result = Utils.RenderContent($"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", customRender);
            Assert.Equal(" ", result);

            result = Utils.RenderContent($"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", new CustomRenderOptionMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article", embedAssetUID: "UID_01")));
            Assert.Equal("<b>title</b><p>filename image: <img  class=\"embedded-asset\"  type=\"asset\"  data-sys-asset-uid=\"UID_01\"  style=\"display:inline;\"  data-sys-content-type-uid=\"sys_assets\"  sys-style-type=\"display\" /></p><div  class=\"embedded-entry block-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  sys-style-type=\"block\"> <b>UID_01</b></div><span> Please find link to: <a  class=\"embedded-entry link-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  style=\"display:inline;\"  sys-style-type=\"link\"><b>{{title}}</b></a></span> <span  class=\"embedded-entry inline-entry\"  type=\"entry\"  data-sys-entry-uid=\"UID_01\"  data-sys-content-type-uid=\"article\"  style=\"display:inline;\"  sys-style-type=\"inline\"><b>UID_01</b></span>", result);
        }
    }
}
