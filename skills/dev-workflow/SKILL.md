---
name: dev-workflow
description: Use for branches, build/pack, test scripts, CI workflows, versioning, CODEOWNERS, and security tooling in contentstack-utils-dotnet.
---

# Dev workflow – Contentstack Utils .NET

## When to use

- Setting up the repo locally, cutting releases, or debugging CI.
- Changing GitHub Actions, branch protection expectations, or dependency scanning.
- Bumping package version or understanding how NuGet publish is triggered.

## Instructions

### Branching and merges

- Release flow is direct **`development` -> `master`** (no `staging` promotion step).
- [`.github/workflows/back-merge-pr.yml`](../../.github/workflows/back-merge-pr.yml) opens an automated PR from `master` back to `development` after changes land on `master`.

### Versioning

- Package version is centralized in [`Directory.Build.props`](../../Directory.Build.props) (`Version` property). [`Contentstack.Utils.csproj`](../../Contentstack.Utils/Contentstack.Utils.csproj) uses `PackageVersion` / `ReleaseVersion` tied to `$(Version)` where applicable—bump version in one place for releases.

### Build

- From repo root: `dotnet build Contentstack.Utils.sln` (use `-c Release` for release builds).

### Test scripts

- **`Scripts/run-unit-test-case.sh`**: Deletes `Contentstack.Utils.Tests/TestResults`, runs `dotnet test Contentstack.Utils.Tests/Contentstack.Utils.Tests.csproj` with TRX logger `Report-Contentstack-DotNet-Test-Case.trx` and `XPlat code coverage`. This is what [`.github/workflows/unit-test.yml`](../../.github/workflows/unit-test.yml) runs on **Windows**.
- **`Scripts/run-test-case.sh`**: Runs `dotnet test` on the **solution** with a date-stamped TRX name, collects coverage, finds `coverage.cobertura.xml`, then runs **`python3 Scripts/generate_test_report.py`** to produce HTML under `Contentstack.Utils.Tests/TestResults/Coverage-.../index.html`. Use this for a local combined test + coverage report (Python 3 stdlib only).

### Pack and publish

- Local pack: `dotnet pack -c Release -o out` (same as [`.github/workflows/nuget-publish.yml`](../../.github/workflows/nuget-publish.yml)).
- Publishing: triggered on **GitHub release created**. Jobs build on Windows, pack, and push `contentstack.utils.*.nupkg` to NuGet.org and GitHub Packages (requires GitHub secrets, including **`NUGET_API_KEY`** for NuGet.org and GitHub Packages auth where applicable; do not commit secrets).

### CI and security jobs

| Workflow | Purpose |
|----------|---------|
| `unit-test.yml` | Windows unit tests via `run-unit-test-case.sh`. |
| `back-merge-pr.yml` | Auto-open `master` → `development` back-merge PRs. |
| `nuget-publish.yml` | Pack and push on release. |
| `sca-scan.yml` | `dotnet restore` + **Snyk** `snyk test` in `Contentstack.Utils` (needs `SNYK_TOKEN`). |
| `policy-scan.yml` | For **public** repos: `SECURITY.md` and license file with current calendar year. |
| `codeql-analysis.yml` | CodeQL **csharp** with autobuild. |
| `issues-jira.yml` | Mirror new issues to Jira (Atlassian actions + secrets). |

### CODEOWNERS

- [`CODEOWNERS`](../../CODEOWNERS): default review **`@contentstack/devex-pr-reviewers`**; **`@contentstack/security-admin`** for security workflows, `.snyk`, and related paths.
- **Note:** `CODEOWNERS` references `.github/workflows/codeql-anaylsis.yml` (typo). The actual file is **`codeql-analysis.yml`**. Fix the typo in `CODEOWNERS` in a dedicated PR if you want CodeQL ownership to apply.

### Talisman

- [`.talismanrc`](../../.talismanrc) pins checksums for specific files. Do not add ignores or weaken checks without security team agreement.
