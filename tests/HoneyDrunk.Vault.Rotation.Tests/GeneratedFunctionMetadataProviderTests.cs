using System.Collections;
using System.Reflection;

namespace HoneyDrunk.Vault.Rotation.Tests;

/// <summary>
/// Tests for Azure Functions worker generated metadata.
/// </summary>
public sealed class GeneratedFunctionMetadataProviderTests
{
    /// <summary>
    /// Verifies that generated worker metadata includes the rotation timer function.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task GetFunctionMetadataAsyncIncludesRotationFunction()
    {
        var providerType = typeof(RotateThirdPartySecretsFunction).Assembly.GetType(
            "HoneyDrunk.Vault.Rotation.GeneratedFunctionMetadataProvider",
            throwOnError: true)!;
        var provider = Activator.CreateInstance(providerType)!;
        var method = providerType.GetMethod("GetFunctionMetadataAsync", BindingFlags.Instance | BindingFlags.Public)!;

        var task = (Task)method.Invoke(provider, [AppContext.BaseDirectory])!;
        await task;

        var result = (IEnumerable)task.GetType().GetProperty("Result")!.GetValue(task)!;
        var names = result
            .Cast<object>()
            .Select(metadata => metadata.GetType().GetProperty("Name")!.GetValue(metadata))
            .OfType<string>()
            .ToArray();

        Assert.Contains("RotateThirdPartySecretsFunction", names);
    }
}
