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

        var initial = new InitialNode {StartNodes = {a, b}};
        var cpm = new Cpm(initial);

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
}