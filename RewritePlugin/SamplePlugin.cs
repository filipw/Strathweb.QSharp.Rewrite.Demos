#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Quantum.QsCompiler;
using Microsoft.Quantum.QsCompiler.SyntaxTokens;
using Microsoft.Quantum.QsCompiler.SyntaxTree;

namespace Strathweb.QSharp.Rewrite.Demos
{
    public class SamplePlugin : IRewriteStep
    {
        public string Name => "Case-based Access Modifiers";
        public int Priority => 0;
        public IDictionary<string, string?> AssemblyConstants { get; } = new Dictionary<string, string?>();
        public IEnumerable<IRewriteStep.Diagnostic> GeneratedDiagnostics { get; } = new List<IRewriteStep.Diagnostic>();
        public bool ImplementsPreconditionVerification => false;
        public bool ImplementsTransformation => true;
        public bool ImplementsPostconditionVerification => false;

        public bool PreconditionVerification(QsCompilation compilation) =>
            throw new NotImplementedException();

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
