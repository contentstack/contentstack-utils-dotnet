using System;
using System.Collections.Generic;
using Contentstack.Utils.Tests.Constants;
using Contentstack.Utils.Tests.Helpers;
using Contentstack.Utils.Tests.Mocks;
using Xunit;
using static Contentstack.Utils.Utils;

namespace Contentstack.Utils.Tests
{
    public class GQLTest
    {
        DefaultRenderMock defaultRender = new DefaultRenderMock(null);

        [Fact]
        public void Should_Return_Empty_String_For_EmptyNode()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kBlankDocument);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { "" }, multipleRTE);
            Assert.Equal("", singleRTE);
        }
        [Fact]
        public void Should_Return_Result_PlainText_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kPlainTextJson);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kPlainTextHtml }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kPlainTextHtml, singleRTE);
        }

        [Fact]
        public void Should_Return_Blank_Result_For_AssetReference_Without_Embedded_Items()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kAssetReferenceJson);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { "" }, multipleRTE);
            Assert.Equal("", singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_AssetReference_With_Embedded_Items()
        {
            var model = GQLParser.parse<AssetModel>(JsonToHtmlConstants.kAssetReferenceJson, JsonToHtmlConstants.KAssetNode);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { "<img src=\"/v3/assets/blt333/blt44asset/dummy.pdf\" alt=\"dummy.pdf\" />" }, multipleRTE);
            Assert.Equal("<img src=\"/v3/assets/blt333/blt44asset/dummy.pdf\" alt=\"dummy.pdf\" />", singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Block()
        {
            var model = GQLParser.parse<AssetModel>(JsonToHtmlConstants.kEntryReferenceBlockJson, JsonToHtmlConstants.KEntryBlocNode);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { "<div><p>blttitleuid</p><p>Content type: <span>content_block</span></p></div>" }, multipleRTE);
            Assert.Equal("<div><p>blttitleuid</p><p>Content type: <span>content_block</span></p></div>", singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Link()
        {
            var model = GQLParser.parse<AssetModel>(JsonToHtmlConstants.kEntryReferenceLinkJson, JsonToHtmlConstants.KEntryLinkNode);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { "<a href=\"bltemmbedEntryuid\">/copy-of-entry-final-02</a>" }, multipleRTE);
            Assert.Equal("<a href=\"bltemmbedEntryuid\">/copy-of-entry-final-02</a>", singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Reference_Entry_As_Inline()
        {
            var model = GQLParser.parse<AssetModel>(JsonToHtmlConstants.kEntryReferenceInlineJson, JsonToHtmlConstants.KEntryInlineNode);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { "<span>blttitleUpdateuid</span>" }, multipleRTE);
            Assert.Equal("<span>blttitleUpdateuid</span>", singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Paragraph_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kParagraphJson);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kParagraphHtml }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kParagraphHtml, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Link_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kLinkInPJson);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kLinkInPHtml }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kLinkInPHtml, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Image_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kImgJson);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kImgHtml }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kImgHtml, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Embed_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kEmbedJson);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kEmbedHtml }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kEmbedHtml, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Header_1_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kH1Json);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kH1Html }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kH1Html, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Header_2_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kH2Json);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kH2Html }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kH2Html, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Header_3_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kH3Json);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kH3Html }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kH3Html, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Header_4_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kH4Json);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kH4Html }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kH4Html, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Header_5_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kH5Json);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kH5Html }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kH5Html, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Header_6_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kH6Json);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kH6Html }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kH6Html, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_OrderList_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kOrderListJson);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kOrderListHtml }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kOrderListHtml, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_UnorderList_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kUnorderListJson);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kIUnorderListHtml }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kIUnorderListHtml, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Table_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kTableJson);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kTableHtml }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kTableHtml, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_BlockQuote_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kBlockquoteJson);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kBlockquoteHtml }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kBlockquoteHtml, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_Code_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kCodeJson);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kCodeHtml }, multipleRTE);
            Assert.Equal(JsonToHtmlResultConstants.kCodeHtml, singleRTE);
        }

        [Fact]
        public void Should_Return_Result_For_HR_Document()
        {
            var model = GQLParser.parse<EntryModel>(JsonToHtmlConstants.kHRJson);

            var multipleRTE = GQL.JsonToHtml(model.multiplerte, defaultRender);
            var singleRTE = GQL.JsonToHtml(model.singlerte, defaultRender);

            Assert.Equal(new List<string>() { "<hr>" }, multipleRTE);
            Assert.Equal("<hr>", singleRTE);
        }
    }
}
