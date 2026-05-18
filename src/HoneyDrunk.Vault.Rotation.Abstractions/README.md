# HoneyDrunk.Vault.Rotation.Abstractions

Contracts for third-party secret rotation in the HoneyDrunk Grid.

`RotationContext.CorrelationId` is required and must be non-empty so every rotation attempt can be tied back to the Kernel operation context that scheduled it.
