### Version: 1.3.1
#### Date: June-23-2026
- Added `EmbeddedObject` as a concrete implementation of `IEmbeddedObject`, covering both `IEmbeddedEntry` and `IEmbeddedAsset`.
- Added `EmbeddedObjectConverter` to resolve `IEmbeddedObject` during JSON deserialization without requiring changes in consumer code.
- Custom fields on embedded entries and assets are preserved via `[JsonExtensionData]`.

### Version: 1.3.0

#### Date: May-11-2026
- Added Live Preview editable tags support through `addEditableTags` and `addTags`.
- Added variant-aware CSLP tag generation using `_applied_variants` / `system.applied_variants`.
- Added nested fields, arrays, references, and null-safe editable tag generation.
- Added configurable locale casing through `AddEditableTagsOptions.UseLowerCaseLocale`.
- Added unit tests for Live Preview editable tags.


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
