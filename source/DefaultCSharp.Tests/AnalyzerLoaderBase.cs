using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace DefaultCSharp;

public class AnalyzerLoaderBase
{
    private static readonly Lazy<string> _nuGetPackagesFolder = new(GetNuGetPackagesFolder, isThreadSafe: true);

    protected static string NuGetPackagesFolder => _nuGetPackagesFolder.Value;

    protected static Type FindType(
        Lazy<Assembly> assembly,
        string typeName) =>
            assembly.Value.GetType(typeName) ?? throw new InvalidOperationException($"Could not locate type '{typeName}' from '{assembly.Value.GetName().Name}'");

    private static string GetNuGetPackagesFolder()
    {
        string result = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
        if (result is null)
        {
            string homeFolder =
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? Environment.GetEnvironmentVariable("USERPROFILE")
                    : Environment.GetEnvironmentVariable("HOME");

            result = Path.Combine(homeFolder ?? throw new InvalidOperationException("Could not determine home folder"), ".nuget", "packages");
        }

        if (!Directory.Exists(result))
        {
            throw new InvalidOperationException($"NuGet package cache folder '{result}' does not exist");
        }

        return result;
    }

    protected static Assembly LoadAssembly(string assemblyPath) => AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
}
