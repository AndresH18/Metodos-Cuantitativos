// See https://aka.ms/new-console-template for more information


using PERT_CPM_Console;
using PERT_CPM_Console.CPM;
using PERT_CPM_Console.ITC;
using PERT_CPM_Console.PERT;
using PERT_CPM_Console.Tests;
using FinalNode = PERT_CPM_Console.FinalNode;
using InitialNode = PERT_CPM_Console.InitialNode;

Console.WriteLine("Hello, World!");
Itc();
//
// var a = new Node {Name = "A", Length = 1};
// var b = new Node {Name = "B", Length = 2};
// var c = new Node {Name = "C", Length = 3};
// var d = new Node {Name = "D", Length = 4};
// var e = new Node {Name = "E", Length = 2};
// var f = new Node {Name = "F", Length = 10};
// var g = new Node {Name = "G", Length = 7};
// var h = new Node {Name = "H", Length = 5};
// var i = new Node {Name = "I", Length = 1};
// var j = new Node {Name = "J", Length = 2};
// var k = new Node {Name = "K", Length = 1};
// var l = new Node {Name = "L", Length = 4};
//
// a.AddChild(b);
// a.AddChild(d);
//
// b.AddChild(c);
//
// e.AddChild(f).AddChild(g);
// e.AddChild(f).AddChild(h).AddChild(i);
//
// e.AddChild(f).AddChild(j).AddChild(l);
// e.AddChild(f).AddChild(k).AddChild(l);
//
//
// // e.AddChild()
// /*
//  * A -> B -> C            (l = 1 + 2 + 3 = 6)
//  * A -> D                 (l = 1 + 4 = 5)
//  * E -> F -> G            (l = 2 + 10 + 7 = 19)
//  * E -> F -> H -> I       (l = 2 + 10 + 5 + 1 = 18)
//  * E -> F -> J -> L       (l = 2 + 10 + 2 + 4 = 16) 12
//  * E -> F -> K -> L       (l = 2 + 10 + 1 + 4 = 17) 13
//  * E -> F -> (J, K) -> L  (max l = 17)
//  */
//
// var cpm = new Cpm(new InitialNode {StartNodes = {a, e}});
//
// Console.WriteLine($"Max Lenght = {cpm.StartToEnd()}");


// // var a = new Node("A", 5);
// // var b = new Node("B", 3);
// // var c = new Node("C", 7);
// // var d = new Node("D", 6);
// // var e = new Node("E", 7);
// // var f = new Node("F", 3);
// // var g = new Node("G", 10);
// // var h = new Node("H", 8);

// // a.AddChild(c).AddChild(h);
// // a.AddChild(d).AddChild(f).AddChild(h);
// // d.AddChild(g);

// // b.AddChild(e).AddChild(f);
// // e.AddChild(g);

// // var initial = new InitialNode { StartNodes = { a, b } };
// // var final = new FinalNode { FinalNodes = { h, g } };

// // var cpm = new Cpm(initial, final);
// // cpm.Calculate();

// // cpm.Nodes = new HashSet<Node>() { a, b, c, d, e, f, g, h };

// // cpm.CriticalRoute.ToList().ForEach(n => Console.WriteLine(n.Name));

// TestCases.Test8();

Dictionary<string, Node> nodesDictionary;
InitialNode initialNode;
FinalNode finalNode;

Console.WriteLine(@"Ingrese -c para ""CPM"" o -p para ""PERT""");

var response = Console.ReadLine() ?? "";
if (response.Contains("-c"))
{
    Console.WriteLine(
        "Ingrese la información de los nodos en el siguiente formato, " +
        "Pulse enter sin ingresar ningún dato para continuar");
    Console.WriteLine("<nombre>;<duración>\n" + "Ejemplo: A; 1.2");

    nodesDictionary = new Dictionary<string, Node>();

    while (!string.IsNullOrWhiteSpace(response = Console.ReadLine()?.ToUpper()))
    {
        var r = response.Replace(" ", "").Split(';');
        if (r.Length > 1)
        {
            var name = r[0];
            double length;
            if (double.TryParse(r[1], out length))
            {
                if (nodesDictionary.TryAdd(name, new Node(name, length)))
                {
                    Console.WriteLine($"Name={name}, Length={length}");
                }
                else
                {
                    Console.WriteLine($"El nodo {name} ya existe.");
                }
            }
            else
            {
                Console.WriteLine("Ingrese una longitud valida");
            }
        }
        else
        {
            Console.WriteLine("Ingrese información valida");
        }
    }

    initialNode = new InitialNode();
    finalNode = new FinalNode();

    Console.WriteLine("Ingrese los nombres de los padres de los nodos, separados por coma ','.\n" +
                      "Si no ingresa un nombre, se considerara el nodo como una actividad inicial.");
    foreach (var kvp in nodesDictionary)
    {
        Console.WriteLine($"Ingrese el nombre de los padres del nodo {kvp.Key}");
        response = Console.ReadLine()?.Replace(" ", "").ToUpper();

        if (!string.IsNullOrWhiteSpace(response))
        {
            var parentsNames = response.Split(',');
            foreach (var name in parentsNames)
            {
                if (nodesDictionary.TryGetValue(name, out var parentNode))
                {
                    kvp.Value.AddParent(parentNode);
                }
                else
                {
                    Console.WriteLine($"No existe un nodo con nombre {name}");
                }
            }
        }
        else
        {
            initialNode.StartNodes.Add(kvp.Value);
            Console.WriteLine($"{kvp.Key} es un nodo inicial");
        }
    }

    Console.WriteLine("\nIngrese los Nodos finales separados por ','");
    response = Console.ReadLine()?.Replace(" ", "").ToUpper() ?? "";
    foreach (var name in response.Split(','))
    {
        if (nodesDictionary.TryGetValue(name, out var node))
        {
            finalNode.FinalNodes.Add(node);
        }
        else
        {
            Console.WriteLine($"No existe el nodo {name}");
        }
    }

    var cpm = new Cpm(initialNode, finalNode) {Nodes = nodesDictionary.Values.ToHashSet()};

    cpm.Calculate();

    // TODO: show each node's slack
    Console.WriteLine(cpm.FormattedData());

    Console.WriteLine("\n\nDesea hacer \"Intercambio Tiempo Costo\"? [Y/N]");
    if (Console.ReadLine()?.ToUpper().Equals("Y") ?? false)
    {
        Itc();
    }
}
else if (response.Contains("-p"))
{
    // TODO: ask for node's information (name, time_a, time_m, time_b)

    // TODO: ask for each node's parent by name. Nodes without parents are start nodes

    // TODO: ask for final nodes

    // TODO: perform PERT. Show nodes information
    //      (name, lenght, early-start, early-end, late-start, late-end, variance, is-critical)

    Console.WriteLine("\n\nDesea hacer \"Intercambio Tiempo Costo\"? [Y/N]");
    if (Console.ReadLine()?.ToUpper().Equals("Y") ?? false)
    {
        Itc();
    }
}


void Itc()
{
    // TODO: ask information for ITC

    // TODO: ask compression time

    // TODO: calculate Itc
}