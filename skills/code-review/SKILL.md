---
name: code-review
description: Use for PR expectations, review checklist, docs/changelog, and security considerations in contentstack-utils-dotnet.
---

# Code review – Contentstack Utils .NET

## When to use

- Opening or reviewing a pull request.
- Deciding whether README/CHANGELOG updates or extra tests are required.
- Assessing impact of new dependencies or public API changes.

## Instructions

### Branch and ownership

- Merges into **`master`** are expected from **`staging`** only (see [`check-branch.yml`](../../.github/workflows/check-branch.yml)). Align your PR base/head with team process.
- [`CODEOWNERS`](../../CODEOWNERS) may request reviews from **`@contentstack/devex-pr-reviewers`** and security admins for workflow or `.snyk` changes.

### Checklist

- **Behavior**: New or changed logic in [`Contentstack.Utils`](../../Contentstack.Utils/) should have **xUnit** coverage in [`Contentstack.Utils.Tests`](../../Contentstack.Utils.Tests/) unless truly non-functional (e.g. comment-only).
- **Public API**: User-visible changes should update [`README.md`](../../README.md) and [`CHANGELOG.md`](../../CHANGELOG.md) as appropriate.
- **Multi-targeting**: The library builds **netstandard2.0**, **net47**, **net472**. After API or dependency changes, verify `dotnet build` succeeds for all target frameworks (or rely on CI).
- **Dependencies**: New or upgraded NuGet packages affect **Snyk** ([`sca-scan.yml`](../../.github/workflows/sca-scan.yml)) and **CodeQL** ([`codeql-analysis.yml`](../../.github/workflows/codeql-analysis.yml)). Ensure licenses and advisories are acceptable.
- **Secrets**: Never commit API keys, tokens, or connection strings. CI uses GitHub secrets only.

### Severity labels (optional)

Teams may use **Blocker** (must fix before merge), **Major** (should fix or track), **Minor** (nit / follow-up)—keep comments actionable.
