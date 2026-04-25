namespace HoneyDrunk.Vault.Rotation.Abstractions;

/// <summary>
/// Describes the lifecycle status of a rotation attempt.
/// </summary>
public enum RotationStatus
{
    /// <summary>
    /// Rotation succeeded and a new secret version was written.
    /// </summary>
    Succeeded,

    /// <summary>
    /// Rotation failed.
    /// </summary>
    Failed,

    /// <summary>
    /// Rotation is not automated for this provider yet.
    /// </summary>
    Skipped,

    /// <summary>
    /// Rotation requires a manual runbook.
    /// </summary>
    ManualActionRequired,
}
