# Contentstack Utils Dotnet

Contentstack is a headless CMS with an API-first approach. It is a CMS that developers can use to build powerful cross-platform applications in their favorite languages. Build your application frontend, and Contentstack will take care of the rest. Read More.


This guide will help you get started with Contentstack .NET Utils SDK to build apps powered by Contentstack.

## Prerequisites
To get started with .NET, you will need the following:

-   .NET 10 or later

> **Migrating from v1.x?** Version 2.0.0 replaces Newtonsoft.Json with System.Text.Json and changes several public method signatures. See the [migration guide](https://www.contentstack.com/docs/developers/sdks/utils-sdk/dot-net/migrate-dotnet-utils-sdk-from-newtonsoft-to-stj) before upgrading.

## Setup and Installation

Note: If you are using Contentstack .NET SDK, you don’t need to download the Contentstack.Utils package separately as it will be already available for use.

To download the Contentstack.Utils module, open the terminal and perform any of the following options:

-   Via Package Manager:
```sh
PM> Install-Package contentstack.utils
```

-   Via .NET CLI:
```sh
dotnet add package contentstack.utils
```
 
After successful installation, to use the module in your application, you need to add a namespace to your class:
```c#
using Contentstack.Utils
```

## Usage

Let’s learn how you can use .NET Utils SDK to render embedded items by performing the following steps:

1.  ### Create CustomRenderOption class

To render embedded items on the front-end, use the CustomRenderOption class, and define the UI elements you want to show in the front-end of your website, as shown in the example below. In this example, we have specified the cases for each method of adding embedded items: Block, Inline, Link, Display and Download (for asset).
```c#
using System.Collections.Generic;  
using Contentstack.Utils.Interfaces;  
using Contentstack.Utils.Models;  
using Contentstack.Utils.Enums;  
public  class CustomRenderOption: Options  
{  
	public CustomRenderOption(IEntryEmbedable entry) : base(entry)  
	{  
	}
	public override string RenderMark(MarkType markType, string text)
    {
        switch (markType)
        {
            case MarkType.Bold:
                return $"<b>{text}</b>";
            default:
                return base.RenderMark(markType: markType, text: text);
        }
    }

    public override string RenderNode(string nodeType, Node node, NodeChildrenCallBack callBack)
    {
        switch (nodeType)
        {
            case "p":
                return $"<p class='class-id'>{callBack(node.children)}</p>";
            case "h1":
                return "<h1 class='class-id'>{necallBackxt(node.children)}</h1>";
            default:
                return base.RenderNode(nodeType, node, callBack);
        }
    }

	public  override  string RenderOption(IEmbeddedObject embeddedObject, Metadata metadata)  
	{  
		switch (metadata.StyleType)  
			{

			//if you have added embedded object using the “Block” option  
			case StyleType.Block:  
				string renderString = "";  
				if (embeddedObject is IEmbeddedEntry)  
				{  
					renderString += $"<div> <b>{EscapeText(((IEmbeddedEntry)embeddedObject).Title)}</b></div>";  
				}  
				else  if (embeddedObject is IEmbeddedContentTypeUid)  
				{  
					renderString += $"<div> <b>{EscapeText(embeddedObject.Uid)}</b></div>";  
				}  
				return renderString;  
			
			//if you have added embedded object using the “Inline” option
			case StyleType.Inline:  
				if (embeddedObject is IEmbeddedEntry)  
				{  
					return  $"<span><b>{EscapeText(((IEmbeddedEntry)embeddedObject).Title)}</b></span>";  
				}  
				else  if (embeddedObject is IEmbeddedContentTypeUid)  
				{  
					return  $"<span><b>{EscapeText(embeddedObject.Uid)}</b></span>";  
				}  
				return  "<span>" + EscapeText(embeddedObject.Uid) + "</span>";  
				
			//if you have added embedded object using the “Link” option
			case StyleType.Link:  
				if (embeddedObject is IEmbeddedEntry)  
				{ 
					return  $"<span> Please find link to: <a><b>{EscapeText(metadata.Text ?? ((IEmbeddedEntry)embeddedObject).Title)}</b></a></span>";  
				}  
				else  if (embeddedObject is IEmbeddedContentTypeUid)  
				{  
					return  $"<span> Please find link to: <a><b>{EscapeText(metadata.Text ?? embeddedObject.Uid)}</b></a></span>";  
				}  
				return  "<a href=\"" + EscapeAttribute(embeddedObject.Uid) + "\">" + EscapeText(metadata.Text ?? embeddedObject.Uid) + "</a>";  

			//if you have embedded an asset into the RTE field
			case StyleType.Display:  
				if (embeddedObject is IEmbeddedAsset)  
				{  
					var asset = (IEmbeddedAsset)embeddedObject;
					return  $"<b>{EscapeText(asset.Title)}</b><p>{EscapeText(asset.FileName)} image: <img src=\"{EscapeAttribute(asset.Url)}\" alt=\"{EscapeAttribute(asset.Title)}\"/></p>";  
				}  
				return  "<img src=\"" + EscapeAttribute(embeddedObject.Uid) + "\" alt=\"" + EscapeAttribute(embeddedObject.Uid) + "\" />";  

			//if you have embedded an asset directly via a download link.
			case StyleType.Download:  
				if (embeddedObject is IEmbeddedAsset)  
				{  
					return  "<span> Please find link to: <a href=\"" + EscapeAttribute(((IEmbeddedAsset)embeddedObject).Url) + "\">" + EscapeText(metadata.Text ?? ((IEmbeddedAsset)embeddedObject).Title) + "</a></span>";  
				}  
				return  "<a href=\"" + EscapeAttribute(embeddedObject.Uid) + "\">" + EscapeText(metadata.Text ?? embeddedObject.Uid) + "</a>";  
		}  
		return  base.RenderOption(embeddedObject, metadata);  
	}  
}
```
  
2.  ### Initialize the class
Initialize either the Options or CustomRenderOption class to use them for rendering embedded items as shown below:
```c#
//To use the default render option:  
Options defaultRender = new Options(entry);
  
//To use CustomRenderOptions:  
CustomRenderOption defaultRender = new CustomRenderOption(entry);  
  ```
> Note: Make sure the entry parameter has implemented the IEmbeddedObject property.

> **Note:** As of v2.0.0, `Node.attrs` values are `System.Text.Json.JsonElement` instead of `Newtonsoft.Json.Linq.JToken`. If you access `attrs` directly, update the accessor:
> ```c#
> // Before (v1.x)
> string src = node.attrs["src"].Value<string>();
> // After (v2.0.0)
> string src = ((System.Text.Json.JsonElement)node.attrs["src"]).GetString();
> ```

## HTML escaping

From v2.1.0 the default renderers escape RTE content before writing it into HTML, the same way the Java Utils SDK does. This is on by default.

- **Text nodes:** `&`, `<` and `>` are escaped, so HTML typed as text displays as text.
- **Attribute values** (`href`, `src`, `title`, `target`, `style`, `class`, `id`, `alt`): encoded with `System.Net.WebUtility.HtmlEncode`, so a value can't break out of its attribute.
- **URL schemes are not filtered.** As in the Java and JavaScript Utils SDKs, a `javascript:` URL stored in the content is written into `href` as is. If authors you don't trust can edit content, sanitize the rendered HTML (for example with an HTML sanitizer library) before you display it.

Custom renderers now receive escaped text in `RenderMark` and escaped child HTML from the `RenderNode` callback, so don't escape those again. They are safe as element content, not inside an attribute. Values you read yourself, such as `node.attrs`, entry fields or the `className`/`id` arguments, still arrive raw. Escape them with `EscapeText` and `EscapeAttribute` on `Options`:

```c#
public override string RenderNode(string nodeType, Node node, NodeChildrenCallBack callBack)
{
    if (nodeType == "a")
    {
        string url = ((System.Text.Json.JsonElement)node.attrs["url"]).GetString();
        return $"<a class=\"link\" href=\"{EscapeAttribute(url)}\">{callBack(node.children)}</a>";
    }
    return base.RenderNode(nodeType, node, callBack);
}
```

To restore the unescaped output of earlier versions, set `EscapeHtml = false`:

```c#
Options renderOption = new Options(entry) { EscapeHtml = false };
```

The HTML RTE path (`Utils.RenderContent`) still returns the stored HTML as is; only the embedded entries and assets it replaces are escaped.

## Variant Utility

Use the `Utils.GetVariantAliases` and `Utils.GetVariantMetadataTags` methods to extract variant alias information from an entry and build the `data-csvariants` attribute payload used for [Personalize](https://www.contentstack.com/docs/personalize) variants. As of v2.0.0, these methods accept `System.Text.Json.Nodes.JsonObject` / `JsonArray` instead of `Newtonsoft.Json.Linq.JObject` / `JArray`.

```c#
using System.Text.Json.Nodes;
using Contentstack.Utils;

JsonObject entry = JsonNode.Parse(entryJson)!.AsObject();

// Get variant aliases for a single entry
JsonObject aliases = Utils.GetVariantAliases(entry, "product");

// Build the data-csvariants attribute payload for a single entry
JsonObject variantTags = Utils.GetVariantMetadataTags(entry, "product");

// Both methods also accept a JsonArray of entries
JsonArray entries = JsonNode.Parse(entriesJson)!.AsArray();
JsonArray aliasesForEntries = Utils.GetVariantAliases(entries, "product");
JsonObject variantTagsForEntries = Utils.GetVariantMetadataTags(entries, "product");
```

## Basic Queries

Contentstack Utils SDK lets you interact with the Content Delivery APIs and retrieve embedded items from the RTE field of an entry.

### Fetch Embedded Item(s) from a Single Entry
#### Render HTML RTE Embedded object

To get an embedded item of a single entry, you need to provide the stack API key, environment name, delivery token, content type and entry UID. Then, use the `Utils.RenderContent` functions as shown below:
```c#		
using Contentstack.Core; // ContentstackClient  
using Contentstack.Core.Models; // Stack, Query, Entry, Asset, ContentType, ContentstackCollection  
using Contentstack.Core.Configuration; // ContentstackOptions  
using Contentstack.Utils; // Utils.RenderContent  
Using Contentstack.Utils.Models; // Options, Metadata  
ContentstackClient client = new ContentstackClient("api_key", "delivery_token", "environment_name");  
  
client.ContentType("product").Entry("<entry_uid>");  
	.includeEmbeddedItems()
	.Fetch<Product>().ContinueWith((response) => {  
		if (!response.IsFaulted) {
			// To use the default render option:
			string result = Utils.RenderContent(response.result.rte, new Option(response.result));
			// To use the Custom render option:  
			string result = Utils.RenderContent(response.result.rte, new CustomRenderOption(response.result));  
		}  
});
  ```

#### Render Supercharged RTE contents

To get a single entry, you need to provide the stack API key, environment name, delivery token, content type and entry UID. Then, use `Utils.JsonToHtml` function as shown below:
```c#		
using Contentstack.Core; // ContentstackClient  
using Contentstack.Core.Models; // Stack, Query, Entry, Asset, ContentType, ContentstackCollection  
using Contentstack.Core.Configuration; // ContentstackOptions  
using Contentstack.Utils; // Utils.RenderContent  
Using Contentstack.Utils.Models; // Options, Metadata  
ContentstackClient client = new ContentstackClient("api_key", "delivery_token", "environment_name");  
  
client.ContentType("product").Entry("<entry_uid>");  
	.includeEmbeddedItems()
	.Fetch<Product>().ContinueWith((response) => {  
		if (!response.IsFaulted) {
			// To use the default render option:
			string result = Utils.JsonToHtml(response.result.rte, new Option(response.result));
			// To use the Custom render option:  
			string result = Utils.JsonToHtml(response.result.rte, new CustomRenderOption(response.result));  
		}  
});
  ```
### Fetch Embedded Item(s) from Multiple Entries
#### Render HTML RTE Embedded object
To get embedded items from multiple entries, you need to provide the stack API key, environment name, delivery token, content type UID. Then, use the `Utils.RenderContent` functions as shown below:
```c#
using Contentstack.Core; // ContentstackClient  
using Contentstack.Core.Models; // Stack, Query, Entry, Asset, ContentType, ContentstackCollection  
using Contentstack.Core.Configuration; // ContentstackOptions  
using Contentstack.Utils; // Utils.RenderContent  
Using Contentstack.Utils.Models; // Options, Metadata  
  
ContentstackClient client = new ContentstackClient("api_key", "delivery_token", "environment_name");  
  
client.ContentType("product").Query()  
	.includeEmbeddedItems();  
	.Find<Product>().ContinueWith((t) => {  
		if (!t.IsFaulted) {  
			ContentstackCollection<Product> result = t.Result;  
			foreach (var product in result.Items)  
			{  
				// To use the default render option  
				string result = Utils.RenderContent(product.rte, new Option(product));  
				// To use the Custom render option  
				string result = Utils.RenderContent(product.rte, new CustomRenderOption(product));  
			}  
		}  
	});
```


#### Render Supercharged RTE contents
To get embedded items from multiple entries, you need to provide the stack API key, environment name, delivery token, content type UID. Then, use `Utils.jsonToHtml` function as shown below:

```c#
using Contentstack.Core; // ContentstackClient  
using Contentstack.Core.Models; // Stack, Query, Entry, Asset, ContentType, ContentstackCollection  
using Contentstack.Core.Configuration; // ContentstackOptions  
using Contentstack.Utils; // Utils.RenderContent  
Using Contentstack.Utils.Models; // Options, Metadata  
  
ContentstackClient client = new ContentstackClient("api_key", "delivery_token", "environment_name");  
  
client.ContentType("product").Query()  
	.includeEmbeddedItems();  
	.Find<Product>().ContinueWith((t) => {  
		if (!t.IsFaulted) {  
			ContentstackCollection<Product> result = t.Result;  
			foreach (var product in result.Items)  
			{  
				// To use the default render option  
				string result = Utils.JsonToHtml(product.rte, new Option(product));  
				// To use the Custom render option  
				string result = Utils.JsonToHtml(product.rte, new CustomRenderOption(product));  
			}  
		}  
	});
```