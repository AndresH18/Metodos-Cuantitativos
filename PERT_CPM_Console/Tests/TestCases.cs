using System.Text;
using PERT_CPM_Console.CPM;
using PERT_CPM_Console.ITC;
using PERT_CPM_Console.PERT;

namespace PERT_CPM_Console.Tests;

public static class TestCases
{
    #region CPM

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
            FinalNodes = {h, i}
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
            FinalNodes = {h, i}
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

    /// <summary>
    /// This test checks the calculate critical route method. For this, both start to end and end to start were used 
    /// </summary>
    public static void Test3()
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
            FinalNodes = {h, i}
        };

        // declare initial Node
        var initial = new InitialNode {StartNodes = {a, b}};
        // create cpm object
        var cpm = new Cpm {InitialNode = initial, FinalNode = final};
        // start to end. get proyect duration.
        var duracionProyecto = cpm.StartToEnd();

        cpm.EndToStart();

        var cRoute = cpm.CalculateCriticalRoute();
        Console.WriteLine("====================================");
        cRoute.ForEach(c => Console.WriteLine(c.Name));
    }

    /// <summary>
    /// Expected output to console: length = 31; Route = {A, B, C, F}
    /// </summary>
    public static void Test4()
    {
        var a = new Node("A", 10);
        var b = new Node("B", 8);
        var c = new Node("C", 10);
        var d = new Node("D", 7);
        var e = new Node("E", 10);
        var f = new Node("F", 3);

        a.AddChild(b).AddChild(c).AddChild(f);
        a.AddChild(d).AddChild(e).AddChild(f);

        var initial = new InitialNode {StartNodes = {a}};

        var final = new FinalNode {FinalNodes = {f}};

        var cpm = new Cpm(initial, final);

        cpm.Calculate();
        Console.WriteLine("=========================");
        Console.WriteLine($"Project length = {cpm.ProjectLength}");
        Console.WriteLine("Critical Route:");
        cpm.CriticalRoute.ForEach(n => Console.WriteLine(n.Name));
    }

    /// <summary>
    /// Expected output to console: length = 22; Route = {A, D, F, H}
    /// </summary>
    public static void Test5()
    {
        var a = new Node("A", 5);
        var b = new Node("B", 3);
        var c = new Node("C", 7);
        var d = new Node("D", 6);
        var e = new Node("E", 7);
        var f = new Node("F", 3);
        var g = new Node("G", 10);
        var h = new Node("H", 8);

        a.AddChild(c).AddChild(h);
        a.AddChild(d).AddChild(f).AddChild(h);
        d.AddChild(g);

        b.AddChild(e).AddChild(f);
        e.AddChild(g);

        var initial = new InitialNode {StartNodes = {a, b}};
        var final = new FinalNode {FinalNodes = {h, g}};

        var cpm = new Cpm(initial, final);
        cpm.Calculate();
        Console.WriteLine("=========================");
        Console.WriteLine($"Project length = {cpm.ProjectLength}");
        Console.WriteLine("Critical Route:");
        cpm.CriticalRoute.ForEach(n => Console.WriteLine(n.Name));
    }

    #endregion

    #region PERT

    /// <summary>
    /// Expected output: Time=52; Route = {A, C, E, G, H}; Deviation=3.7851
    /// </summary>
    public static void Test6()
    {
        var a = new PertNode("A", 4, 8, 12);
        var b = new PertNode("B", 6, 7, 8);
        var c = new PertNode("C", 6, 12, 18);
        var d = new PertNode("D", 3, 5, 7);
        var e = new PertNode("E", 6, 9, 18);
        var f = new PertNode("F", 5, 8, 17);
        var g = new PertNode("G", 10, 15, 20);
        var h = new PertNode("H", 5, 6, 13);

        a.AddChild(b).AddChild(f).AddChild(h);
        f.AddChild(g).AddChild(h);
        a.AddChild(c).AddChild(f);
        c.AddChild(e).AddChild(g);
        a.AddChild(d).AddChild(e);

        var initial = new InitialNode() {StartNodes = {a}};
        var final = new FinalNode() {FinalNodes = {h, e}};

        var pert = new Pert(initial, final);

        pert.Calculate();

        Console.WriteLine($"Project lenght={pert.ProjectLength}");
        Console.WriteLine($"Project Deviation={pert.ProjectDeviation:.0000}");
        pert.CriticalRoute.ForEach(n => Console.WriteLine(n.Name));
    }

    #endregion

    #region ITC

    /// <summary>
    /// Test <see cref="Itc.ObjectiveFunction"/>
    /// </summary>
    public static void Test7()
    {
        var a = new Node("A", 10);
        var b = new Node("B", 8);
        var c = new Node("C", 10);
        var d = new Node("D", 7);
        var e = new Node("E", 10);
        var f = new Node("F", 3);

        a.AddChild(b).AddChild(c).AddChild(f);
        a.AddChild(d).AddChild(e).AddChild(f);

        // var itc = new Itc
        // {
        //     NodeSet =
        //     {
        //         new ItcNode {Node = a},
        //         new ItcNode {Node = b},
        //         new ItcNode {Node = c},
        //         new ItcNode {Node = d},
        //         new ItcNode {Node = e},
        //         new ItcNode {Node = f, CompressedTime = 3},
        //     }
        // };
        //
        // Console.WriteLine(itc.ObjectiveFunction());
    }


    public static void Test8()
    {
        var a = new Node("A", 10);
        var b = new Node("B", 8);
        var c = new Node("C", 10);
        var d = new Node("D", 7);
        var e = new Node("E", 10);
        var f = new Node("F", 3);

        a.AddChild(b).AddChild(c).AddChild(f);
        a.AddChild(d).AddChild(e).AddChild(f);

        var itc = new Itc()
        {
            NodeSet =
            {
                new ItcNode(a)
                {
                    CompressedTime = 8,
                    NormalPrice = 30,
                    CompressedPrice = 70,
                },
                new ItcNode(b)
                {
                    CompressedTime = 6,
                    NormalPrice = 120,
                    CompressedPrice = 150,
                },
                new ItcNode(c)
                {
                    CompressedTime = 7,
                    NormalPrice = 100,
                    CompressedPrice = 160,
                },
                new ItcNode(d)
                {
                    CompressedTime = 6,
                    NormalPrice = 40,
                    CompressedPrice = 50,
                },
                new ItcNode(e)
                {
                    CompressedTime = 8,
                    NormalPrice = 50,
                    CompressedPrice = 75,
                },
                new ItcNode(f)
                {
                    CompressedTime = 3,
                    NormalPrice = 60,
                },
            }
        };

        var r = itc.Generate();
        var sb = new StringBuilder();

        foreach (var line in r)
        {
            sb.AppendJoin(',', line);

            sb.AppendLine();
        }

        var directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        using var file = File.Create(Path.Combine(directory, "metodos.csv"));

        using var fileWriter = new StreamWriter(file);
        fileWriter.WriteLine(sb);
    }

    #endregion
}