// See https://aka.ms/new-console-template for more information


using PERT_CPM_Console;

// Console.WriteLine("Hello, World!");
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


TestCases.Test2();