using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

namespace DefaultCSharp.Internal.Extensions;

internal static class SuppressionDescriptorExtensions
{
    public static DiagnosticResult ToDiagnosticResult(this SuppressionDescriptor descriptor, DiagnosticSeverity severity = DiagnosticSeverity.Warning)
        => new(descriptor.SuppressedDiagnosticId, severity);
}
