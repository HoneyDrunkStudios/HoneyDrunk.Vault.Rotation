using HoneyDrunk.Vault.Rotation.Abstractions;

namespace HoneyDrunk.Vault.Rotation.Tests;

/// <summary>
/// Tests for rotation context invariants.
/// </summary>
public sealed class RotationContextTests
{
    /// <summary>
    /// Verifies that rotation contexts require a populated correlation identifier.
    /// </summary>
    /// <param name="correlationId">The invalid correlation identifier.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ConstructorRejectsMissingCorrelationId(string? correlationId)
    {
        var act = () => new RotationContext(
            "openai",
            "test-vault",
            "test-secret",
            DateTimeOffset.UtcNow,
            correlationId!);

        Assert.ThrowsAny<ArgumentException>(act);
    }
}
