---
name: csharp-style
description: Use for folder placement, namespaces, naming consistency, and multi-target framework rules in Contentstack.Utils.
---

# C# style (this repo) – Contentstack Utils .NET

## When to use

- Adding new types to the library or tests.
- Choosing namespaces and file locations to match existing structure.
- Ensuring changes remain compatible with **netstandard2.0** and desktop TFMs.

## Instructions

### Library folder taxonomy

Under [`Contentstack.Utils/`](../../Contentstack.Utils/), place new code according to its role:

| Folder | Contents |
|--------|----------|
| `Interfaces/` | [`IEntryEmbedable.cs`](../../Contentstack.Utils/Interfaces/IEntryEmbedable.cs) (`IEntryEmbedable`); [`IEmbeddedObject.cs`](../../Contentstack.Utils/Interfaces/IEmbeddedObject.cs) (`IEmbeddedObject`, `IEmbeddedEntry`, `EditableEntry`, `IEmbeddedAsset`); [`IOptions.cs`](../../Contentstack.Utils/Interfaces/IOptions.cs) (`IRenderable`, `NodeChildrenCallBack`); [`IEdges.cs`](../../Contentstack.Utils/Interfaces/IEdges.cs) (`IEdges<T>`). |
| `Models/` | DTOs and [`Options.cs`](../../Contentstack.Utils/Models/Options.cs) (`Options` base class). |
| `Enums/` | [`MarkType`](../../Contentstack.Utils/Enums/MarkType.cs), [`StyleType`](../../Contentstack.Utils/Enums/StyleType.cs), [`EmbedItemType`](../../Contentstack.Utils/Enums/EmbedItemType.cs). |
| `Extensions/` | Extension methods — [`HtmlDocumentExtension.cs`](../../Contentstack.Utils/Extensions/HtmlDocumentExtension.cs) (`FindEmbeddedObject` on **HtmlAgilityPack** `HtmlDocument`). |
| `Converters/` | Newtonsoft.Json converters for RTE/node JSON ([`NodeJsonConverter.cs`](../../Contentstack.Utils/Converters/NodeJsonConverter.cs), [`RTEJsonConverter.cs`](../../Contentstack.Utils/Converters/RTEJsonConverter.cs)). |
| `Constants/` | Shared strings — [`ErrorMessages.cs`](../../Contentstack.Utils/Constants/ErrorMessages.cs). |

### Namespaces

- Mirror the folder structure: `Contentstack.Utils.Interfaces`, `Contentstack.Utils.Models`, and so on. Keep **`Contentstack.Utils`** as the root for public surface consistency.

### Naming consistency

- The codebase mixes typical .NET **PascalCase** for most public members with some **camelCase** public static names (notably **`addEditableTags`** on [`Utils`](../../Contentstack.Utils/Utils.cs)). For new code, **prefer consistency with the nearest existing API** in the same class or feature area rather than blindly applying IDE defaults, so consumers see uniform style per subsystem.

### Target frameworks

- **Library** ([`Contentstack.Utils.csproj`](../../Contentstack.Utils/Contentstack.Utils.csproj)): **netstandard2.0**, **net47**, **net472**. Public APIs must be implementable on all targets; avoid APIs that only compile on .NET 5+ unless you intentionally raise baselines with a major version policy.
- **Tests** ([`Contentstack.Utils.Tests.csproj`](../../Contentstack.Utils.Tests/Contentstack.Utils.Tests.csproj)): **net7.0** only—fine for test-only APIs; do not assume test-only APIs exist in the library.

## References

- [`skills/contentstack-utils/SKILL.md`](../contentstack-utils/SKILL.md) — what belongs in the public API.
- [`skills/testing/SKILL.md`](../testing/SKILL.md) — test project structure.
