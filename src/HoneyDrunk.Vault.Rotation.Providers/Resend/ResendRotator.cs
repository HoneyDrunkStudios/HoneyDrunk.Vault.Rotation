using HoneyDrunk.Vault.Rotation.Abstractions;

namespace HoneyDrunk.Vault.Rotation.Providers.Resend;

/// <summary>
/// Placeholder rotator for Resend API keys.
/// </summary>
public sealed class ResendRotator : IRotator
{
    /// <inheritdoc />
    public string ProviderName => "resend";

    /// <inheritdoc />
    public Task<RotationResult> RotateAsync(RotationContext ctx, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(ctx);
        ct.ThrowIfCancellationRequested();

        return Task.FromResult(RotationResult.Skipped(
            ProviderName,
            "Resend rotation is not implemented in the scaffold."));
    }
}
