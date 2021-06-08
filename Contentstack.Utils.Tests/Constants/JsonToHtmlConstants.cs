namespace Contentstack.Utils.Tests.Constants
{
    public static class JsonToHtmlResultConstants
    {
        public const string kPlainTextHtml = "<strong>Aliquam sit amet libero dapibus, eleifend ligula at, varius justo</strong><strong><em>Lorem ipsum</em></strong><strong><em><u>dolor sit amet</u></em></strong><strong><em><u><strike>consectetur adipiscing elit.</strike></u></em></strong><strong><em><u><span>Sed condimentum iaculis magna in vehicula. </span></u></em></strong><strong><em><u><sup>  Vestibulum vitae convallis </sup></u></em></strong><strong><em><u><sub> lacus. </sub></u></em></strong>";
        public const string kParagraphHtml = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed condimentum iaculis magna in vehicula. Vestibulum vitae convallis lacus. Praesent a diam iaculis turpis rhoncus faucibus. Aliquam sed pulvinar sem.</p>";
        public const string kH1Html = "<h1><strong><em><u><sub>Lorem ipsum dolor sit amet.</sub></u></em></strong></h1>";
        public const string kH2Html = "<h2><strong><em><u><sub>Vestibulum a ligula eget massa sagittis aliquam sit amet quis tortor. </sub></u></em></strong></h2>";
        public const string kH3Html = "<h3><strong><em><u><sub>Mauris venenatis dui id massa sollicitudin, non bibendum nunc dictum.</sub></u></em></strong></h3>";
        public const string kH4Html = "<h4><strong><em><u><sub>MaNullam feugiat turpis quis elit interdum, vitae laoreet quam viverra</sub></u></em></strong></h4>";
        public const string kH5Html = "<h5>Mauris venenatis dui id massa sollicitudin, non bibendum nunc dictum.</h5>";
        public const string kH6Html = "<h6>Nunc porta diam vitae purus semper, ut consequat lorem vehicula.</h6>";
        public const string kOrderListHtml = "<ol><li>Morbi in quam molestie, fermentum diam vitae, bibendum ipsum.</li><li>Pellentesque mattis lacus in quam aliquam congue</li><li>Integer feugiat leo dignissim, lobortis enim vitae, mollis lectus.</li><li>Sed in ante lacinia, molestie metus eu, fringilla sapien.</li></ol>";
        public const string kIUnorderListHtml = "<ul><li>Sed quis metus sed mi hendrerit mollis vel et odio.</li><li>Integer vitae sem dignissim, elementum libero vel, fringilla massa.</li><li>Integer imperdiet arcu sit amet tortor faucibus aliquet.</li><li>Aenean scelerisque velit vitae dui vehicula, at congue massa sagittis.</li></ul>";
        public const string kImgHtml = "<img src=\"https://images.contentstack.com/v3/assets/blt7726e6b/bltb42cd1/5fa3be959bedb6b/Donald.jog.png\" />";
        public const string kTableHtml = "<table><thead><tr><th><p>Header 1</p></th><th><p>Header 2</p></th></tr></thead><tbody><tr><td><p>Body row 1 data 1</p></td><td><p>Body row 1 data 2</p></td></tr><tr><td><p>Body row 2 data 1</p></td><td><p>Body row 2 data 2</p></td></tr></tbody></table>";
        public const string kBlockquoteHtml = "<blockquote>Praesent eu ex sed nibh venenatis pretium.</blockquote>";
        public const string kCodeHtml = "<code>Code template.</code>";
        public const string kLinkInPHtml = "<p><strong><em><u><sub></sub></u></em></strong><a href=\"LINK.com\">LINK</a></p>";
        public const string kEmbedHtml = "<iframe src=\"https://www.youtube.com/watch?v=AOP0yARiW8U\"></iframe>";
    }

    public static class JsonToHtmlConstants
    {
        public const string kBlankDocument = "{"
                                              + "\"uid\": \"06e34a7a4e5d7fc2acd\","
                                              + "\"_version\": 13,"
                                              + "\"attrs\": {},"
                                              + "\"children\": [],"
                                              + "\"type\": \"doc\""
                                              + "}";


        public const string kPlainTextJson = "{"
                                                + "\"uid\": \"06e34a7a4e5d7fc2acd\","
                                                + "\"_version\": 13,"
                                                + "\"attrs\": {},"
                                                + "\"children\":"
                                                + "["
                                                + "{"
                                                + "\"text\": \"Aliquam sit amet libero dapibus, eleifend ligula at, varius justo\","
                                                + "\"bold\": true"
                                                    + "},"
                                                + "{"
                                                + "\"text\": \"Lorem ipsum\","
                                                    + "\"bold\": true,"
                                                    + "\"italic\": true"
                                                + "},"
                                                + "{"
                                                + "\"text\": \"dolor sit amet\","
                                                    + "\"bold\": true,"
                                                    + "\"italic\": true,"
                                                    + "\"underline\": true"
                                                + "},"
                                                + "{"
                                                + "\"text\": \"consectetur adipiscing elit.\","
                                                    + "\"bold\": true,"
                                                    + "\"italic\": true,"
                                                    + "\"underline\": true,"
                                                    + "\"strikethrough\": true"
                                                + "},"
                                                + "{"
                                                + "\"text\": \"Sed condimentum iaculis magna in vehicula. \","
                                                    + "\"bold\": true,"
                                                    + "\"italic\": true,"
                                                    + "\"underline\": true,"
                                                    + "\"inlineCode\": true"
                                                + "},"
                                                + "{"
                                                + "\"text\": \"  Vestibulum vitae convallis \","
                                                    + "\"bold\": true,"
                                                    + "\"italic\": true,"
                                                    + "\"underline\": true,"
                                                    + "\"superscript\": true"
                                                + "},"
                                                + "{"
                                                + "\"text\": \" lacus. \","
                                                    + "\"bold\": true,"
                                                    + "\"italic\": true,"
                                                    + "\"underline\": true,"
                                                    + "\"subscript\": true"
                                                + "}"
                                                + "],"
                                                + "\"type\": \"doc\""
                                            + "}";
    }
}
