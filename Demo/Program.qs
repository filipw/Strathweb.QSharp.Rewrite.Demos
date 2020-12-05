namespace Strathweb.QSharp.Rewrite.Demos {
    
    open Microsoft.Quantum.Intrinsic;

    @EntryPoint()
    operation Main() : Unit {
        let message = "World";
        Message($"Hello {message}");
    }

    function add(n : Int, m : Int) : Int {
        return n + m;
    }

    operation bellState(q1 : Qubit, q2 : Qubit) : Unit is Adj {
        H(q1);
        CNOT(q1, q2);
    }
}
