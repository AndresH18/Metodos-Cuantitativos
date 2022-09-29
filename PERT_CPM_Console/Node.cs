namespace PERT_CPM_Console;

public class Node
{
    public string Name { get; set; } = string.Empty;
    public double Length { get; set; }
    public double EarlyStart { get; set; }
    public double EarlyEnd => EarlyStart + Length;

    public double LateEnd { get; set; } = double.MaxValue;
    public double LateStart => LateEnd - Length;

    public bool IsCritical { get; set; }

    public HashSet<Node> ParentNodes { get; } = new();
    public HashSet<Node> ChildrenNodes { get; } = new();


    public Node()
    {
    }

    public Node(string name, double length)
    {
        Name = name;
        Length = length;
    }

    public double ToEnd(Node? parent = default)
    {
        if (parent is not null)
        {
            if (parent.EarlyEnd > EarlyStart)
            {
                EarlyStart = parent.EarlyEnd;
            }
        }

        Console.WriteLine($"Node {Name}: length={Length}, early-start={EarlyStart}");
        return ChildrenNodes.Count == 0 ? EarlyEnd : ChildrenNodes.Max(n => n.ToEnd(this));
    }

    // public void ToStart(Node? child = default)
    // {
    //     if (child is not null)
    //     {
    //         if (child.)
    //     }
    // }

    public void ToStart(double parentLateStart)
    {
        if (LateEnd > parentLateStart)
        {
            LateEnd = parentLateStart;
        }

        foreach (var parentNode in ParentNodes)
        {
            parentNode.ToStart(LateStart);
        }
    }

    public Node AddChild(Node node)
    {
        ChildrenNodes.Add(node);
        node.ParentNodes.Add(this);
        return node;
    }
}