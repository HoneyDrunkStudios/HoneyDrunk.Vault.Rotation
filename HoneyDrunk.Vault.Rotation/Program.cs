using HoneyDrunk.Kernel.Abstractions;
using HoneyDrunk.Kernel.Abstractions.Identity;
using HoneyDrunk.Kernel.Hosting;
using HoneyDrunk.Vault.Extensions;
using HoneyDrunk.Vault.Providers.AzureKeyVault.Extensions;
using HoneyDrunk.Vault.Rotation.Providers.DependencyInjection;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using GridEnvironments = HoneyDrunk.Kernel.Abstractions.Environments;

const string KeyVaultUriSetting = "AZURE_KEYVAULT_URI";

var builder = FunctionsApplication.CreateBuilder(args);

var honeyDrunkBuilder = builder.Services
    .AddHoneyDrunkNode(options =>
    {
        options.NodeId = new NodeId("honeydrunk-vault-rotation");
        options.SectorId = Sectors.Core;
        options.EnvironmentId = ResolveEnvironment(builder.Configuration);
        options.Version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "0.1.0";
        options.StudioId = builder.Configuration["Grid:StudioId"] ?? "honeydrunk";
        options.Tags["service"] = "vaultrot";
        options.Tags["adr"] = "ADR-0006";
    })
    .AddVault();

if (Uri.TryCreate(builder.Configuration[KeyVaultUriSetting], UriKind.Absolute, out var vaultUri))
{
    honeyDrunkBuilder.Services.AddVaultWithAzureKeyVault(options =>
    {
        options.VaultUri = vaultUri;
        options.UseManagedIdentity = true;
    });
}

honeyDrunkBuilder.Services
    .AddHoneyDrunkVaultRotators();

builder.Build().Run();

static EnvironmentId ResolveEnvironment(IConfiguration configuration)
{
    var configured =
        configuration["Grid:Environment"]
        ?? configuration["DOTNET_ENVIRONMENT"]
        ?? configuration["AZURE_FUNCTIONS_ENVIRONMENT"];

    return string.IsNullOrWhiteSpace(configured)
        ? GridEnvironments.Development
        : new EnvironmentId(configured.ToLowerInvariant());
}
