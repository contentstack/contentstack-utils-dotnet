using Xunit;
using Contentstack.Utils.Tests.Mocks;
using System.Collections.Generic;
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

            var result = Utils.JsonToHtml(node, new DefaultRenderMock(new EmbeddedModel("", embedAssetUID: "UID_12")));

            Assert.Equal("<img src=\"url\" alt=\"title\" />", result);
        }

        [Fact]
        public void Should_Return_Result_For_AssetReference_Array()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kAssetReferenceJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, new DefaultRenderMock(new EmbeddedModel("", embedAssetUID: "UID_12")));

            Assert.Equal(new List<string>() { "<img src=\"url\" alt=\"title\" />" }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Block()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceBlockJson);

            var result = Utils.JsonToHtml(node, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "UID_07", contentTypeUid: "content_block")));

            Assert.Equal("<div><p>UID_07</p><p>Content type: <span>content_block</span></p></div>", result);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Block_Array()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceBlockJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "UID_07", contentTypeUid: "content_block")));

            Assert.Equal(new List<string>() { "<div><p>UID_07</p><p>Content type: <span>content_block</span></p></div>" }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Link()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceLinkJson);

            var result = Utils.JsonToHtml(node, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "UID_08", contentTypeUid: "embeddedrte")));

            Assert.Equal("<a href=\"UID_08\">/copy-of-entry-final-02</a>", result);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Link_Array()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceLinkJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "UID_08", contentTypeUid: "embeddedrte")));

            Assert.Equal(new List<string>() { "<a href=\"UID_08\">/copy-of-entry-final-02</a>" }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Inline()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceInlineJson);

            var result = Utils.JsonToHtml(node, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "UID_09", contentTypeUid: "embeddedrte")));

            Assert.Equal("<span>UID_09</span>", result);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Inline_Array()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceInlineJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "UID_09", contentTypeUid: "embeddedrte")));

            Assert.Equal(new List<string>() { "<span>UID_09</span>" }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Paragraph_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kParagraphJson);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kParagraphHtml, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_Paragraph_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kParagraphJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kParagraphHtml }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Link_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kLinkInPJson);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kLinkInPHtml, result);
        }

        [Fact]
        public void Should_Return_Result_For_Link_MailTo_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kLinkInPMailToJson);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kLinkInPMailToHtml, result);
        }

        [Fact]
        public void Should_Return_Result_For_Link_MailTo_Document_Custom()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kLinkInPMailToJson);

            string result = Utils.JsonToHtml(node, new CustomRenderOptionMock(null));

            Assert.Equal(JsonToHtmlResultConstants.kLinkInPMailToTARGEtHtml, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_Link_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kLinkInPJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kLinkInPHtml }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Image_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kImgJson);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kImgHtml, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_Image_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kImgJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kImgHtml }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Embed_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEmbedJson);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kEmbedHtml, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_Embed_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEmbedJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kEmbedHtml }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Header_1_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kH1Json);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kH1Html, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_Header_1_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kH1Json);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kH1Html }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Header_2_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kH2Json);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kH2Html, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_Header_2_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kH2Json);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kH2Html }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Header_3_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kH3Json);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kH3Html, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_Header_3_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kH3Json);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kH3Html }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Header_4_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kH4Json);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kH4Html, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_Header_4_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kH4Json);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kH4Html }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Header_5_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kH5Json);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kH5Html, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_Header_5_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kH5Json);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kH5Html }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Header_6_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kH6Json);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kH6Html, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_Header_6_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kH6Json);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kH6Html }, result);
        }

        [Fact]
        public void Should_Return_Result_For_OrderList_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kOrderListJson);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kOrderListHtml, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_OrderList_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kOrderListJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kOrderListHtml }, result);
        }

        [Fact]
        public void Should_Return_Result_For_UnorderList_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kUnorderListJson);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kIUnorderListHtml, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_UnorderList_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kUnorderListJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kIUnorderListHtml }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Table_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kTableJson);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kTableHtml, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_Table_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kTableJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kTableHtml }, result);
        }

        [Fact]
        public void Should_Return_Result_For_BlockQuote_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kBlockquoteJson);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kBlockquoteHtml, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_BlockQuote_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kBlockquoteJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kBlockquoteHtml }, result);
        }

        [Fact]
        public void Should_Return_Result_For_Code_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kCodeJson);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kCodeHtml, result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_Code_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kCodeJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kCodeHtml }, result);
        }

        [Fact]
        public void Should_Return_Result_For_HR_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kHRJson);

            string result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal("<hr>", result);
        }

        [Fact]
        public void Should_Return_Result_For_Array_Hr_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kHRJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { "<hr>" }, result);
        }

        [Fact]
        public void TestForClassandId()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.stringClassId);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.stringClassIdResult }, result);
        }
    }
}
