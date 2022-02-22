using System;
namespace Contentstack.Utils.Tests.Constants
{
    public static class Constants
    {
        public const string kBlankString = "";

        public const string kNoHTML = "non html string";
        public const string kSimpleHTML = "<h1>Hello</h1> World";

        public const string kUnexpectedClose = "<figur2 class=\"embedded-entry\" type=\"entry\" data-sys-entry-uid=\"uid\" data-sys-content-type-uid=\"data-sys-content-type-uid\" style=\"display:inline;\" sys-style-type=\"inline\">"
                                                +"</figure>";
        public const string kNoChildNode = "<figure class=\"embedded-entry\" type=\"entry\" data-sys-entry-uid=\"uid\" data-sys-content-type-uid=\"data-sys-content-type-uid\" style=\"display:inline;\" sys-style-type=\"inline\">"
                                            +"</figure>";

        public const string kAssetDisplay = "<figure class=\"embedded-asset\" type=\"asset\" data-sys-asset-uid=\"UID_01\" style=\"display:inline;\" data-sys-content-type-uid=\"sys_assets\" sys-style-type=\"display\">"
                                            + "<img src = \"{{url}}\" data-sys-asset-uid=\"{{uid}}\" alt=\"{{object.title}}\"></figure>";

public const string kEntryBlock = "<figure class=\"embedded-entry block-entry\" type=\"entry\" data-sys-entry-uid=\"UID_01\" data-sys-content-type-uid=\"article\" sys-style-type=\"block\">"
+"<span>{{title}}</span>"
+"</figure>";

public const string kEntryInline = "<figure class=\"embedded-entry inline-entry\" type=\"entry\" data-sys-entry-uid=\"UID_01\" data-sys-content-type-uid=\"article\" style=\"display:inline;\" sys-style-type=\"inline\">"
+"<data data-sys-field-uid=\"title\">{{title}}</data>"
+"</figure>";

        public const string kEntryLink = "<figure class=\"embedded-entry link-entry\" type=\"entry\" data-sys-entry-uid=\"UID_01\" data-sys-content-type-uid=\"article\" style=\"display:inline;\" sys-style-type=\"link\">"
+"<a data-sys-field-uid=\"title\" href=\"{{url}}\">{{title}}</a>"
+"</figure>";

        public const string kAssetEmbed = "<figure class=\"embedded-asset\" data-sys-asset-filelink=\"https://image.contentstack.com/v3/5f74813386c10/clitud.jpeg\" data-sys-asset-uid=\"UID_02\" data-sys-asset-filename=\"Cuvier-67_Autruche_d_Afrique.jpg\" data-sys-asset-contenttype=\"image/jpeg\" data-sys-asset-alt=\"Cuvier-67_Autruche_d_Afrique.jpg\" data-sys-asset-caption=\"somecaption\" data-sys-asset-link=\"http://abc.com\" data-sys-asset-position=\"center\" data-sys-asset-isnewtab=\"true\" data-sys-content-type-uid=\"sys_assets\" type=\"asset\" sys-style-type=\"display\"></figure>"
+ "<p></p>"
+"<p></p>"
+ "<figure class=\"embedded-asset\" data-redactor-type=\"embed\" data-widget-code=\"\"\" data-sys-asset-filelink=\"https://images.contentstack.com/v3/assets/iphone-mockup.png\" data-sys-asset-uid=\"UID_03\" data-sys-asset-filename=\"iphone-mockup.png\" data-sys-asset-contenttype=\"image/png\" data-sys-content-type-uid=\"sys_assets\" type=\"asset\" sys-style-type=\"display\"></figure>";

        public const string kEntryEmbed = "<div class=\"redactor-component embedded-entry block-entry redactor-component-active\" data-redactor-type=\"embed\" data-widget-code=\"\"\" data-sys-entry-uid=\"UID_04\" data-sys-entry-locale=\"en-us\" data-sys-content-type-uid=\"00_suraj\" sys-style-type=\"block\" type=\"entry\" data-sys-can-edit=\"true\"></div>"
+"<p>bkcsdcsdc</p>"
+"<div class=\"redactor-component embedded-entry block-entry\" data-redactor-type=\"embed\" data-widget-code=\"\"\" data-sys-entry-uid=\"UID_05\" data-sys-entry-locale=\"en-us\" data-sys-content-type-uid=\"manish\" sys-style-type=\"block\" type=\"entry\" data-sys-can-edit=\"true\"></div>"
+"<p></p>"
+"<div class=\"redactor-component embedded-entry block-entry\" data-redactor-type=\"embed\" data-widget-code=\"\"\" data-sys-entry-uid=\"UID_06\" data-sys-entry-locale=\"en-us\" data-sys-content-type-uid=\"manish\" sys-style-type=\"block\" type=\"entry\" data-sys-can-edit=\"true\"></div>";

        public const string kAllEmbeddEntry = "<p><br>Sample&nbsp;<a data-sys-entry-uid=\"UID_04\" data-sys-entry-locale=\"en-us\" data-sys-content-type-uid=\"00_suraj\" sys-style-type=\"link\" data-sys-can-edit=\"true\" class=\"embedded-entry\" type=\"entry\" href=\"/suraj-123-entry\" title=\"Manish New entry\">LinkText</a> inside RTE&nbsp;<span class=\"redactor-component embedded-entry inline-entry\" data-redactor-type=\"embed\" data-widget-code=\"\"\" data-sys-entry-uid=\"UID_04\" data-sys-entry-locale=\"en-us\" data-sys-content-type-uid=\"00_suraj\" data-sys-can-edit=\"true\" sys-style-type=\"inline\" type=\"entry\"></span></p><div class=\"redactor-component embedded-entry block-entry\" data-redactor-type=\"embed\" data-widget-code=\"\"\" data-sys-entry-uid=\"UID_06\" data-sys-entry-locale=\"en-us\" data-sys-content-type-uid=\"manish\" data-sys-can-edit=\"true\" sys-style-type=\"block\" type=\"entry\"></div>";

        public const string kUnexpectedResult = "<span class=\"embedded-asset\" data-sys-content-type-uid=\"data-sys-content-type-uid\" data-sys-entry-uid=\"uid\" style=\"display:inline;\" sys-style-type=\"inline\" data-sys-content-type-uid=\"sys_assets\" type=\"asset\"><b>title</b> "
+ "</span>";

        public const string kAssetDisplayCustomResult = "<b>title</b><p>filename image: <img class=\"embedded-asset\" data-sys-asset-uid=\"UID_01\" style=\"display:inline;\" sys-style-type=\"display\" data-sys-content-type-uid=\"sys_assets\" type=\"asset\" /></p>";


        public const string kEntryBlockCustomResult = "<div class=\"embedded-entry block-entry\" data-sys-content-type-uid=\"article\" data-sys-entry-uid=\"UID_01\" sys-style-type=\"block\" type=\"entry\"> <b>UID_01</b></div>";

        public const string kEntryInlineCustomeResult = "<span class=\"embedded-entry inline-entry\" data-sys-content-type-uid=\"article\" data-sys-entry-uid=\"UID_01\" style=\"display:inline;\" sys-style-type=\"inline\" type=\"entry\"><b>UID_01</b></span>";

        public const string kEntryLinkCustomResult = "<span> Please find link to: <a class=\"embedded-entry link-entry\" data-sys-content-type-uid=\"article\" data-sys-entry-uid=\"UID_01\" style=\"display:inline;\" sys-style-type=\"link\" type=\"entry\"><b>"
+"{{title}}"
+"</b></a>";

        public const string kAssetdisplayCustomResult = "<b>title</b><p>filename image: <img class=\"embedded-asset\" data-sys-asset-alt=\"Cuvier-67_Autruche_d_Afrique.jpg\" data-sys-asset-caption=\"somecaption\" data-sys-asset-contenttype=\"image/jpeg\" data-sys-asset-filelink=\"https://image.contentstack.com/v3/5f74813386c10/clitud.jpeg\" data-sys-asset-filename=\"Cuvier-67_Autruche_d_Afrique.jpg\" data-sys-asset-isnewtab=\"true\" data-sys-asset-link=\"http://abc.com\" data-sys-asset-position=\"center\" data-sys-asset-uid=\"UID_02\" sys-style-type=\"display\" data-sys-content-type-uid=\"sys_assets\" type=\"asset\" /></p>"
+ "<p></p>"
+"<p></p>"
+"<b>title</b><p>filename image: <img class=\"embedded-asset\" data-redactor-type=\"embed\" data-sys-asset-contenttype=\"image/png\" data-sys-asset-filelink=\"https://images.contentstack.com/v3/assets/iphone-mockup.png\" data-sys-asset-filename=\"iphone-mockup.png\" data-sys-asset-uid=\"UID_03\" data-widget-code=\"\"\" sys-style-type=\"display\" type=\"asset\" /></p>";


        public const string kEntryBlockLinkCustomResult = "<div class=\"embedded-entry block-entry\" data-sys-content-type-uid=\"article\" data-sys-entry-uid=\"UID_01\" sys-style-type=\"block\" type=\"entry\"> <b>UID_01</b></div><span> Please find link to: <a class=\"embedded-entry link-entry\" data-sys-content-type-uid=\"article\" data-sys-entry-uid=\"UID_01\" style=\"display:inline;\" sys-style-type=\"link\" type=\"entry\"><b>"
+"{{title}}"
+"</b></a>";

        public const string kEntryBlockLinkInlineCustomResult = "<div class=\"embedded-entry block-entry\" data-sys-content-type-uid=\"article\" data-sys-entry-uid=\"UID_01\" sys-style-type=\"block\" type=\"entry\"> <b>UID_01</b></div><span> Please find link to: <a class=\"embedded-entry link-entry\" data-sys-content-type-uid=\"article\" data-sys-entry-uid=\"UID_01\" style=\"display:inline;\" sys-style-type=\"link\" type=\"entry\"><b>"
+"{{title}}"
+"</b></a> <span class=\"embedded-entry inline-entry\" data-sys-content-type-uid=\"article\" data-sys-entry-uid=\"UID_01\" style=\"display:inline;\" sys-style-type=\"inline\" type=\"entry\"><b>UID_01</b></span>";

        public const string kAllEmbedCustomeResult = "<div class=\"embedded-entry block-entry\" data-sys-content-type-uid=\"article\" data-sys-entry-uid=\"UID_01\" sys-style-type=\"block\" type=\"entry\"> <b>UID_01</b></div>"
+"<span> Please find link to: <a class=\"embedded-entry link-entry\" data-sys-content-type-uid=\"article\" data-sys-entry-uid=\"UID_01\" style=\"display:inline;\" sys-style-type=\"link\" type=\"entry\"><b>"
+"{{title}}"
+"</b></a>"
+"<span class=\"embedded-entry inline-entry\" data-sys-content-type-uid=\"article\" data-sys-entry-uid=\"UID_01\" style=\"display:inline;\" sys-style-type=\"inline\" type=\"entry\"><b>UID_01</b></span>";

        public const string kContentblockRTEResult = "<div class=\"embedded-entry block-entry\" data-sys-content-type-uid=\"content_block\" data-sys-entry-locale=\"en-us\" data-sys-entry-uid=\"UID_07\" sys-style-type=\"block\" type=\"entry\"> <b>Update this title</b>"
+"<figure class=\"embedded-entry inline-entry\" data-sys-entry-uid=\"UID_07\" data-sys-entry-locale=\"en-us\" data-sys-content-type-uid=\"content_block\" sys-style-type=\"inline\" type=\"entry\"></figure>"
+"</div>"
+"<span class=\"embedded-entry inline-entry\" data-sys-content-type-uid=\"embeddedrte\" data-sys-entry-locale=\"en-us\" data-sys-entry-uid=\"UID_08\" sys-style-type=\"inline\" type=\"entry\"><b>Entry with embedded entry</b> </span>"
+"<p></p>";

        public const string kContentblockRichTextResult = "<div class=\"embedded-entry block-entry\" data-sys-content-type-uid=\"embeddedrte\" data-sys-entry-locale=\"en-us\" data-sys-entry-uid=\"UID_09\" sys-style-type=\"block\" type=\"entry\"> <b>updated title</b>"
+ "<figure class=\"embedded-asset\" data-sys-asset-filelink=\"https://contentstack.image/v3/UID_10/DIABETICDIET-800x600.jpg\" data-sys-asset-uid=\"UID_10\" data-sys-asset-filename=\"DIABETICDIET-800x600.jpg\" data-sys-asset-contenttype=\"image/jpeg\" data-sys-content-type-uid=\"sys_assets\" type=\"asset\" sys-style-type=\"display\"></figure>"
+ "</div>"
+"<p></p>"
+ "<b>svg-logo-text.png</b><p>svg-logo-text.png image: <img class=\"embedded-asset\" data-sys-asset-contenttype=\"image/png\" data-sys-asset-filelink=\"https://contentstack.image/v3/UID_11/html5.png\" data-sys-asset-filename=\"svg-logo-text.png\" data-sys-asset-uid=\"UID_11\" sys-style-type=\"display\" data-sys-content-type-uid=\"sys_assets\" type=\"asset\" /></p>";

        public const string kAssetMetaGlobalPreset = "http://image.contenstack.com/crop_area.jpeg?height=712&width=864&orient=4&format=jpeg&quality=100";
        public const string kAssetBlankExtensionResult = "http://image.contenstack.com/crop_area.jpeg?height=712&width=864&orient=4&format=jpeg&quality=100";
        public const string kAssetMetadataBlank = "http://image.contenstack.com/crop_area.jpeg?height=712&width=864&orient=4&format=jpeg&quality=100";
        public const string kAssetPresets = "http://image.contenstack.com/crop_area.jpeg?height=712&width=864&orient=4&format=jpeg&quality=100";
        public const string kAssetMetaFilterPreset = "http://image.contenstack.com/crop_area.jpeg?height=712&width=864&format=jpeg&quality=100&brightness=52&contrast=15&saturation=-30&blur=16&sharpen=a9,r669,t207";
        public const string kAssetMetaLocalPreset = "http://image.contenstack.com/crop_area.jpeg?height=500&width=500&orient=2&format=jpeg&quality=100";
        public const string kAssetMetaWithCropPreset = "http://image.contenstack.com/crop_area.jpeg?height=569.6&width=569.6&orient=3&format=jpeg&quality=100";
        public const string kAssetMetaQueryParam = "http://image.contenstack.com/crop_area.jpeg?render=full&noval=&height=712&width=864&orient=4&format=jpeg&quality=100";
    }
}
