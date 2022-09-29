namespace PERT_CPM_Console;

public static class TestCases
{
    /// <summary>
    /// <p>Test Start to end algorithm</p>
    /// <p>Expected Value: 23</p>
    /// </summary>
    public static bool Test1()
    {
        var a = new Node("A", 7);
        var b = new Node("B", 6);
        var c = new Node("C", 4);
        var d = new Node("D", 2);
        var e = new Node("E", 3);
        var f = new Node("F", 2);
        var g = new Node("G", 4);
        var h = new Node("H", 8);
        var i = new Node("I", 3);

        a.AddChild(c).AddChild(g).AddChild(h);
        a.AddChild(c).AddChild(f).AddChild(h);
        a.AddChild(c).AddChild(f).AddChild(i);

        b.AddChild(d).AddChild(g); // poner el resto de g es redundante
        b.AddChild(d).AddChild(f); // poner el resto de f es redundante

        b.AddChild(e).AddChild(f);

        var end = new FinalNode
        {
            ParentNodes = {h, i}
        };

        var initial = new InitialNode {StartNodes = {a, b}};
        var cpm = new Cpm {InitialNode = initial};

        var duracionProyecto = cpm.StartToEnd();
        Console.WriteLine("=========================");
        Console.WriteLine($"Node {a.Name}: early-start={a.EarlyStart}, early-end={a.EarlyEnd}");
        Console.WriteLine($"Node {b.Name}: early-start={b.EarlyStart}, early-end={b.EarlyEnd}");
        Console.WriteLine($"Node {c.Name}: early-start={c.EarlyStart}, early-end={c.EarlyEnd}");
        Console.WriteLine($"Node {d.Name}: early-start={d.EarlyStart}, early-end={d.EarlyEnd}");
        Console.WriteLine($"Node {e.Name}: early-start={e.EarlyStart}, early-end={e.EarlyEnd}");
        Console.WriteLine($"Node {f.Name}: early-start={f.EarlyStart}, early-end={f.EarlyEnd}");
        Console.WriteLine($"Node {g.Name}: early-start={g.EarlyStart}, early-end={g.EarlyEnd}");
        Console.WriteLine($"Node {h.Name}: early-start={h.EarlyStart}, early-end={h.EarlyEnd}");
        Console.WriteLine($"Node {i.Name}: early-start={i.EarlyStart}, early-end={i.EarlyEnd}");
        return duracionProyecto == 23;
    }

    /// <summary>
    /// This test checks the end to start method. Due to this, it uses the start to end method.
    /// </summary>
    public static void Test2()
    {
        var a = new Node("A", 7);
        var b = new Node("B", 6);
        var c = new Node("C", 4);
        var d = new Node("D", 2);
        var e = new Node("E", 3);
        var f = new Node("F", 2);
        var g = new Node("G", 4);
        var h = new Node("H", 8);
        var i = new Node("I", 3);

        a.AddChild(c).AddChild(g).AddChild(h);
        a.AddChild(c).AddChild(f).AddChild(h);
        a.AddChild(c).AddChild(f).AddChild(i);

        b.AddChild(d).AddChild(g); // poner el resto de g es redundante
        b.AddChild(d).AddChild(f); // poner el resto de f es redundante

        b.AddChild(e).AddChild(f);

        // declare end node
        var final = new FinalNode
        {
            ParentNodes = {h, i}
        };

        // declare initial Node
        var initial = new InitialNode {StartNodes = {a, b}};
        // create cpm object
        var cpm = new Cpm {InitialNode = initial, FinalNode = final};
        // start to end. get proyect duration.
        var duracionProyecto = cpm.StartToEnd();

        cpm.EndToStart();
        
        Console.WriteLine("=========================");
        Console.WriteLine($"Node {a.Name}: late-start={a.LateStart}, late-end={a.LateEnd}");
        Console.WriteLine($"Node {b.Name}: late-start={b.LateStart}, late-end={b.LateEnd}");
        Console.WriteLine($"Node {c.Name}: late-start={c.LateStart}, late-end={c.LateEnd}");
        Console.WriteLine($"Node {d.Name}: late-start={d.LateStart}, late-end={d.LateEnd}");
        Console.WriteLine($"Node {e.Name}: late-start={e.LateStart}, late-end={e.LateEnd}");
        Console.WriteLine($"Node {f.Name}: late-start={f.LateStart}, late-end={f.LateEnd}");
        Console.WriteLine($"Node {g.Name}: late-start={g.LateStart}, late-end={g.LateEnd}");
        Console.WriteLine($"Node {h.Name}: late-start={h.LateStart}, late-end={h.LateEnd}");
        Console.WriteLine($"Node {i.Name}: late-start={i.LateStart}, late-end={i.LateEnd}");
    }
}