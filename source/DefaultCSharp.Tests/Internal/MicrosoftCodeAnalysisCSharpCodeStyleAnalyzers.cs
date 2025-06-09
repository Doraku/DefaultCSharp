using System;
using System.Reflection;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DefaultCSharp.Internal;

internal sealed class MicrosoftCodeAnalysisCSharpCodeStyleAnalyzers : AnalyzerLoaderBase
{
    private static readonly Lazy<Assembly> _assembly = new(LoadAssembly, isThreadSafe: true);
    private static readonly Lazy<DiagnosticAnalyzer> _analyzerForIDE0130 = CreateDiagnosticAnalyzer(_assembly, "Microsoft.CodeAnalysis.CSharp.Analyzers.MatchFolderAndNamespace.CSharpMatchFolderAndNamespaceDiagnosticAnalyzer");

    public static DiagnosticAnalyzer IDE0130 => _analyzerForIDE0130.Value;

    private static Assembly LoadAssembly()
    {
        LoadAssembly("microsoft.codeanalysis.csharp.codestyle", "4.14.0", "analyzers", "dotnet", "cs", "Microsoft.CodeAnalysis.CodeStyle.dll");

        return LoadAssembly("microsoft.codeanalysis.csharp.codestyle", "4.14.0", "analyzers", "dotnet", "cs", "Microsoft.CodeAnalysis.CSharp.CodeStyle.dll");
    }
}
