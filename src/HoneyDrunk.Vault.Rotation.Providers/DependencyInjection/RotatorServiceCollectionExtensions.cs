using HoneyDrunk.Vault.Rotation.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HoneyDrunk.Vault.Rotation.Providers.DependencyInjection;

/// <summary>
/// Dependency injection extensions for third-party secret rotators.
/// </summary>
public static class RotatorServiceCollectionExtensions
{
    /// <summary>
    /// Discovers and registers all concrete <see cref="IRotator"/> implementations in the supplied assemblies.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">Optional assemblies to scan. Defaults to the providers assembly.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddHoneyDrunkVaultRotators(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);

        var scanAssemblies = assemblies.Length == 0
            ? [typeof(RotatorServiceCollectionExtensions).Assembly]
            : assemblies;

        foreach (var implementationType in scanAssemblies.SelectMany(DiscoverRotators))
        {
            services.AddTransient(typeof(IRotator), implementationType);
        }

        return services;
    }

    private static IEnumerable<Type> DiscoverRotators(Assembly assembly) =>
        assembly
            .DefinedTypes
            .Where(typeInfo =>
                !typeInfo.IsAbstract &&
                !typeInfo.IsInterface &&
                typeof(IRotator).IsAssignableFrom(typeInfo))
            .Select(typeInfo => typeInfo.AsType());
}
