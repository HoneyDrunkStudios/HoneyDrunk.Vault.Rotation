using HoneyDrunk.Kernel.Abstractions;
using HoneyDrunk.Kernel.Abstractions.Identity;
using HoneyDrunk.Kernel.Hosting;
using HoneyDrunk.Vault.Extensions;
using HoneyDrunk.Vault.Providers.AzureKeyVault.Extensions;
using HoneyDrunk.Vault.Rotation.Providers.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GridEnvironments = HoneyDrunk.Kernel.Abstractions.Environments;

namespace HoneyDrunk.Vault.Rotation;

internal static class VaultRotationBootstrapper
{
    private const string KeyVaultUriSetting = "AZURE_KEYVAULT_URI";

    public static void Configure(IServiceCollection services, IConfiguration configuration, string version)
    {
        var honeyDrunkBuilder = services
            .AddHoneyDrunkNode(options =>
            {
                options.NodeId = ResolveNodeId(configuration);
                options.SectorId = Sectors.Core;
                options.EnvironmentId = ResolveEnvironment(configuration);
                options.Version = version;
                options.StudioId = configuration["Grid:StudioId"] ?? "honeydrunk";
                options.Tags["service"] = "vaultrot";
                options.Tags["adr"] = "ADR-0006";
            })
            .AddVault();

        if (Uri.TryCreate(configuration[KeyVaultUriSetting], UriKind.Absolute, out var vaultUri))
        {
            honeyDrunkBuilder.Services.AddVaultWithAzureKeyVault(options =>
            {
                options.VaultUri = vaultUri;
                options.UseManagedIdentity = true;
            });
        }

        honeyDrunkBuilder.Services
            .AddHoneyDrunkVaultRotators();
    }

    internal static NodeId ResolveNodeId(IConfiguration configuration)
    {
        var configured =
            configuration["HONEYDRUNK_NODE_ID"]
            ?? configuration["Grid:NodeId"];

        return string.IsNullOrWhiteSpace(configured)
            ? WellKnownNodes.Core.VaultRotation
            : new NodeId(configured);
    }

    internal static EnvironmentId ResolveEnvironment(IConfiguration configuration)
    {
        var configured =
            configuration["Grid:Environment"]
            ?? configuration["DOTNET_ENVIRONMENT"]
            ?? configuration["AZURE_FUNCTIONS_ENVIRONMENT"];

        return string.IsNullOrWhiteSpace(configured)
            ? GridEnvironments.Development
            : new EnvironmentId(configured.ToLowerInvariant());
    }
}
