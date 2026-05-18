namespace HoneyDrunk.Vault.Rotation.Abstractions;

/// <summary>
/// Describes a third-party secret rotation request.
/// </summary>
public sealed record RotationContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RotationContext"/> class.
    /// </summary>
    /// <param name="providerName">The provider to rotate.</param>
    /// <param name="targetVaultName">The destination Key Vault name.</param>
    /// <param name="secretName">The destination secret name.</param>
    /// <param name="requestedAtUtc">The UTC time the rotation was requested.</param>
    /// <param name="correlationId">A non-empty cross-system correlation identifier.</param>
    /// <param name="metadata">Additional provider-specific request metadata.</param>
    public RotationContext(
        string providerName,
        string targetVaultName,
        string secretName,
        DateTimeOffset requestedAtUtc,
        string correlationId,
        IReadOnlyDictionary<string, string>? metadata = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(providerName);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetVaultName);
        ArgumentException.ThrowIfNullOrWhiteSpace(secretName);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        ProviderName = providerName;
        TargetVaultName = targetVaultName;
        SecretName = secretName;
        RequestedAtUtc = requestedAtUtc;
        CorrelationId = correlationId;
        Metadata = metadata;
    }

    /// <summary>
    /// Gets the provider to rotate.
    /// </summary>
    public string ProviderName { get; init; }

    /// <summary>
    /// Gets the destination Key Vault name.
    /// </summary>
    public string TargetVaultName { get; init; }

    /// <summary>
    /// Gets the destination secret name.
    /// </summary>
    public string SecretName { get; init; }

    /// <summary>
    /// Gets the UTC time the rotation was requested.
    /// </summary>
    public DateTimeOffset RequestedAtUtc { get; init; }

    /// <summary>
    /// Gets the non-empty cross-system correlation identifier.
    /// </summary>
    public string CorrelationId { get; init; }

    /// <summary>
    /// Gets additional provider-specific request metadata.
    /// </summary>
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}
