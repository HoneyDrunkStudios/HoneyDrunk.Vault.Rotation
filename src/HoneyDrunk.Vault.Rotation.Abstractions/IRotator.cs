namespace HoneyDrunk.Vault.Rotation.Abstractions;

/// <summary>
/// Rotates a third-party provider secret and writes the new version to the configured target vault.
/// </summary>
public interface IRotator
{
    /// <summary>
    /// Gets the canonical provider name handled by this rotator.
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Rotates the provider secret.
    /// </summary>
    /// <param name="ctx">The rotation request context.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>The rotation result.</returns>
    Task<RotationResult> RotateAsync(RotationContext ctx, CancellationToken ct);
}
