using Xunit;
using Contentstack.Utils.Tests.Mocks;
using System.Collections.Generic;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;
using Contentstack.Utils.Tests.Helpers;
using Contentstack.Utils.Tests.Constants;

namespace Contentstack.Utils.Tests
{
    public class JsonToHtmlTest
    {
        DefaultRenderMock defaultRender = new DefaultRenderMock(null);

        [Fact]
        public void Should_Return_Empty_String_For_EmptyNode()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kBlankDocument);

            var result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal("", result);
        }

        [Fact]
        public void Should_Return_Empty_List_String_For_EmptyNode_Array()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kBlankDocument);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { "" }, result);
        }

        [Fact]
        public void Should_Return_Result_PlainText_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kPlainTextJson);

            var result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kPlainTextHtml, result);
        }

        [Fact]
        public void Should_Return_Result_Array_PlainText_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kPlainTextJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kPlainTextHtml }, result);
        }

        [Fact]
        public void Should_Return_Blank_Result_For_AssetReference_Without_Embedded_Items ()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kAssetReferenceJson);

            var result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal("", result);
        }

        [Fact]
        public void Should_Return_Result_For_AssetReference_With_Embedded_Items()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kAssetReferenceJson);

            var result = Utils.JsonToHtml(node, new DefaultRenderMock(new EmbeddedModel("", embedAssetUID: "blt44asset")));

            Assert.Equal("<img src=\"url\" alt=\"title\" />", result);
        }

        [Fact]
        public void Should_Return_Result_For_AssetReference_Array()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kAssetReferenceJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, new DefaultRenderMock(new EmbeddedModel("", embedAssetUID: "blt44asset")));

            Assert.Equal(new List<string>() { "<img src=\"url\" alt=\"title\" />" }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Block()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceBlockJson);

            var result = Utils.JsonToHtml(node, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blttitleuid", contentTypeUid: "content_block")));

            Assert.Equal("<div><p>blttitleuid</p><p>Content type: <span>content_block</span></p></div>", result);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Block_Array()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceBlockJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blttitleuid", contentTypeUid: "content_block")));

            Assert.Equal(new List<string>() { "<div><p>blttitleuid</p><p>Content type: <span>content_block</span></p></div>" }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Link()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceLinkJson);

            var result = Utils.JsonToHtml(node, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "bltemmbedEntryuid", contentTypeUid: "embeddedrte")));

            Assert.Equal("<a href=\"bltemmbedEntryuid\">/copy-of-entry-final-02</a>", result);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Link_Array()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceLinkJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "bltemmbedEntryuid", contentTypeUid: "embeddedrte")));

            Assert.Equal(new List<string>() { "<a href=\"bltemmbedEntryuid\">/copy-of-entry-final-02</a>" }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Inline()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceInlineJson);

            var result = Utils.JsonToHtml(node, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blttitleUpdateuid", contentTypeUid: "embeddedrte")));

            Assert.Equal("<span>blttitleUpdateuid</span>", result);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Inline_Array()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceInlineJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blttitleUpdateuid", contentTypeUid: "embeddedrte")));

            Assert.Equal(new List<string>() { "<span>blttitleUpdateuid</span>" }, result);
        }
    }
}
