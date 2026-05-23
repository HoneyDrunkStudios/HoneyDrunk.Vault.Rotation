using HoneyDrunk.Vault.Rotation;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

VaultRotationBootstrapper.Configure(
    builder.Services,
    builder.Configuration,
    typeof(Program).Assembly.GetName().Version?.ToString() ?? "0.1.0");

builder.Build().Run();
