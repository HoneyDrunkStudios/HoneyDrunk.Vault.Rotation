# HoneyDrunk.Vault.Rotation

Azure Function App host for Tier 2 third-party secret rotation in the HoneyDrunk Grid.

This project is the deployable Functions host (`IsPackable=false`), not a NuGet
library. It wires the rotation abstractions and providers into a Functions
worker via `VaultRotationBootstrapper` and runs the timer-triggered
`RotateThirdPartySecretsFunction`. The reusable contracts and provider
implementations ship as the `HoneyDrunk.Vault.Rotation.Abstractions` and
`HoneyDrunk.Vault.Rotation.Providers` packages.

## Run locally

```bash
func start --csharp
```

Configuration is supplied through `local.settings.json` (local) or app settings
(Azure). The host authenticates to Key Vault with `DefaultAzureCredential` and
resolves rotation targets through the providers package.
