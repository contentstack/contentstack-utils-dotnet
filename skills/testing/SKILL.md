---
name: testing
description: Use for xUnit tests, JSON fixtures, mocks, Coverlet coverage, and scripts in Contentstack.Utils.Tests.
---

# Testing – Contentstack Utils .NET

## When to use

- Adding or changing behavior in `Contentstack.Utils` and needing unit tests.
- Debugging CI test failures on Windows (unit-test workflow).
- Generating local HTML coverage reports.

## Instructions

### Test project

- [`Contentstack.Utils.Tests.csproj`](../../Contentstack.Utils.Tests/Contentstack.Utils.Tests.csproj): targets **net7.0**, **IsPackable** false, references the main library project.
- Packages: **xunit**, **xunit.runner.visualstudio**, **Microsoft.NET.Test.Sdk**, **coverlet.collector**, **Newtonsoft.Json** (aligned with test needs).

### Test classes (inventory)

Use these as a map of coverage areas when adding related behavior:

| Class | Focus |
|-------|--------|
| `JsonToHtmlTest` | JSON RTE → HTML |
| `GQLTest` | `Utils.GQL` / GraphQL-shaped RTE |
| `UtilsTest` | Core `Utils` APIs |
| `UtilsArrayStringTest` | Array/string rendering paths |
| `UtilsCustomRenderTest` | Custom `Options` / rendering |
| `DefaultRenderTest` | Default render behavior |
| `HtmlDocumentExtensionTest` | HTML document extensions |
| `MetadataTest` | Embedded metadata |
| `VariantAliasesTest` | Variant aliases / metadata tags |

### Fixtures and content

- JSON files live under [`Contentstack.Utils.Tests/Resources/`](../../Contentstack.Utils.Tests/Resources/). The csproj uses `Content Include="Resources\**\*.json" CopyToOutputDirectory="PreserveNewest"` so files are available at test runtime.
- Checked-in fixtures: [`variantsSingleEntry.json`](../../Contentstack.Utils.Tests/Resources/variantsSingleEntry.json), [`variantsEntries.json`](../../Contentstack.Utils.Tests/Resources/variantsEntries.json).

### Constants, helpers, mocks

- Shared expected strings/constants: [`Constants/`](../../Contentstack.Utils.Tests/Constants/).
- Parsing helpers: [`Helpers/NodeParser.cs`](../../Contentstack.Utils.Tests/Helpers/NodeParser.cs).
- Test doubles in [`Mocks/`](../../Contentstack.Utils.Tests/Mocks/): [`CustomRenderOptionMock.cs`](../../Contentstack.Utils.Tests/Mocks/CustomRenderOptionMock.cs), [`DefaultRenderMock.cs`](../../Contentstack.Utils.Tests/Mocks/DefaultRenderMock.cs), [`EmbeddedModelMock.cs`](../../Contentstack.Utils.Tests/Mocks/EmbeddedModelMock.cs), [`GQLModel.cs`](../../Contentstack.Utils.Tests/Mocks/GQLModel.cs). Follow existing patterns for new scenarios.

### Quick local test

- From repo root: `dotnet test Contentstack.Utils.sln` — runs all tests without the shell scripts (no TRX or coverage collection).

### Coverage

- Scripts use Coverlet’s **XPlat code coverage** and emit Cobertura XML (e.g. `coverage.cobertura.xml` under `TestResults`).
- For HTML: run [`Scripts/run-test-case.sh`](../../Scripts/run-test-case.sh), which invokes [`Scripts/generate_test_report.py`](../../Scripts/generate_test_report.py) (Python 3, standard library only).

### CI alignment

- GitHub Actions runs [`Scripts/run-unit-test-case.sh`](../../Scripts/run-unit-test-case.sh) on **Windows**. When reproducing CI failures, use that script (or the same `dotnet test` arguments) from repo root.
