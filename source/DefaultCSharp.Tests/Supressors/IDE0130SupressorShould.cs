using System.Threading.Tasks;
using Xunit;

namespace DefaultCSharp.Supressors;

public sealed class IDE0130SupressorShould
{
    private readonly CSharpAnalyzerTester<IDE0130Supressor> _tester;

    public IDE0130SupressorShould()
    {
        _tester = new CSharpAnalyzerTester<IDE0130Supressor>()
            .AddEditorConfig(
                "/0/.editorconfig",
                """
                is_global=true
                
                build_property.ProjectDir = /0/
                build_property.RootNamespace = Test

                dotnet_style_namespace_match_folder = true

                dotnet_analyzer_diagnostic.severity = none
                dotnet_diagnostic.IDE0130.severity = warning
                """)
            .AddAnalyzers(
                MicrosoftCodeAnalysisCSharpCodeStyleAnalyzers.IDE0130);
    }

    [Fact]
    public Task SuppressIDE0130WhenContainsExtensionsType()
    {
        return _tester
            .AddSource(
                "/0/Test/Test.cs",
                /* lang=c#-test */ """
                namespace {|#0:System|};

                internal static class StringExtensions
                {
                    public static void Dummy(this string value)
                    { }
                }
                """)
            .AddExpectedDiagnostics(
                IDE0130Supressor.Rule.ToDiagnosticResult().WithLocation(0).WithIsSuppressed(true))
            .RunAsync();
    }

    [Fact]
    public Task NotSuppressIDE0130WhenDoNotContainsExtensionsType()
    {
        return _tester
            .AddSource(
                "/0/Test/Test.cs",
                /* lang=c#-test */ """
                namespace {|#0:DummyNamespace|};

                internal static class DummyClass
                {
                    public static void DummyMethod(string value)
                    { }
                }
                """)
            .AddExpectedDiagnostics(
                IDE0130Supressor.Rule.ToDiagnosticResult().WithLocation(0))
            .RunAsync();
    }
}