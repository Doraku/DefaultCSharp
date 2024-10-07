using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
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
    }
}
