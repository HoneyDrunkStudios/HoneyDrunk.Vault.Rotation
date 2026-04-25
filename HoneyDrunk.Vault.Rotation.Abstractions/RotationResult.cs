namespace HoneyDrunk.Vault.Rotation.Abstractions;

/// <summary>
/// Represents the outcome of a third-party secret rotation attempt.
/// </summary>
/// <param name="ProviderName">The provider that handled the request.</param>
/// <param name="Status">The outcome status.</param>
/// <param name="SecretName">The target secret name, when known.</param>
/// <param name="NewVersion">The new Key Vault secret version, when a version was written.</param>
/// <param name="Message">A human-readable operational message.</param>
/// <param name="CompletedAtUtc">The UTC completion time.</param>
public sealed record RotationResult(
    string ProviderName,
    RotationStatus Status,
    string? SecretName = null,
    string? NewVersion = null,
    string? Message = null,
    DateTimeOffset? CompletedAtUtc = null)
{
    /// <summary>
    /// Creates a skipped result for scaffolding and providers that do not yet rotate automatically.
    /// </summary>
    /// <param name="providerName">The provider name.</param>
    /// <param name="message">The operational message.</param>
    /// <returns>A skipped rotation result.</returns>
    public static RotationResult Skipped(string providerName, string message) =>
        new(providerName, RotationStatus.Skipped, Message: message, CompletedAtUtc: DateTimeOffset.UtcNow);
}
