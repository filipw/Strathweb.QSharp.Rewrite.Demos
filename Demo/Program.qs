namespace Strathweb.QSharp.Rewrite.Demos {
    
    open Microsoft.Quantum.Intrinsic;

    @EntryPoint()
    operation Main() : Unit {
        let message = "World";
        Message($"Hello{message}");
    }
}
