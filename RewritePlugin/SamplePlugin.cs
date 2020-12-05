#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Quantum.QsCompiler;
using Microsoft.Quantum.QsCompiler.SyntaxTokens;
using Microsoft.Quantum.QsCompiler.SyntaxTree;

namespace Strathweb.QSharp.Rewrite.Demos
{
    public class SamplePlugin : IRewriteStep
    {
        private readonly List<IRewriteStep.Diagnostic> _diagnostics = new List<IRewriteStep.Diagnostic>();
        public string Name => "Case-based Access Modifiers";
        public int Priority => 0;
        public IDictionary<string, string?> AssemblyConstants { get; } = new Dictionary<string, string?>();
        public IEnumerable<IRewriteStep.Diagnostic> GeneratedDiagnostics => _diagnostics;
        public bool ImplementsPreconditionVerification => true;
        public bool ImplementsTransformation => true;
        public bool ImplementsPostconditionVerification => false;

        public bool PreconditionVerification(QsCompilation compilation)
        {
            var callablesWithInvalidAccessibility = compilation.Namespaces.Callables()
                .Where(c => c.SourceFile.EndsWith(".qs"))
                .Where(c => c.Modifiers.Access.IsDefaultAccess && !char.IsUpper(c.FullName.Name[0]) || 
                            c.Modifiers.Access.IsInternal && char.IsUpper(c.FullName.Name[0]));

            foreach (var callable in callablesWithInvalidAccessibility)
            {
                _diagnostics.Add(new IRewriteStep.Diagnostic
                {
                    Severity = DiagnosticSeverity.Warning,
                    Message = $@"Callable '{callable.FullName}' should be {(callable.Modifiers.Access.IsDefaultAccess ? "internal" : "public" )}. This will be auto-corrected.",
                    Stage = IRewriteStep.Stage.PreconditionVerification,
                    Source = callable.SourceFile,
                    Range = callable.Location.IsValue ? callable.Location.Item.Range : null
                });
            }

            return true;
        }

        public bool Transformation(QsCompilation compilation, out QsCompilation transformed)
        {
            Console.WriteLine("Callables before transformation:");
            foreach (var callable in compilation.Namespaces.Callables().Where(c => c.SourceFile.EndsWith(".qs"))) 
            {
                Console.WriteLine(callable.Modifiers.Access + " " + callable.FullName + " | " + callable.SourceFile);
            }

            var rewriter = new RewriteAccessModifiers();
            transformed = rewriter.OnCompilation(compilation);

            Console.WriteLine("Callables after transformation:");
            foreach (var callable in transformed.Namespaces.Callables().Where(c => c.SourceFile.EndsWith(".qs"))) 
            {
                Console.WriteLine(callable.Modifiers.Access + " " + callable.FullName + " | " + callable.SourceFile);
            }
            return true;
        }

        public bool PostconditionVerification(QsCompilation compilation) =>
            throw new NotImplementedException();
    }
}
