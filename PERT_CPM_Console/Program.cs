// See https://aka.ms/new-console-template for more information


using System.Text;
using PERT_CPM_Console;
using PERT_CPM_Console.CPM;
using PERT_CPM_Console.ITC;
using PERT_CPM_Console.PERT;
using PERT_CPM_Console.Tests;
using FinalNode = PERT_CPM_Console.FinalNode;
using InitialNode = PERT_CPM_Console.InitialNode;


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

Console.WriteLine(">>===> PERT/CPM <===<<");

Console.WriteLine(@"Ingrese -c para ""CPM"" o -p para ""PERT""");

var response = Console.ReadLine()?.Replace(" ", "") ?? "";

if (response.Contains("-c"))
{
    Console.WriteLine(
        "Ingrese la información de los nodos en el siguiente formato, " +
        "Pulse enter sin ingresar ningún dato para continuar");
    Console.WriteLine("<nombre>, <duración>\n" + "Ejemplo: A, 1.2");

    nodesDictionary = new Dictionary<string, Node>();

    while (!string.IsNullOrWhiteSpace(response = Console.ReadLine()?.ToUpper()))
    {
        var r = response.Replace(" ", "").Split(',');
        if (r.Length > 1)
        {
            var name = r[0];
            double length;
            if (double.TryParse(r[1], out length))
            {
                if (nodesDictionary.TryAdd(name, new Node(name, length)))
                {
                    Console.WriteLine($"Name={name}, Length={length}\n");
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
                      "Si no ingresa un nombre, se considerara el nodo como una actividad inicial.\n");
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

    Console.WriteLine(cpm.FormattedData());

    Console.WriteLine("\n\nDesea hacer \"Intercambio Tiempo Costo\"? [Y/N]");
    if (Console.ReadLine()?.ToUpper().Equals("Y") ?? false)
    {
        Itc();
    }
}
else if (response.Contains("-p"))
{
    // ask for node's information (name, time_a, time_m, time_b)
    Console.WriteLine(
        "Ingrese la información de los nodos en el siguiente formato, " +
        "Pulse enter sin ingresar ningún dato para continuar");
    Console.WriteLine("<nombre>, <t-optimista>, <t-mas-probable>, <t-pesimista>\n" + "Ejemplo: A, 1.2, 2, 3");

    nodesDictionary = new Dictionary<string, Node>();

    while (!string.IsNullOrWhiteSpace(response = Console.ReadLine()?.ToUpper()))
    {
        var r = response.Replace(" ", "").Split(',');
        if (r.Length >= 4)
        {
            var name = r[0];
            double initial;
            double probable;
            double lastly;
            if (double.TryParse(r[1], out initial) && double.TryParse(r[2], out probable) &&
                double.TryParse(r[3], out lastly))
            {
                if (nodesDictionary.TryAdd(name, new PertNode(name, initial, probable, lastly)))
                {
                    Console.WriteLine(
                        $"Name={name}, Initial-Time={initial}, Probable-Time={probable}," +
                        $" Last-Time={lastly}, Length={nodesDictionary[name].Length}\n");
                }
                else
                {
                    Console.WriteLine($"El nodo {name} ya existe.");
                }
            }
            else
            {
                Console.WriteLine("Ingrese tiempos validos");
            }
        }
        else
        {
            Console.WriteLine("Ingrese información valida");
        }
    }

    initialNode = new InitialNode();
    finalNode = new FinalNode();

    // ask for each node's parent by name. Nodes without parents are start nodes
    Console.WriteLine("Ingrese los nombres de los padres de los nodos, separados por coma ','.\n" +
                      "Si no ingresa un nombre, se considerara el nodo como una actividad inicial.\n");
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

    // ask for final nodes
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

    // TODO: perform PERT. Show nodes information
    //      (name, lenght, early-start, early-end, late-start, late-end, variance, is-critical)

    var pert = new Pert(initialNode, finalNode) {Nodes = nodesDictionary.Values.ToHashSet()};

    pert.Calculate();

    Console.WriteLine(pert.FormattedData());

    Console.WriteLine("\n\nDesea hacer \"Intercambio Tiempo Costo\"? [Y/N]");
    if (Console.ReadLine()?.ToUpper().Equals("Y") ?? false)
    {
        Itc();
    }
}


void Itc()
{
    // ask information for ITC

    var nodeSet = new HashSet<ItcNode>();

    Console.WriteLine("Ingrese los datos de tiempo y costo de las actividades.\n" +
                      "Utilize el siguiete formato: <tiempo-comprimido>, <costo-normal>, <costo-comprimido>\n");

    bool cont = false;
    foreach (var kvp in nodesDictionary)
    {
        string[]? resp;
        do
        {
            Console.WriteLine($"Ingrese los datos para el nodo {kvp.Key}");

            resp = Console.ReadLine()?.Replace(" ", "").ToUpper().Split(',');
            double tiempoComprimido, costoNormal, costoComprimido;

            if (resp is {Length: >= 3} && double.TryParse(resp[0], out tiempoComprimido) &&
                double.TryParse(resp[1], out costoNormal) && double.TryParse(resp[2], out costoComprimido))
            {
                nodeSet.Add(new ItcNode(kvp.Value)
                    {CompressedTime = tiempoComprimido, NormalPrice = costoNormal, CompressedPrice = costoComprimido});
                cont = false;
            }
            else
            {
                Console.WriteLine("Ingrese Datos Validos");
                cont = true;
            }
        } while (cont);
    }
    // TODO: ask compression time

    var itc = new Itc {NodeSet = nodeSet};
    Console.WriteLine("Ingrese el tiempo de compresión deseado para el proyecto.");

    response = Console.ReadLine()?.ToUpper().Replace(" ", "");

    if (double.TryParse(response, out var d))
    {
        // calculate Itc
        var r = itc.Generate(d);

        // ask for storage location
        Console.WriteLine(
            "Ingrese la ruta del directorio en la que quiere guardar el archivo. Si se deja en blanco, se guardara en el escritorio");
        response = Console.ReadLine();

        // create string
        var sb = new StringBuilder();
        foreach (var line in r)
        {
            sb.AppendJoin(',', line);
            sb.AppendLine();
        }

        var directory = !string.IsNullOrWhiteSpace(response)
            ? response
            : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        using var file = File.Create(Path.Combine(directory, "itc_metodos_output.csv"));

        using var fileWriter = new StreamWriter(file);
        fileWriter.WriteLine(sb);
        Console.WriteLine($"Output: {file.Name}");
    }
}