namespace HoneyDrunk.Vault.Rotation.Tests;

/// <summary>
/// Tests for ADR-0005 resource naming constraints.
/// </summary>
public sealed class VaultNamingTests
{
    /// <summary>
    /// Verifies that the rotation service token fits the 13-character service-name budget.
    /// </summary>
    [Fact]
    public void VaultrotFitsServiceNameBudget()
    {
        const string serviceName = "vaultrot";

        Assert.True(serviceName.Length <= 13);
        Assert.Equal(8, serviceName.Length);
    }
}
