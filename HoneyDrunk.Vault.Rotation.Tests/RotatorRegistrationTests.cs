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
}
