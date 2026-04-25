using HoneyDrunk.Vault.Rotation.Abstractions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace HoneyDrunk.Vault.Rotation;

/// <summary>
/// Scheduled entry point for third-party secret rotation.
/// </summary>
public sealed class RotateThirdPartySecretsFunction(
    IEnumerable<IRotator> rotators,
    ILogger<RotateThirdPartySecretsFunction> logger)
{
    /// <summary>
    /// Runs the scheduled rotation placeholder.
    /// </summary>
    /// <param name="timer">Timer metadata supplied by the Functions runtime.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A completed task.</returns>
    [Function(nameof(RotateThirdPartySecretsFunction))]
    public Task Run(
        [TimerTrigger("%RotationSchedule%")] TimerInfo timer,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogInformation(
            "Third-party secret rotation is not yet implemented. Discovered {RotatorCount} rotator stubs.",
            rotators.Count());

        return Task.CompletedTask;
    }
}
