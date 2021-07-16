﻿namespace Contentstack.Utils.Tests.Constants
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
        public const string kBlankDocument = "{ \"uid\":\"06e34a7a4e5d7fc2acd\", \"_version\":13, \"attrs\":{ }, \"children\":[],\"type\":\"doc\"}";
        public const string kPlainTextJson = "{ \"uid\":\"06e34a7a4e5d7fc2acd\", \"_version\":13, \"attrs\":{ }, \"children\":[{\"text\":\"Aliquam sit amet libero dapibus, eleifend ligula at, varius justo\",\"bold\":true},{ \"text\":\"Lorem ipsum\",\"bold\":true,\"italic\":true},{ \"text\":\"dolor sit amet\",\"bold\":true,\"italic\":true,\"underline\":true},{ \"text\":\"consectetur adipiscing elit.\",\"bold\":true,\"italic\":true,\"underline\":true,\"strikethrough\":true},{ \"text\":\"Sed condimentum iaculis magna in vehicula. \",\"bold\":true,\"italic\":true,\"underline\":true,\"inlineCode\":true},{ \"text\":\"  Vestibulum vitae convallis \",\"bold\":true,\"italic\":true,\"underline\":true,\"superscript\":true},{ \"text\":\" lacus. \",\"bold\":true,\"italic\":true,\"underline\":true,\"subscript\":true}],\"type\":\"doc\"}";
        public const string kH1Json = "{ \"uid\":\"06e34a7a449d7fc2acd\",\"_version\":13,\"attrs\":{ },\"children\":[{ \"type\":\"h1\",\"attrs\":{ },\"uid\":\"c2dfed70 4d7030c65e2e1\",\"children\":[{ \"text\":\"Lorem ipsum dolor sit amet.\",\"bold\":true,\"italic\":true,\"underline\":true,\"subscript\":true}]}],\"type\":\"doc\"}";
        public const string kH2Json = "{ \"uid\":\"06e34a7a4e2acd\",\"_version\":13,\"attrs\":{ },\"children\":[{ \"type\":\"h2\",\"attrs\":{ },\"uid\":\"c2dfed9a7030c65e2e1\",\"children\":[{ \"text\":\"Vestibulum a ligula eget massa sagittis aliquam sit amet quis tortor. \",\"bold\":true,\"italic\":true,\"underline\":true,\"subscript\":true}]}],\"type\":\"doc\"}";
        public const string kH3Json = "{ \"uid\":\"06e34ad7fc2acd\",\"_version\":13,\"attrs\":{ },\"children\":[{ \"type\":\"h3\",\"attrs\":{ },\"uid\":\"c2df42cfb70 4d7030c65e2e1\",\"children\":[{ \"text\":\"Mauris venenatis dui id massa sollicitudin, non bibendum nunc dictum.\",\"bold\":true,\"italic\":true,\"underline\":true,\"subscript\":true}]}],\"type\":\"doc\"}";
        public const string kH4Json = "{ \"uid\":\"06e34a7a4e54cd\", \"_version\":13, \"attrs\":{ \"style\":{ \"text-align\":\"center\" }, \"redactor-attributes\":{ } }, \"children\":[{\"type\":\"h4\",\"attrs\":{},\"uid\":\"c2dfed4d7030c65e2e1\",\"children\":[{\"text\":\"MaNullam feugiat turpis quis elit interdum, vitae laoreet quam viverra\",\"bold\":true,\"italic\":true,\"underline\":true,\"subscript\":true}]}],\"type\":\"doc\"}";
        public const string kH5Json = "{ \"uid\": \"06e381190849dacd\", \"_version\": 13, \"attrs\": { }, \"children\": [ { \"type\": \"h5\", \"attrs\": {}, \"uid\": \"c2d672242cfb7045e2e1\", \"children\": [ { \"text\": \"Mauris venenatis dui id massa sollicitudin, non bibendum nunc dictum.\", } ] } ], \"type\": \"doc\" }";
        public const string kH6Json = "{ \"uid\": \"06e34a71190849d7fcd\", \"_version\": 13, \"attrs\": { }, \"children\": [ { \"type\": \"h6\", \"attrs\": {}, \"uid\": \"c2dfa672242cfb7e2e1\", \"children\": [ { \"text\": \"Nunc porta diam vitae purus semper, ut consequat lorem vehicula.\", } ] } ], \"type\": \"doc\" }";
        public const string kOrderListJson = "{ \"uid\":\"06e35 48119084 9d7fc2acd\", \"_version\":13, \"attrs\":{ }, \"children\":[{\"uid\":\"2b5b4acbb3cfce02d3e\",\"type\":\"ol\",\"children\":[{\"type\":\"li\",\"attrs\":{\"style\":{\"text-align\":\"justify\"},\"redactor-attributes\":{ }},\"uid\":\"160bbd7430b98bd3d996\",\"children\":[{\"text\":\"Morbi in quam molestie, fermentum diam vitae, bibendum ipsum.\"}]},{ \"type\":\"li\",\"attrs\":{ \"style\":{ \"text-align\":\"justify\"},\"redactor-attributes\":{ } },\"uid\":\"a15f4d749bc2903d\",\"children\":[{ \"text\":\"Pellentesque mattis lacus in quam aliquam congue\"}]},{ \"type\":\"li\",\"attrs\":{ \"style\":{ \"text-align\":\"justify\"},\"redactor-attributes\":{ } },\"uid\":\"e54224bbcb6f9e8f1e43\",\"children\":[{ \"text\":\"Integer feugiat leo dignissim, lobortis enim vitae, mollis lectus.\"}]},{ \"type\":\"li\",\"attrs\":{ \"style\":{ \"text-align\":\"justify\"},\"redactor-attributes\":{ } },\"uid\":\"c0148bab9af758784\",\"children\":[{ \"text\":\"Sed in ante lacinia, molestie metus eu, fringilla sapien.\"}]}],\"id\":\"7f413d448a\",\"attrs\":{ }}],\"type\":\"doc\"}";
        public const string kUnorderListJson = "{ \"uid\":\"0e5481190849a\", \"_version\":13, \"attrs\":{ }, \"children\":[{\"uid\":\"a3a2b443ebffc867b\",\"type\":\"ul\",\"children\":[{\"uid\":\"f67354d4eed64451922\",\"type\":\"li\",\"children\":[{\"text\":\"Sed quis metus sed mi hendrerit mollis vel et odio.\"}],\"attrs\":{ }},{ \"uid\":\"5 50cba5 bea92f23e36fd1\",\"type\":\"li\",\"children\":[{ \"text\":\"Integer vitae sem dignissim, elementum libero vel, fringilla massa.\"}],\"attrs\":{ } },{ \"uid\":\"0e5c9b4cd983e8fd543\",\"type\":\"li\",\"children\":[{ \"text\":\"Integer imperdiet arcu sit amet tortor faucibus aliquet.\"}],\"attrs\":{ } },{ \"uid\":\"6d9a43a3816bd83a9a\",\"type\":\"li\",\"children\":[{ \"text\":\"Aenean scelerisque velit vitae dui vehicula, at congue massa sagittis.\"}],\"attrs\":{ } }],\"id\":\"b083fa46ef899420ab19\",\"attrs\":{ }}],\"type\":\"doc\"}";
        public const string kImgJson = "{ \"uid\":\"06e34a7a4849d7fc2acd\", \"_version\":13, \"attrs\":{ }, \"children\":[{\"uid\":\"f3be74be3b64646e626\",\"type\":\"img\",\"attrs\":{\"url\":\"https://images.contentstack.com/v3/assets/blt7726e6b/bltb42cd1/5fa3be959bedb6b/Donald.jog.png\",\"width\":33.69418132611637,\"height\":\"auto\",\"redactor-attributes\":{\"asset_uid\":\"47f1aa5ae422cd1\"}},\"children\":[{\"text\":\"\"}]}],\"type\":\"doc\"}";
        public const string kParagraphJson = "{ \"uid\":\"0d7fd\", \"_version\":13, \"attrs\":{ }, \"children\":[{\"type\":\"p\",\"attrs\":{},\"uid\":\"0a1b5676aa510e5a\",\"children\":[{\"text\":\"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed condimentum iaculis magna in vehicula. Vestibulum vitae convallis lacus. Praesent a diam iaculis turpis rhoncus faucibus. Aliquam sed pulvinar sem.\"}]}],\"type\":\"doc\"}";
        public const string kBlockquoteJson = "{ \"uid\":\"06084d7fd\", \"_version\":13, \"attrs\":{ }, \"children\":[{\"uid\":\"503f9cc97534db55\",\"type\":\"blockquote\",\"id\":\"431f78e567161460\",\"children\":[{\"text\":\"Praesent eu ex sed nibh venenatis pretium.\"}],\"attrs\":{ }}],\"type\":\"doc\"}";
        public const string kCodeJson = "{ \"uid\":\"06ea490849d7fc2acd\", \"_version\":13, \"attrs\":{ }, \"children\":[{\"uid\":\"83fba92c91b30002b\",\"type\":\"code\",\"attrs\":{},\"children\":[{\"text\":\"Code template.\"}]}],\"type\":\"doc\"}";
        public const string kTableJson = "{ \"uid\": \"06e481190849d7fcd\", \"_version\": 13, \"attrs\": { }, \"children\": [ { \"uid\": \"6dd64343bf634bfadd4\", \"type\": \"table\", \"attrs\": { \"rows\": 4, \"cols\": 2, \"colWidths\": [ 250, 250 ] }, \"children\": [ { \"uid\": \"b9082\", \"type\": \"thead\", \"attrs\": {}, \"children\": [ { \"type\": \"tr\", \"attrs\": {}, \"children\": [ { \"type\": \"th\", \"attrs\": {}, \"children\": [ { \"type\": \"p\", \"attrs\": {}, \"children\": [ { \"text\": \"Header 1\" } ], \"uid\": \"daa3ef\" } ], \"uid\": \"4b3124414a3\" }, { \"type\": \"th\", \"attrs\": { }, \"children\": [ { \"type\": \"p\", \"attrs\": { }, \"children\": [ { \"text\": \"Header 2\" } ], \"uid\": \"eae83c5797d\" } ], \"uid\": \"bca9b6f037a04fb485\" } ], \"uid\": \"b91ae7a48ef2e9da1\" } ] }, { \"type\": \"tbody\", \"attrs\": { }, \"children\": [ { \"type\": \"tr\", \"attrs\": { }, \"children\": [ { \"type\": \"td\", \"attrs\": { }, \"children\": [ { \"type\": \"p\", \"attrs\": { }, \"children\": [ { \"text\": \"Body row 1 data 1\" } ], \"uid\": \"ec674ccc5ce70b7cab\" } ], \"uid\": \"2a70bdeeb99a\" }, { \"type\": \"td\", \"attrs\": { }, \"children\": [ { \"type\": \"p\", \"attrs\": { }, \"children\": [ { \"text\": \"Body row 1 data 2\" } ], \"uid\": \"769a 3f9db34 ce8ec 10486d50\" } ], \"uid\": \"d6407 34a5c6 1ab1e5f7d1\" } ], \"uid\": \"77f6 b951c68 7f9eb397c5\" }, { \"type\": \"tr\", \"attrs\": { }, \"children\": [ { \"type\": \"td\", \"attrs\": { }, \"children\": [ { \"type\": \"p\", \"attrs\": { }, \"children\": [ { \"text\": \"Body row 2 data 1\" } ], \"uid\": \"a6bf 11bb48 630e87d721c0\" } ], \"uid\": \"3da39838b0feaf\" }, { \"type\": \"td\", \"attrs\": { }, \"children\": [ { \"type\": \"p\", \"attrs\": { }, \"children\": [ { \"text\": \"Body row 2 data 2\" } ], \"uid\": \"3b7d12 1f694202 49029e86313\" } ], \"uid\": \"95b38b04abcbc25e94f\" } ], \"uid\": \"b 227fea 8d247013 4f1e1e8\" } ], \"uid\": \"fd5ab86aa642798451b\" } ] }, ], \"type\": \"doc\" }";
        public const string kLinkInPJson = "{ \"uid\":\"06e34a7190849d7f2acd\", \"_version\":13, \"attrs\":{ }, \"children\":[{\"type\":\"p\",\"attrs\":{\"style\":{\"text-align\":\"left\"},\"redactor-attributes\":{ }},\"uid\":\"d88dcdf4590dff2d\",\"children\":[{\"text\":\"\",\"bold\":true,\"italic\":true,\"underline\":true,\"subscript\":true},{ \"uid\":\"0d06598201aa8b47\",\"type\":\"a\",\"attrs\":{ \"url\":\"LINK.com\",\"target\":\"_self\"},\"children\":[{ \"text\":\"LINK\"}]},{ \"text\":\"\"}]}],\"type\":\"doc\"}";
        public const string kEmbedJson = "{ \"uid\":\"06e34a7190849d7f2acd\", \"_version\":13, \"attrs\":{ }, \"children\":[{\"uid\":\"251017315905c35d42c9\",\"type\":\"embed\",\"attrs\":{\"url\":\"https://www.youtube.com/watch?v=AOP0yARiW8U\"},\"children\":[{\"text\":\"\"}]}],\"type\":\"doc\"}";
        public const string kAssetReferenceJson = "{\"uid\":\"06e34a7  5e4 e549d \", \"_version\":1, \"attrs\":{}, \"children\":[{  \"uid\": \"4f7e33 3390a955 de10c1 c836\", \"type\":\"reference\",\"attrs\":{\"display-type\":\"display\",\"asset-uid\":\"blt44asset\",\"content-type-uid\":\"sys_assets\",\"asset-link\":\"https://images.contentstack.com/v3/assets/blt77263d3e6b/blt73403ee7281/51807f919e0e4/11.jpg\",\"asset-name\":\"11.jpg\",\"asset-type\":\"image/jpeg\",\"type\":\"asset\",\"class-name\":\"embedded-asset\",\"width\":25.16914749661705,\"className\":\"dsd\",\"id\":\"sdf\"},\"children\":[{\"text\":\"\"}]}],\"type\":\"doc\"}";
        public const string kEntryReferenceBlockJson = "{ \"uid\":\"06e34a7  5e4 e549d \", \"_version\":1, \"attrs\":{ }, \"children\":[{\"uid\":\"70f9b 325075d43 128c0d0 aa3eb7f291f\",\"type\":\"reference\",\"attrs\":{\"display-type\":\"block\",\"entry-uid\":\"blttitleuid\",\"content-type-uid\":\"content_block\",\"locale\":\"en-us\",\"type\":\"entry\",\"class-name\":\"embedded-entry\"},\"children\":[{\"text\":\"\"}]}],\"type\":\"doc\"}";
        public const string kEntryReferenceLinkJson = "{ \"uid\":\"06e34a7  5e4 e549d\", \"_version\":1, \"attrs\":{ }, \"children\":[{\"uid\":\"7626ea98e0e95d602210\",\"type\":\"reference\",\"attrs\":{\"target\":\"_self\",\"href\":\"/copy-of-entry-final-02\",\"display-type\":\"link\",\"entry-uid\":\"bltemmbedEntryuid\",\"content-type-uid\":\"embeddedrte\",\"locale\":\"en-us\",\"type\":\"entry\",\"class-name\":\"embedded-entry\"},\"children\":[{\"text\":\"/copy-of-entry-final-02\"}]}],\"type\":\"doc\"}";
        public const string kEntryReferenceInlineJson = "{ \"uid\":\"06e34a7  5e4 e549d\", \"_version\":1, \"attrs\":{ }, \"children\":[{\"uid\":\"506 4878f3f46 s21f0cbc aff\",\"type\":\"reference\",\"attrs\":{\"display-type\":\"inline\",\"entry-uid\":\"blttitleUpdateuid\",\"content-type-uid\":\"embeddedrte\",\"locale\":\"en-us\",\"type\":\"entry\",\"class-name\":\"embedded-entry\"},\"children\":[{\"text\":\"\"}]}],\"type\":\"doc\"}";
        public const string kHRJson = "{ \"uid\":\"06e34a7  5e4 e549d\", \"_version\":1, \"attrs\":{ }, \"children\":[{\"uid\":\"f5a7b57 40a8a5c3 576828276b\",\"type\":\"hr\",\"children\":[{\"text\":\"\"}],\"attrs\":{ }}],\"type\":\"doc\"}";


        public const string KAssetNode = "\"embedded_itemsConnection\": { \"edges\": [{ \"node\": { \"system\": { \"content_type_uid\": \"sys_assets\", \"uid\": \"blt44asset\" }, \"created_at\": \"2020-08-19T09:13:32.785Z\", \"updated_at\": \"2020-08-19T09:13:32.785Z\", \"created_by\": \"bltcreate\", \"updated_by\": \"bltcreate\", \"content_type\": \"application/pdf\", \"file_size\": \"13264\", \"filename\": \"dummy.pdf\", \"url\":\"/v3/assets/blt333/blt44asset/dummy.pdf\", \"_version\": 1, \"title\": \"dummy.pdf\" } } ]}";
        public const string KEntryBlocNode = "\"embedded_itemsConnection\": { \"edges\": [{ \"node\": { \"title\": \"Update this title\", \"url\": \"\", \"locale\": \"en-us\", \"system\": { \"uid\": \"blttitleuid\", \"content_type_uid\": \"content_block\" }, \"_version\": 5, \"_in_progress\": false, \"multi_line\": \"\", \"rich_text_editor\": \"\" } } ]}";
        public const string KEntryLinkNode = "\"embedded_itemsConnection\": { \"edges\": [{ \"node\": { \"title\": \"Entry with embedded entry\", \"rich_text_editor\": [ \"\" ], \"locale\": \"en-us\", \"system\": { \"uid\": \"bltemmbedEntryuid\", \"content_type_uid\": \"embeddedrte\" }, \"_in_progress\": false } } ]}";
        public const string KEntryInlineNode = "\"embedded_itemsConnection\": { \"edges\": [{ \"node\": { \"title\": \"updated title\", \"rich_text_editor\": [ \"\" ], \"locale\": \"en-us\", \"system\": { \"uid\": \"blttitleUpdateuid\", \"content_type_uid\": \"embeddedrte\", }, \"_in_progress\": false } } ]}";
        public static string KGQLModel(string node, string embedConnection = null)
        {
            return $"{{\"multiplerte\":{{\"json\":[{node}]{(embedConnection != null ? ","+embedConnection : "")} }}, \"singlerte\":{{\"json\":{node}{(embedConnection != null ? "," + embedConnection : "")}}} }}";
        }
    }
}
