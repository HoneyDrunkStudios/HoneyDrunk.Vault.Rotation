# Changelog

## [0.1.1] - 2026-09-26

### Changed

- Refresh stable NuGet dependencies; preserve target frameworks and HoneyDrunk public contracts.

| Dependency | Previous | Updated |
| --- | --- | --- |
| Azure.Security.KeyVault.Secrets | 4.10.0 | 4.11.1 |
| Microsoft.Azure.Functions.Worker.Sdk | 2.0.7 | 2.1.0 |
| Microsoft.CodeAnalysis.NetAnalyzers | 10.0.202 | 10.0.401 |
| Microsoft.Extensions.DependencyInjection | 10.0.7 | 10.0.12 |
| Microsoft.Extensions.DependencyInjection.Abstractions | 10.0.7 | 10.0.12 |
| Microsoft.Extensions.Hosting | 10.0.7 | 10.0.12 |


All notable changes to this project will be documented in this file.



### Verified HoneyDrunk dependencies

- HoneyDrunk.Kernel: 0.7.0 -> 0.8.1 (verified on NuGet.org).
- HoneyDrunk.Standards: 0.2.9 -> 0.3.0 (verified on NuGet.org).
- HoneyDrunk.Standards.Tests: 0.2.9 -> 0.3.0 (verified on NuGet.org).
- HoneyDrunk.Vault: 0.5.0 -> 0.8.1 (verified on NuGet.org).
- HoneyDrunk.Vault.Providers.AzureKeyVault: 0.5.0 -> 0.8.1 (verified on NuGet.org).

## [Unreleased]

### Changed

- Enabled ADR-0044 Grid Review request workflow and repo-local OpenClaw/Codex review configuration.

### Internal

- Adopted HoneyDrunk.Standards.Tests 0.2.9 for Vault.Rotation test and canary projects, removed direct test SDK / runner / coverlet references, refreshed HoneyDrunk.Standards to 0.2.9, and covered runtime bootstrap configuration for ADR-0047 alignment.
- Backfilled test coverage above the Grid PR coverage gate floor and seeded the coverage baseline ratchet artifact.

## [0.1.0] - 2026-04-25

### Added

- Initial HoneyDrunk.Vault.Rotation Function App scaffold for ADR-0006 Tier 2 third-party secret rotation.
- Public rotation contracts in HoneyDrunk.Vault.Rotation.Abstractions.
- Provider discovery scaffolding and Resend, Twilio, and OpenAI rotator stubs.
- Timer-triggered rotation executions now establish Kernel Grid/Operation context with internal tenant context before invoking rotators.
- Rotation context correlation IDs are required and non-empty.
- Placeholder provider rotators now share a reusable scaffold while preserving provider-specific names and skipped messages.
- Runtime/test dependencies refreshed for current Kernel/Vault alignment validation.
- Unit and canary project stubs.
- GitHub Actions workflows for PR validation, release artifact publication, and staging deployment.

## [0.0.1] - 2026-04-25

### Added

- Repository bootstrap placeholder.
