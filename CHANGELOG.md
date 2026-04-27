### Version: 2.0.0-beta.1
#### Date: April-27-2026
- **Breaking:** Replaced **Newtonsoft.Json** with **System.Text.Json** across the package. The `Newtonsoft.Json` package reference is removed; add `System.Text.Json` (or rely on the BCL on supported runtimes) as needed in consuming projects.
- **Breaking:** Variant metadata APIs that previously took `JObject` / `JArray` now use `System.Text.Json.Nodes.JsonObject` and `JsonArray` (`GetVariantAliases`, `GetVariantMetadataTags`, and obsolete `GetDataCsvariantsAttribute` overloads).
- JSON serialization uses the same model attributes with `System.Text.Json.Serialization` (`JsonPropertyName`, `JsonConverter`), including custom converters for RTE/GQL-shaped JSON and **path-mapped** embedded models (`PathMappedJsonConverter<T>`).
- RTE JSON deserialization tolerates **trailing commas** when using the documented test/helper patterns (`AllowTrailingCommas`); attribute dictionaries may surface **`JsonElement`** values instead of boxed strings—use helpers or unwrap explicitly if you access `Node.attrs` directly.
- Internal: `LangVersion` set to **latest** for multi-target builds; utilities normalize attribute values where the HTML pipeline expects strings.

### Version: 1.2.0
#### Date: March-31-2026
- Added `GetVariantMetadataTags(JObject, string)` and `GetVariantMetadataTags(JArray, string)` as the canonical API for building the `data-csvariants` payload (same behavior as the previous helpers).

### Version: 1.1.0
#### Date: March-24-2026
- Added `GetVariantAliases` and `GetDataCsvariantsAttribute` for variant alias extraction and `data-csvariants` serialization; Invalid arguments throw `ArgumentException`.


### Version: 1.0.7
#### Date: January-12-2026
- Improved error messages

### Version: 1.0.6
#### Date: June-09-2025
- Used 'title' and 'target' in a tags

### Version: 1.0.5
#### Date: Oct-10-2024
- Used Class Name and Id property in JsonToHTML converter

### Version: 1.0.4
#### Date: Aug-21-2024
- Classname and ID property support in text node

### Version: 1.0.3
#### Date: Jul-10-2024
- Editable tags added

### Version: 1.0.2
#### Date: Mar-14-2024
- Style attributes supported in converted HTML.
- Fragment tag support added

### Version: 1.0.1 
#### Date: July-16-2021
- Json RTE content to Html Support added.

### Version: 1.0.0 
#### Date: Apr-05-2021
- Introduce ContentStack Utils SDK for DOTNET.
