# Changelog

All notable changes to this project will be documented in this file.

## [0.1.0] - 2026-04-25

### Added

- Initial HoneyDrunk.Vault.Rotation Function App scaffold for ADR-0006 Tier 2 third-party secret rotation.
- Public rotation contracts in HoneyDrunk.Vault.Rotation.Abstractions.
- Provider discovery scaffolding and Resend, Twilio, and OpenAI rotator stubs.
- Timer-triggered rotation executions now establish Kernel Grid/Operation context with internal tenant context before invoking rotators.
- Rotation context correlation IDs are required and non-empty.
- Placeholder provider rotators now share a reusable scaffold while preserving provider-specific names and skipped messages.
- Unit and canary project stubs.
- GitHub Actions workflows for PR validation, release artifact publication, and staging deployment.

## [0.0.1] - 2026-04-25

### Added

- Repository bootstrap placeholder.
