using HoneyDrunk.Vault.Rotation.Abstractions;

namespace HoneyDrunk.Vault.Rotation.Providers;

/// <summary>
/// Base scaffold for providers whose automatic rotation behavior has not landed yet.
/// </summary>
/// <remarks>
/// This class intentionally returns <see cref="RotationStatus.Skipped"/> so provider registrations are observable
/// without implying real third-party credential rotation.
/// </remarks>
/// <param name="providerName">The canonical provider name.</param>
/// <param name="displayName">The human-readable provider name used in skipped messages.</param>
public abstract class PlaceholderRotator(string providerName, string displayName) : IRotator
{
    private readonly string _displayName = string.IsNullOrWhiteSpace(displayName)
        ? throw new ArgumentException("Display name cannot be null or whitespace.", nameof(displayName))
        : displayName;

    /// <inheritdoc />
    public string ProviderName { get; } = string.IsNullOrWhiteSpace(providerName)
        ? throw new ArgumentException("Provider name cannot be null or whitespace.", nameof(providerName))
        : providerName;

    /// <inheritdoc />
    public Task<RotationResult> RotateAsync(RotationContext ctx, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(ctx);
        ct.ThrowIfCancellationRequested();

        return Task.FromResult(RotationResult.Skipped(
            ProviderName,
            $"{_displayName} rotation is not implemented in the scaffold."));
    }
}
