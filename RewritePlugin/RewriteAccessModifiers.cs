using System;
using System.Collections.Generic;
using Microsoft.Quantum.QsCompiler.SyntaxTree;
using Microsoft.Quantum.QsCompiler.Transformations.Core;

namespace Strathweb.QSharp.Rewrite.Demos
{
    public class RewriteAccessModifiers : SyntaxTreeTransformation<RewriteAccessModifiers.TransformationState>
    {
        public class TransformationState { }

        public RewriteAccessModifiers() : base(new TransformationState())
        {
            this.Namespaces = new NamespaceTransformation(this);
        }

        private class NamespaceTransformation : NamespaceTransformation<TransformationState>
        {
            public NamespaceTransformation(SyntaxTreeTransformation<TransformationState> parent)
            : base(parent, TransformationOptions.Default)
            {
            }

            public override QsCallable OnCallableDeclaration(QsCallable c)
            {
                if (!c.SourceFile.EndsWith(".qs")) return c;
                Console.WriteLine($"Callable declaration found: {c.FullName} in {c.SourceFile}");
                c = c.ToCaseDriveAccessModifier();
                return c;
            }
        }
    }
}