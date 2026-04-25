namespace HoneyDrunk.Vault.Rotation.Canary;

/// <summary>
/// Placeholder canary suite for future deployment-boundary assertions.
/// </summary>
public sealed class CanaryPlaceholder
{
    /// <summary>
    /// Keeps the canary project active until real canary checks land.
    /// </summary>
    [Fact]
    public void CanaryProjectIsPresent()
    {
        Assert.True(true);
    }
}
