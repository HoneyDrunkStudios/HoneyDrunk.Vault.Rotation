using HoneyDrunk.Kernel.Abstractions.Context;
using HoneyDrunk.Kernel.Context;
using HoneyDrunk.Kernel.Context.Mappers;
using HoneyDrunk.Vault.Rotation.Abstractions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HoneyDrunk.Vault.Rotation;

/// <summary>
/// Scheduled entry point for third-party secret rotation.
/// </summary>
public sealed class RotateThirdPartySecretsFunction(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<RotateThirdPartySecretsFunction> logger)
{
    private const string OperationName = "RotateThirdPartySecrets";
    private const string PlaceholderTargetVaultName = "pending-target-vault";

    /// <summary>
    /// Runs the scheduled rotation placeholder.
    /// </summary>
    /// <param name="timer">Timer metadata supplied by the Functions runtime.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A completed task.</returns>
    [Function(nameof(RotateThirdPartySecretsFunction))]
    public async Task Run(
        [TimerTrigger("%RotationSchedule%")] TimerInfo timer,
        CancellationToken cancellationToken)
    {
        _ = timer;
        cancellationToken.ThrowIfCancellationRequested();

        using var scope = serviceScopeFactory.CreateScope();
        var services = scope.ServiceProvider;
        var gridContext = services.GetRequiredService<GridContext>();
        var operationContextFactory = services.GetRequiredService<IOperationContextFactory>();
        var operationContextAccessor = services.GetRequiredService<IOperationContextAccessor>();

        JobContextMapper.InitializeForScheduledJob(
            gridContext,
            OperationName,
            DateTimeOffset.UtcNow,
            cancellationToken);

        using var operationContext = operationContextFactory.Create(
            OperationName,
            new Dictionary<string, object?>
            {
                ["trigger"] = "timer",
            });

        try
        {
            var rotators = services.GetServices<IRotator>().ToArray();

            logger.LogInformation(
                "Starting third-party secret rotation scaffold with {RotatorCount} rotator stubs and CorrelationId {CorrelationId}.",
                rotators.Length,
                operationContext.CorrelationId);

            foreach (var rotator in rotators)
            {
                var context = CreateRotationContext(rotator.ProviderName, operationContext.CorrelationId);
                var result = await rotator.RotateAsync(context, cancellationToken).ConfigureAwait(false);

                logger.LogInformation(
                    "Rotation provider {ProviderName} completed with status {RotationStatus} and CorrelationId {CorrelationId}.",
                    result.ProviderName,
                    result.Status,
                    context.CorrelationId);
            }

            operationContext.Complete();
        }
        catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            operationContext.Fail("Third-party secret rotation scaffold was canceled.", ex);
            throw;
        }
        catch (Exception ex)
        {
            operationContext.Fail("Third-party secret rotation scaffold failed.", ex);
            throw;
        }
        finally
        {
            operationContextAccessor.Current = null;
        }
    }

    private static RotationContext CreateRotationContext(string providerName, string correlationId) =>
        new(
            providerName,
            PlaceholderTargetVaultName,
            $"{providerName}-secret-placeholder",
            DateTimeOffset.UtcNow,
            correlationId,
            new Dictionary<string, string>
            {
                ["rotation-mode"] = "placeholder",
            });
}
