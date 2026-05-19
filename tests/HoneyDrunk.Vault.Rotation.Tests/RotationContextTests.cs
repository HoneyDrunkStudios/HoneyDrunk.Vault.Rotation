using HoneyDrunk.Vault.Rotation.Abstractions;

namespace HoneyDrunk.Vault.Rotation.Tests;

/// <summary>
/// Tests for rotation context invariants.
/// </summary>
public sealed class RotationContextTests
{
    /// <summary>
    /// Verifies that the constructor preserves all validated rotation request fields.
    /// </summary>
    [Fact]
    public void ConstructorPreservesRotationRequestFields()
    {
        var requestedAt = new DateTimeOffset(2026, 5, 19, 12, 0, 0, TimeSpan.Zero);
        var metadata = new Dictionary<string, string>
        {
            ["rotation-mode"] = "placeholder",
        };

        var context = new RotationContext(
            "openai",
            "target-vault",
            "api-key",
            requestedAt,
            "correlation-123",
            metadata);

        Assert.Equal("openai", context.ProviderName);
        Assert.Equal("target-vault", context.TargetVaultName);
        Assert.Equal("api-key", context.SecretName);
        Assert.Equal(requestedAt, context.RequestedAtUtc);
        Assert.Equal("correlation-123", context.CorrelationId);
        Assert.NotNull(context.Metadata);
        Assert.Equal("placeholder", context.Metadata["rotation-mode"]);
    }

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

    /// <summary>
    /// Verifies that immutable-copy initialization cannot bypass validated identity fields.
    /// </summary>
    /// <param name="fieldName">The field to mutate.</param>
    /// <param name="invalidValue">The invalid value.</param>
    [Theory]
    [InlineData(nameof(RotationContext.ProviderName), null)]
    [InlineData(nameof(RotationContext.ProviderName), "")]
    [InlineData(nameof(RotationContext.TargetVaultName), "   ")]
    [InlineData(nameof(RotationContext.SecretName), "")]
    [InlineData(nameof(RotationContext.CorrelationId), "   ")]
    public void WithExpressionRejectsMissingRequiredFields(string fieldName, string? invalidValue)
    {
        var context = new RotationContext(
            "openai",
            "test-vault",
            "test-secret",
            DateTimeOffset.UtcNow,
            "test-correlation");

        Action act = fieldName switch
        {
            nameof(RotationContext.ProviderName) => () => _ = context with { ProviderName = invalidValue! },
            nameof(RotationContext.TargetVaultName) => () => _ = context with { TargetVaultName = invalidValue! },
            nameof(RotationContext.SecretName) => () => _ = context with { SecretName = invalidValue! },
            nameof(RotationContext.CorrelationId) => () => _ = context with { CorrelationId = invalidValue! },
            _ => throw new InvalidOperationException($"Unknown field '{fieldName}'."),
        };

        Assert.ThrowsAny<ArgumentException>(act);
    }
}
