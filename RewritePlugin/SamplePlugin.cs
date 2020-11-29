using System;
using System.Collections.Generic;
using Microsoft.Quantum.QsCompiler;
using Microsoft.Quantum.QsCompiler.SyntaxTree;

namespace Strathweb.QSharp.Rewrite.Demos
{
    public class SamplePlugin : IRewriteStep
    {
        public string Name => "SamplePlugin";
        public int Priority => 1;
        public IDictionary<string, string> AssemblyConstants { get; } = new Dictionary<string, string>();
        public IEnumerable<IRewriteStep.Diagnostic> GeneratedDiagnostics { get; } = new List<IRewriteStep.Diagnostic>();
        public bool ImplementsPreconditionVerification => false;
        public bool ImplementsTransformation => true;
        public bool ImplementsPostconditionVerification => false;

        public bool PreconditionVerification(QsCompilation compilation) =>
            throw new NotImplementedException();

        public bool Transformation(QsCompilation compilation, out QsCompilation transformed)
        {
            transformed = compilation;
            Console.WriteLine("Hello!");
            return true;
        }

        public bool PostconditionVerification(QsCompilation compilation) =>
            throw new NotImplementedException();
    }
}
