using HoneyDrunk.Vault.Rotation.Abstractions;
using HoneyDrunk.Vault.Rotation.Providers.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace HoneyDrunk.Vault.Rotation.Tests;

/// <summary>
/// Tests for rotator discovery registration.
/// </summary>
public sealed class RotatorRegistrationTests
{
    /// <summary>
    /// Verifies that provider stubs are discoverable through dependency injection.
    /// </summary>
    [Fact]
    public void AddHoneyDrunkVaultRotatorsRegistersProviderStubs()
    {
        var services = new ServiceCollection();

        services.AddHoneyDrunkVaultRotators();

        using var provider = services.BuildServiceProvider();
        var providerNames = provider
            .GetServices<IRotator>()
            .Select(rotator => rotator.ProviderName)
            .Order()
            .ToArray();

        Assert.Equal(["openai", "resend", "twilio"], providerNames);
    }

    /// <summary>
    /// Verifies that every provider registration keeps the placeholder skipped behavior.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task ProviderStubsReturnSkippedPlaceholderResults()
    {
        var services = new ServiceCollection();
        services.AddHoneyDrunkVaultRotators();

        using var provider = services.BuildServiceProvider();
        var rotators = provider.GetServices<IRotator>().OrderBy(rotator => rotator.ProviderName).ToArray();
        var context = new RotationContext(
            "test-provider",
            "test-vault",
            "test-secret",
            DateTimeOffset.UtcNow,
            "test-correlation");

        var results = new List<RotationResult>();
        foreach (var rotator in rotators)
        {
            results.Add(await rotator.RotateAsync(context with { ProviderName = rotator.ProviderName }, CancellationToken.None));
        }

        Assert.Collection(
            results,
            result => AssertPlaceholderResult(result, "openai", "OpenAI rotation is not implemented in the scaffold."),
            result => AssertPlaceholderResult(result, "resend", "Resend rotation is not implemented in the scaffold."),
            result => AssertPlaceholderResult(result, "twilio", "Twilio rotation is not implemented in the scaffold."));
    }

    private static void AssertPlaceholderResult(RotationResult result, string providerName, string message)
    {
        Assert.Equal(providerName, result.ProviderName);
        Assert.Equal(RotationStatus.Skipped, result.Status);
        Assert.Equal(message, result.Message);
        Assert.NotNull(result.CompletedAtUtc);
    }
}
