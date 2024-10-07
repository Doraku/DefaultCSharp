using System;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DefaultCSharp;

public class MicrosoftCodeAnalysisCSharpCodeStyleAnalyzers : AnalyzerLoaderBase
{
    private static readonly Lazy<Assembly> _assemblyCSharpNetAnalyzers = new(LoadCSharpNetAnalyzers, isThreadSafe: true);
    private static readonly Lazy<Type> _typeIDE0130 = new(() => FindType(_assemblyCSharpNetAnalyzers, "Microsoft.CodeAnalysis.CSharp.Analyzers.MatchFolderAndNamespace.CSharpMatchFolderAndNamespaceDiagnosticAnalyzer"), isThreadSafe: true);

    public static DiagnosticAnalyzer IDE0130() =>
        Activator.CreateInstance(_typeIDE0130.Value) as DiagnosticAnalyzer ?? throw new InvalidOperationException($"Could not create instance of '{_typeIDE0130.Value.FullName}'");

    private static Assembly LoadCSharpNetAnalyzers()
    {
        LoadAssembly(Path.Combine(NuGetPackagesFolder, "microsoft.codeanalysis.csharp.codestyle", "4.11.0", "analyzers", "dotnet", "cs", "Microsoft.CodeAnalysis.CodeStyle.dll"));

        return LoadAssembly(Path.Combine(NuGetPackagesFolder, "microsoft.codeanalysis.csharp.codestyle", "4.11.0", "analyzers", "dotnet", "cs", "Microsoft.CodeAnalysis.CSharp.CodeStyle.dll"));
    }
}
