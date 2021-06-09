# Contentstack Utils Dotnet

Contentstack is a headless CMS with an API-first approach. It is a CMS that developers can use to build powerful cross-platform applications in their favorite languages. Build your application frontend, and Contentstack will take care of the rest. Read More.


This guide will help you get started with Contentstack .NET Utils SDK to build apps powered by Contentstack.

## Prerequisites
To get started with .NET, you will need the following:

-   .NET version 3.1 or later
   
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

    public override string RenderNode(NodeType nodeType, Node node, NodeChildrenCallBack callBack)
    {
        switch (nodeType)
        {
            case NodeType.Paragraph:
                return $"<p class='class-id'>{callBack(node.children)}</p>";
            case NodeType.Heading_1:
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
					renderString += $"<div> <b>{((IEmbeddedEntry)embeddedObject).Title}</b></div>";  
				}  
				else  if (embeddedObject is IEmbeddedContentTypeUid)  
				{  
					renderString += $"<div> <b>{embeddedObject.Uid}</b></div>";  
				}  
				return renderString;  
			
			//if you have added embedded object using the “Inline” option
			case StyleType.Inline:  
				if (embeddedObject is IEmbeddedEntry)  
				{  
					return  $"<span><b>{((IEmbeddedEntry)embeddedObject).Title}</b></span>";  
				}  
				else  if (embeddedObject is IEmbeddedContentTypeUid)  
				{  
					return  $"<span><b>{embeddedObject.Uid}</b></span>";  
				}  
				return  "<span>" + embeddedObject.Uid + "</span>";  
				
			//if you have added embedded object using the “Link” option
			case StyleType.Link:  
				if (embeddedObject is IEmbeddedEntry)  
				{ 
					return  $"<span> Please find link to: <a><b>{metadata.Text ?? ((IEmbeddedEntry)embeddedObject).Title}</b></a></span>";  
				}  
				else  if (embeddedObject is IEmbeddedContentTypeUid)  
				{  
					return  $"<span> Please find link to: <a><b>{metadata.Text ?? embeddedObject.Uid}</b></a></span>";  
				}  
				return  "<a href=\"" + embeddedObject.Uid + "\">" + (metadata.Text ?? embeddedObject.Uid) + "</a></span>";  

			//if you have embedded an asset into the RTE field
			case StyleType.Display:  
				if (embeddedObject is IEmbeddedAsset)  
				{  
					return  $"<b>{((IEmbeddedAsset)embeddedObject).Title}</b><p>{((IEmbeddedAsset)embeddedObject).FileName} image: <img src=\"{((IEmbeddedAsset)embeddedObject).Url}\" alt=\"{((IEmbeddedAsset)embeddedObject).Title}\"/></p>";  
				}  
				return  "<img src=\"" + embeddedObject.Uid + "\" alt=\"" + embeddedObject.Uid + "\" />";  

			//if you have embedded an asset directly via a download link.
			case StyleType.Download:  
				if (embeddedObject is IEmbeddedAsset)  
				{  
					return  "<span> Please find link to: <a href=\"" + ((IEmbeddedAsset)embeddedObject).Url + "\">" + (metadata.Text ?? ((IEmbeddedAsset)embeddedObject).Title) + "</a></span>";  
				}  
				return  "<a href=\"" + embeddedObject.Uid + "\">" + (metadata.Text ?? embeddedObject.Uid) + "</a>";  
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

## Basic Queries

Contentstack Utils SDK lets you interact with the Content Delivery APIs and retrieve embedded items from the RTE field of an entry.

### Fetch Embedded Item(s) from a Single Entry
#### Render HTML RTE Embedded object

To get an embedded item of a single entry, you need to provide the stack API key, environment name, delivery token, content type and entry UID. Then, use the `ContentstackUtils.RenderContent` functions as shown below::
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
To get embedded items from multiple entries, you need to provide the stack API key, environment name, delivery token, content type UID. Then, use the `ContentstackUtils.RenderContent` functions as shown below:
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