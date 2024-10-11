using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DefaultCSharp.Internal;

internal class AnalyzerLoaderBase
{
    private static readonly Lazy<string> _nuGetPackagesFolder = new(GetNuGetPackagesFolder, isThreadSafe: true);

    private static string GetNuGetPackagesFolder()
    {
        string? result = Environment.GetEnvironmentVariable("NUGET_PACKAGES");

        if (result is null)
        {
            string? homeFolder =
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

    protected static Assembly LoadAssembly(params string[] paths) => AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine([_nuGetPackagesFolder.Value, .. paths]));

    protected static Lazy<DiagnosticAnalyzer> CreateDiagnosticAnalyzer(Lazy<Assembly> assembly, string diagnosticAnalyzerTypeName)
    {
        Type diagnosticAnalyzerType = assembly.Value.GetType(diagnosticAnalyzerTypeName) ?? throw new InvalidOperationException($"Could not locate type '{diagnosticAnalyzerTypeName}' from '{assembly.Value.GetName().Name}'");

        return new Lazy<DiagnosticAnalyzer>(
            () => Activator.CreateInstance(diagnosticAnalyzerType) as DiagnosticAnalyzer ?? throw new InvalidOperationException($"Could not create instance of '{diagnosticAnalyzerType.FullName}'"),
            true);
    }
}
