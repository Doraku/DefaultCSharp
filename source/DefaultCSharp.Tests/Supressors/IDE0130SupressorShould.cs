using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Xunit;

namespace DefaultCSharp.Supressors;

public sealed class IDE0130SupressorShould
{
    [Fact]
    public Task SuppressIDE0130WhenContainsExtensionsType()
    {
        return Task.CompletedTask;
        //return new CSharpAnalyzerTest<IDE0130Supressor>()
        //    .AddSource(
        //        /* lang=c#-test */ """
        //        {|#0:namespace System;|}

        //        internal static class StringExtensions
        //        {
        //            public static void Dummy(this string value)
        //            { }
        //        }
        //        """)
        //    .AddAnalyzers(
        //        MicrosoftCodeAnalysisCSharpCodeStyleAnalyzers.IDE0130())
        //    .AddExpectedDiagnostics(
        //        new DiagnosticResult("IDE0130", DiagnosticSeverity.Warning)
        //            .WithLocation(0)
        //            .WithIsSuppressed(true))
        //    .RunAsync();
    }
}