# Contentstack Utils .NET – Agent guide

**Universal entry point** for contributors and AI agents. Detailed conventions live in **`skills/*/SKILL.md`**.

## What this repo is

| Field | Detail |
|-------|--------|
| **Name:** | [contentstack-utils-dotnet](https://github.com/contentstack/contentstack-utils-dotnet) |
| **Purpose:** | NuGet library **`contentstack.utils`**: JSON RTE → HTML rendering, embedded entry/asset rendering, GQL-oriented helpers, variant metadata (`GetVariantMetadataTags` / `GetVariantAliases`), Live Preview–style editable tags (`addEditableTags`). This is **not** a full Contentstack Delivery or Management HTTP SDK. |
| **Out of scope (if any):** | No built-in HTTP client to Contentstack APIs; consumers use the Contentstack .NET Delivery SDK or other clients. This package focuses on parsing, rendering, and JSON helpers. |

## Tech stack (at a glance)

| Area | Details |
|------|---------|
| **Language** | C#; library multi-targets **netstandard2.0**, **net47**, **net472** ([`Contentstack.Utils/Contentstack.Utils.csproj`](Contentstack.Utils/Contentstack.Utils.csproj)); test project **net7.0** ([`Contentstack.Utils.Tests/Contentstack.Utils.Tests.csproj`](Contentstack.Utils.Tests/Contentstack.Utils.Tests.csproj)). |
| **Build** | **dotnet** + solution [`Contentstack.Utils.sln`](Contentstack.Utils.sln); shared package version in [`Directory.Build.props`](Directory.Build.props) (e.g. `Version` **1.2.0**). |
| **Tests** | **xUnit**, **Microsoft.NET.Test.Sdk**, **coverlet.collector**; JSON fixtures under [`Contentstack.Utils.Tests/Resources/`](Contentstack.Utils.Tests/Resources/). |
| **Lint / coverage** | No repo-level `.editorconfig` or format workflow; tests use **Coverlet** (`XPlat code coverage`) via the shell scripts. |
| **Key dependencies** | **HtmlAgilityPack**, **Newtonsoft.Json** in the library; tests reference compatible Newtonsoft.Json. |

## Commands (quick reference)

| Command type | Command |
|--------------|---------|
| **Build** | `dotnet build Contentstack.Utils.sln` (add `-c Release` for release configuration). |
| **Test (quick, local)** | `dotnet test Contentstack.Utils.sln` — from repo root; no TRX/coverage (fastest feedback). |
| **Test (CI parity)** | `sh ./Scripts/run-unit-test-case.sh` — clears `Contentstack.Utils.Tests/TestResults`, runs `dotnet test` on the test project with TRX logging and coverage. |
| **Test + HTML coverage report** | `bash ./Scripts/run-test-case.sh` — tests the **solution**, then runs `python3 Scripts/generate_test_report.py` to emit HTML under `Contentstack.Utils.Tests/TestResults/Coverage-.../index.html`. |
| **Pack (release)** | `dotnet pack -c Release -o out` (see [`.github/workflows/nuget-publish.yml`](.github/workflows/nuget-publish.yml)). |
| **SCA (local parity with CI)** | `dotnet restore ./Contentstack.Utils.sln`, then from `Contentstack.Utils`: `snyk test` (requires Snyk CLI and `SNYK_TOKEN`; see [`sca-scan.yml`](.github/workflows/sca-scan.yml)). |

**CI:** [`.github/workflows/unit-test.yml`](.github/workflows/unit-test.yml) runs `run-unit-test-case.sh` on Windows for `pull_request` and `push`.

## CI and automation (workflows)

| Workflow | Trigger | Role |
|----------|---------|------|
| [`unit-test.yml`](.github/workflows/unit-test.yml) | `pull_request`, `push` | Windows: unit tests via `Scripts/run-unit-test-case.sh`. |
| [`back-merge-pr.yml`](.github/workflows/back-merge-pr.yml) | `push` on `master`, manual dispatch | Opens automated back-merge PRs from `master` to `development` to keep branches aligned. |
| [`nuget-publish.yml`](.github/workflows/nuget-publish.yml) | `release` (created) | `dotnet pack -c Release -o out`; push package to NuGet.org and GitHub Packages. |
| [`sca-scan.yml`](.github/workflows/sca-scan.yml) | PR (opened, synchronize, reopened) | Ubuntu: `dotnet restore`, **Snyk** `snyk test` under `Contentstack.Utils`. |
| [`policy-scan.yml`](.github/workflows/policy-scan.yml) | PR (public repos) | Requires `SECURITY.md` and a license file containing the current year. |
| [`codeql-analysis.yml`](.github/workflows/codeql-analysis.yml) | `pull_request` (branches `*`) | CodeQL analysis for **csharp** (autobuild). |
| [`issues-jira.yml`](.github/workflows/issues-jira.yml) | `issues` (opened) | Creates Jira tickets from GitHub issues (secrets). |

## Where the documentation lives: skills

| Skill | Path | What it covers |
|-------|------|----------------|
| Dev workflow | [`skills/dev-workflow/SKILL.md`](skills/dev-workflow/SKILL.md) | Branches, build/test/pack, CI, security workflows, CODEOWNERS, Talisman. |
| Testing | [`skills/testing/SKILL.md`](skills/testing/SKILL.md) | xUnit layout, fixtures, mocks, coverage scripts, alignment with CI. |
| Code review | [`skills/code-review/SKILL.md`](skills/code-review/SKILL.md) | PR expectations, checklist, security notes. |
| Contentstack Utils (API) | [`skills/contentstack-utils/SKILL.md`](skills/contentstack-utils/SKILL.md) | Public API, package boundaries, `Utils` / GQL / variants, dependencies. |
| C# style (this repo) | [`skills/csharp-style/SKILL.md`](skills/csharp-style/SKILL.md) | Folder layout, namespaces, naming consistency, TFMs. |
