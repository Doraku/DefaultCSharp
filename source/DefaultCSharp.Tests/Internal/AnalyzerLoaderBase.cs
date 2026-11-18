using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DefaultCSharp.Internal;

internal class AnalyzerLoaderBase
{
    private static readonly Lazy<string> _baseDirectory = new(GetBaseDirectory, isThreadSafe: true);

    private static string GetBaseDirectory()
    {
        return Path.GetDirectoryName(typeof(AnalyzerLoaderBase).Assembly.Location) ?? Environment.CurrentDirectory;
    }

    protected static Assembly LoadAssembly(params ReadOnlySpan<string> paths) => AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine([_baseDirectory.Value, .. paths]));

    protected static Lazy<DiagnosticAnalyzer> CreateDiagnosticAnalyzer(Lazy<Assembly> assembly, string diagnosticAnalyzerTypeName)
    {
        Type diagnosticAnalyzerType = assembly.Value.GetType(diagnosticAnalyzerTypeName) ?? throw new InvalidOperationException($"Could not locate type '{diagnosticAnalyzerTypeName}' from '{assembly.Value.GetName().Name}'");

        return new Lazy<DiagnosticAnalyzer>(
            () => Activator.CreateInstance(diagnosticAnalyzerType) as DiagnosticAnalyzer ?? throw new InvalidOperationException($"Could not create instance of '{diagnosticAnalyzerType.FullName}'"),
            true);
    }
}
