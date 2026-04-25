namespace HoneyDrunk.Vault.Rotation.Abstractions;

/// <summary>
/// Describes a third-party secret rotation request.
/// </summary>
/// <param name="ProviderName">The provider to rotate.</param>
/// <param name="TargetVaultName">The destination Key Vault name.</param>
/// <param name="SecretName">The destination secret name.</param>
/// <param name="RequestedAtUtc">The UTC time the rotation was requested.</param>
/// <param name="CorrelationId">An optional cross-system correlation identifier.</param>
/// <param name="Metadata">Additional provider-specific request metadata.</param>
public sealed record RotationContext(
    string ProviderName,
    string TargetVaultName,
    string SecretName,
    DateTimeOffset RequestedAtUtc,
    string? CorrelationId = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
