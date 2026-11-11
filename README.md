# HoneyDrunk.Vault.Rotation

Azure Function App sub-Node for ADR-0006 Tier 2 third-party secret rotation.

HoneyDrunk.Vault.Rotation owns scheduled rotation orchestration for provider secrets that Azure Key Vault cannot rotate natively, such as Resend, Twilio, and OpenAI. Provider-specific rotators are intentionally stubbed in this scaffold and will land as follow-up packets.

## Grid Placement

- **Node:** `honeydrunk-vault-rotation` via `WellKnownNodes.Core.VaultRotation` unless `HONEYDRUNK_NODE_ID` / `Grid:NodeId` overrides it
- **Service token:** `vaultrot`
- **Tier:** ADR-0006 Tier 2, third-party rotation within 90 days
- **Vault:** `kv-hd-vaultrot-{env}`
- **Managed identity:** system-assigned Function App identity
- **RBAC:** `Key Vault Secrets Officer` on each downstream target vault, granted per vault

## Invariants

- `vaultrot` is 8 characters, which satisfies the 13-character service-name budget from ADR-0005.
- The Function App uses OIDC for CI deployment authentication. Service-principal client secrets are not used.
- Every scheduled rotation execution establishes a Kernel Grid/Operation context before provider work begins.
- Internal system rotation work uses Kernel internal tenant context by default.
- Rotation code must write new secret versions and consumers must continue resolving latest versions through `ISecretStore`.
- This Node does not duplicate Azure-native Key Vault rotation.

## Local Development

```powershell
dotnet restore HoneyDrunk.Vault.Rotation.slnx
dotnet build HoneyDrunk.Vault.Rotation.slnx -c Release
dotnet test HoneyDrunk.Vault.Rotation.slnx -c Release --no-build
```

To run the Function host locally:

```powershell
cd src/HoneyDrunk.Vault.Rotation
func start
```

When `AZURE_KEYVAULT_URI` is set, the host wires HoneyDrunk.Vault to Azure Key Vault with Managed Identity. Without it, the scaffold starts with the Vault core services only.

## References

- ADR-0006: Secret Rotation and Lifecycle
- ADR-0005: Azure resource naming and per-Node vault invariants

## .NET SDK

Build with the .NET 11 SDK pinned in `global.json` (11.0.100, stable patch roll-forward). Runtime and test projects target `net11.0`; Abstractions retain `net10.0` alongside `net11.0` and public contract shapes are unchanged.

## .NET 11 Functions migration

The Functions host uses `Azure.Functions.Sdk/1.0.1` with the existing explicit Worker 2.52.0 and trigger/binding references. The new SDK owns the executable output type, Functions v4 setting, metadata generation, and worker indexing; do not re-add the former Worker.Sdk package.

After a solution restore, explicitly restore the host project before a no-restore build if the SDK reports AZFW0108. Do not edit or build its generated `obj/azure_functions/azure_functions.g.csproj` directly.

Before deployment, independently verify that the Azure app uses the .NET 11 isolated-worker stack and a supported hosting plan. Linux Consumption does not support .NET 10 or later. Local unit tests do not verify cloud configuration or live providers. See the [isolated worker guide](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide).
