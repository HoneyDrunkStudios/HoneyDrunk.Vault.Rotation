# Changelog

All notable changes to this project will be documented in this file.

## [0.1.1] - 2026-09-26

### Changed

- Refresh dependency and shared build-tooling versions; preserve target frameworks and existing public contracts. See the [repository dependency changes](../../CHANGELOG.md).

## [Unreleased]

## [0.1.0] - 2026-04-25

### Added

- Initial documented release of the HoneyDrunk.Vault.Rotation.Abstractions package.
- `IRotator` contract for third-party provider secret rotation.
- `RotationContext`, `RotationResult`, and `RotationStatus` types, with a required non-empty `RotationContext.CorrelationId`.
