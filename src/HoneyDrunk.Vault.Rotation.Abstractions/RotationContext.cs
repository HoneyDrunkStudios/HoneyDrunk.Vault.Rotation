namespace HoneyDrunk.Vault.Rotation.Abstractions;

/// <summary>
/// Describes a third-party secret rotation request.
/// </summary>
public sealed record RotationContext
{
    private string _providerName = string.Empty;
    private string _targetVaultName = string.Empty;
    private string _secretName = string.Empty;
    private string _correlationId = string.Empty;

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
    public string ProviderName
    {
        get => _providerName;
        init
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _providerName = value;
        }
    }

    /// <summary>
    /// Gets the destination Key Vault name.
    /// </summary>
    public string TargetVaultName
    {
        get => _targetVaultName;
        init
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _targetVaultName = value;
        }
    }

    /// <summary>
    /// Gets the destination secret name.
    /// </summary>
    public string SecretName
    {
        get => _secretName;
        init
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _secretName = value;
        }
    }

    /// <summary>
    /// Gets the UTC time the rotation was requested.
    /// </summary>
    public DateTimeOffset RequestedAtUtc { get; init; }

    /// <summary>
    /// Gets the non-empty cross-system correlation identifier.
    /// </summary>
    public string CorrelationId
    {
        get => _correlationId;
        init
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _correlationId = value;
        }
    }

    /// <summary>
    /// Gets additional provider-specific request metadata.
    /// </summary>
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}
