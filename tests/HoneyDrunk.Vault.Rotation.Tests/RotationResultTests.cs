using HoneyDrunk.Vault.Rotation.Abstractions;

namespace HoneyDrunk.Vault.Rotation.Tests;

/// <summary>
/// Tests for rotation result factory behavior.
/// </summary>
public sealed class RotationResultTests
{
    /// <summary>
    /// Verifies that skipped results carry provider, status, message, and completion time.
    /// </summary>
    [Fact]
    public void SkippedCreatesOperationalResult()
    {
        var before = DateTimeOffset.UtcNow;

        var result = RotationResult.Skipped("openai", "not implemented yet");

        Assert.Equal("openai", result.ProviderName);
        Assert.Equal(RotationStatus.Skipped, result.Status);
        Assert.Null(result.SecretName);
        Assert.Null(result.NewVersion);
        Assert.Equal("not implemented yet", result.Message);
        Assert.NotNull(result.CompletedAtUtc);
        Assert.True(result.CompletedAtUtc >= before);
        Assert.True(result.CompletedAtUtc <= DateTimeOffset.UtcNow);
    }
}
