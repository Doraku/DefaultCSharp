using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DefaultCSharp.Supressors;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class IDE0130Supressor : DiagnosticSuppressor
{
    public static readonly SuppressionDescriptor Rule = new(
        "DCS0130",
        "IDE0130",
        "Extension type should have same namespace as extended type.");

    public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions => ImmutableArray.Create(Rule);

    public override void ReportSuppressions(SuppressionAnalysisContext context)
    {
        foreach (Diagnostic diagnostic in context
                .ReportedDiagnostics
                .Where(diagnostic => diagnostic
                    .Location
                    .SourceTree
                    ?.GetRoot(context.CancellationToken)
                    .DescendantNodesAndSelf()
                    .OfType<MethodDeclarationSyntax>()
                    .Any(method => method
                        .ParameterList
                        .Parameters
                        .FirstOrDefault()
                        ?.Modifiers
                        .Any(modifier => modifier.ValueText == "this")
                        ?? false)
                    ?? false))
        {
            context.ReportSuppression(Suppression.Create(Rule, diagnostic));
        }
    }
}
