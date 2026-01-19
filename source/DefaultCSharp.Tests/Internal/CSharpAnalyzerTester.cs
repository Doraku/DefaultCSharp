using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace DefaultCSharp.Internal;

internal sealed class CSharpAnalyzerTester<TAnalyzer> : CSharpCodeFixTest<TAnalyzer, EmptyCodeFixProvider, DefaultVerifier>
    where TAnalyzer : DiagnosticAnalyzer, new()
{
    private readonly List<DiagnosticAnalyzer> _additionalDiagnosticAnalyzers = [];

    protected override CompilationOptions CreateCompilationOptions() => base.CreateCompilationOptions().WithGeneralDiagnosticOption(ReportDiagnostic.Warn);

    protected override IEnumerable<DiagnosticAnalyzer> GetDiagnosticAnalyzers() => [.. base.GetDiagnosticAnalyzers(), .. _additionalDiagnosticAnalyzers];

    public CSharpAnalyzerTester<TAnalyzer> AddEditorConfig(string filePath, string content)
    {
        TestState.AnalyzerConfigFiles.Add((filePath, content));

        return this;
    }

    public CSharpAnalyzerTester<TAnalyzer> AddSource(string filePath, string code)
    {
        TestState.Sources.Add((filePath, code));

        return this;
    }

    public CSharpAnalyzerTester<TAnalyzer> AddAnalyzers(params DiagnosticAnalyzer[] analyzers)
    {
        _additionalDiagnosticAnalyzers.AddRange(analyzers);

        return this;
    }

    public CSharpAnalyzerTester<TAnalyzer> AddExpectedDiagnostics(params DiagnosticResult[] diagnostics)
    {
        TestState.ExpectedDiagnostics.AddRange(diagnostics);

        return this;
    }
}
