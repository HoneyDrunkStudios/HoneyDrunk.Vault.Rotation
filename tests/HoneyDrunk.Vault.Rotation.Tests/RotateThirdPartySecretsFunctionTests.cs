using HoneyDrunk.Kernel.Abstractions;
using HoneyDrunk.Kernel.Abstractions.Context;
using HoneyDrunk.Kernel.Abstractions.Identity;
using HoneyDrunk.Kernel.Hosting;
using HoneyDrunk.Vault.Rotation.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using GridEnvironments = HoneyDrunk.Kernel.Abstractions.Environments;

namespace HoneyDrunk.Vault.Rotation.Tests;

/// <summary>
/// Tests for the timer-triggered rotation function.
/// </summary>
public sealed class RotateThirdPartySecretsFunctionTests
{
    /// <summary>
    /// Verifies that timer execution creates Kernel context before invoking rotators.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task RunCreatesKernelOperationContextForTimerExecution()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services
            .AddHoneyDrunkNode(options =>
            {
                options.NodeId = WellKnownNodes.Core.VaultRotation;
                options.SectorId = Sectors.Core;
                options.EnvironmentId = GridEnvironments.Development;
                options.StudioId = "honeydrunk";
                options.Version = "0.1.0";
            })
            .Services
            .AddSingleton<CapturingRotator>()
            .AddSingleton<IRotator>(sp => sp.GetRequiredService<CapturingRotator>());

        using var provider = services.BuildServiceProvider();
        var function = new RotateThirdPartySecretsFunction(
            provider.GetRequiredService<IServiceScopeFactory>(),
            NullLogger<RotateThirdPartySecretsFunction>.Instance);

        await function.Run(null!, CancellationToken.None);

        var rotator = provider.GetRequiredService<CapturingRotator>();
        Assert.NotNull(rotator.Context);
        Assert.False(string.IsNullOrWhiteSpace(rotator.Context.CorrelationId));
        Assert.Equal(TenantId.Internal, rotator.TenantId);
        Assert.Equal(WellKnownNodes.Core.VaultRotation.Value, rotator.NodeId);
        Assert.Null(provider.GetRequiredService<IOperationContextAccessor>().Current);
    }

    /// <summary>
    /// Test rotator that captures the active rotation and Kernel operation context.
    /// </summary>
    /// <param name="operationContextAccessor">The operation context accessor.</param>
    public sealed class CapturingRotator(IOperationContextAccessor operationContextAccessor) : IRotator
    {
        /// <summary>
        /// Gets the captured rotation context.
        /// </summary>
        public RotationContext? Context { get; private set; }

        /// <summary>
        /// Gets the captured tenant identifier.
        /// </summary>
        public TenantId? TenantId { get; private set; }

        /// <summary>
        /// Gets the captured Node identifier.
        /// </summary>
        public string? NodeId { get; private set; }

        /// <inheritdoc />
        public string ProviderName => "openai";

        /// <inheritdoc />
        public Task<RotationResult> RotateAsync(RotationContext ctx, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(ctx);
            ct.ThrowIfCancellationRequested();

            var operationContext = operationContextAccessor.Current
                ?? throw new InvalidOperationException("Expected an active operation context.");

            Context = ctx;
            TenantId = operationContext.TenantId;
            NodeId = operationContext.GridContext.NodeId;
            return Task.FromResult(RotationResult.Skipped(ProviderName, "captured"));
        }
    }
}
