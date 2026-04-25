using HoneyDrunk.Vault.Rotation.Abstractions;

namespace HoneyDrunk.Vault.Rotation.Providers.Twilio;

/// <summary>
/// Placeholder rotator for Twilio credentials.
/// </summary>
public sealed class TwilioRotator : IRotator
{
    /// <inheritdoc />
    public string ProviderName => "twilio";

    /// <inheritdoc />
    public Task<RotationResult> RotateAsync(RotationContext ctx, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(ctx);
        ct.ThrowIfCancellationRequested();

        return Task.FromResult(RotationResult.Skipped(
            ProviderName,
            "Twilio rotation is not implemented in the scaffold."));
    }
}
