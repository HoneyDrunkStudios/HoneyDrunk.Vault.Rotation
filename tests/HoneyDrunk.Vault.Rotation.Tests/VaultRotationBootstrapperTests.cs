using HoneyDrunk.Kernel.Abstractions;
using HoneyDrunk.Kernel.Abstractions.Identity;
using HoneyDrunk.Vault.Rotation.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GridEnvironments = HoneyDrunk.Kernel.Abstractions.Environments;

namespace HoneyDrunk.Vault.Rotation.Tests;

/// <summary>
/// Tests for runtime bootstrap configuration.
/// </summary>
public sealed class VaultRotationBootstrapperTests
{
    /// <summary>
    /// Verifies that runtime bootstrap wires Kernel, Vault, and provider rotators.
    /// </summary>
    [Fact]
    public void ConfigureRegistersRuntimeServicesAndRotators()
    {
        var services = new ServiceCollection();
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["Grid:StudioId"] = "studio-test",
            ["Grid:NodeId"] = "custom-vaultrot",
            ["Grid:Environment"] = "Staging",
        });

        VaultRotationBootstrapper.Configure(services, configuration, "0.1.0-test");

        using var provider = services.BuildServiceProvider();
        var rotators = provider.GetServices<IRotator>().Select(rotator => rotator.ProviderName).Order().ToArray();

        Assert.Equal(["openai", "resend", "twilio"], rotators);
    }

    /// <summary>
    /// Verifies that runtime bootstrap wires Azure Key Vault services when a vault URI is configured.
    /// </summary>
    [Fact]
    public void ConfigureRegistersAzureKeyVaultWhenVaultUriIsConfigured()
    {
        var services = new ServiceCollection();
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["AZURE_KEYVAULT_URI"] = "https://vaultrot-test.vault.azure.net/",
        });

        VaultRotationBootstrapper.Configure(services, configuration, "0.1.0-test");

        using var provider = services.BuildServiceProvider();
        var rotators = provider.GetServices<IRotator>().Select(rotator => rotator.ProviderName).Order().ToArray();

        Assert.Equal(["openai", "resend", "twilio"], rotators);
    }

    /// <summary>
    /// Verifies NodeId resolution precedence and defaults.
    /// </summary>
    /// <param name="environmentNodeId">The environment variable node identifier.</param>
    /// <param name="gridNodeId">The Grid configuration node identifier.</param>
    /// <param name="expected">The expected resolved node identifier, or null for the default.</param>
    [Theory]
    [InlineData(null, null, null)]
    [InlineData("env-node", "grid-node", "env-node")]
    [InlineData(null, "grid-node", "grid-node")]
    [InlineData("   ", null, null)]
    public void ResolveNodeIdUsesEnvironmentThenGridThenDefault(string? environmentNodeId, string? gridNodeId, string? expected)
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["HONEYDRUNK_NODE_ID"] = environmentNodeId,
            ["Grid:NodeId"] = gridNodeId,
        });

        var nodeId = VaultRotationBootstrapper.ResolveNodeId(configuration);

        Assert.Equal(expected is null ? WellKnownNodes.Core.VaultRotation : new NodeId(expected), nodeId);
    }

    /// <summary>
    /// Verifies environment resolution precedence, normalization, and defaults.
    /// </summary>
    /// <param name="gridEnvironment">The Grid environment setting.</param>
    /// <param name="dotnetEnvironment">The DOTNET_ENVIRONMENT setting.</param>
    /// <param name="functionsEnvironment">The Azure Functions environment setting.</param>
    /// <param name="expected">The expected resolved environment, or null for the default.</param>
    [Theory]
    [InlineData(null, null, null, null)]
    [InlineData("Production", "Staging", "Development", "production")]
    [InlineData(null, "Staging", "Development", "staging")]
    [InlineData(null, null, "Production", "production")]
    [InlineData("   ", null, null, null)]
    public void ResolveEnvironmentUsesGridThenDotnetThenFunctionsThenDefault(
        string? gridEnvironment,
        string? dotnetEnvironment,
        string? functionsEnvironment,
        string? expected)
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["Grid:Environment"] = gridEnvironment,
            ["DOTNET_ENVIRONMENT"] = dotnetEnvironment,
            ["AZURE_FUNCTIONS_ENVIRONMENT"] = functionsEnvironment,
        });

        var environmentId = VaultRotationBootstrapper.ResolveEnvironment(configuration);

        Assert.Equal(expected is null ? GridEnvironments.Development : new EnvironmentId(expected), environmentId);
    }

    private static IConfiguration BuildConfiguration(IReadOnlyDictionary<string, string?> values) =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
}
