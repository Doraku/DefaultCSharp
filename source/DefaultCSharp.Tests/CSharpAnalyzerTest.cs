using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace DefaultCSharp;

public sealed class CSharpAnalyzerTest<TAnalyzer> : CSharpCodeFixTest<TAnalyzer, EmptyCodeFixProvider, DefaultVerifier>
        where TAnalyzer : DiagnosticAnalyzer, new()
{
    private readonly List<DiagnosticAnalyzer> _additionalDiagnosticAnalyzers = [];

    protected override CompilationOptions CreateCompilationOptions() => base.CreateCompilationOptions().WithGeneralDiagnosticOption(ReportDiagnostic.Warn);

    protected override IEnumerable<DiagnosticAnalyzer> GetDiagnosticAnalyzers() => [.. base.GetDiagnosticAnalyzers(), .. _additionalDiagnosticAnalyzers];

    public CSharpAnalyzerTest<TAnalyzer> AddSource(string code)
    {
        TestState.Sources.Add(code);

        return this;
    }

    public CSharpAnalyzerTest<TAnalyzer> AddAnalyzers(params DiagnosticAnalyzer[] analyzers)
    {
        _additionalDiagnosticAnalyzers.AddRange(analyzers);

        return this;
    }

    public CSharpAnalyzerTest<TAnalyzer> AddExpectedDiagnostics(params DiagnosticResult[] diagnostics)
    {
        TestState.ExpectedDiagnostics.AddRange(diagnostics);

        return this;
    }
}
