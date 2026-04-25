using HoneyDrunk.Vault.Rotation.Abstractions;

namespace HoneyDrunk.Vault.Rotation.Providers.OpenAI;

/// <summary>
/// Placeholder rotator for OpenAI API keys.
/// </summary>
public sealed class OpenAIRotator : IRotator
{
    /// <inheritdoc />
    public string ProviderName => "openai";

    /// <inheritdoc />
    public Task<RotationResult> RotateAsync(RotationContext ctx, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(ctx);
        ct.ThrowIfCancellationRequested();

        return Task.FromResult(RotationResult.Skipped(
            ProviderName,
            "OpenAI rotation is not implemented in the scaffold."));
    }
}
