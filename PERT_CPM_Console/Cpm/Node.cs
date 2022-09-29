namespace PERT_CPM_Console;

public class Node
{
    public string Name { get; protected init; } = string.Empty;
    public virtual double Length { get;  }
    public double EarlyStart { get; private set; }
    public double EarlyEnd => EarlyStart + Length;

    public double LateEnd { get; private set; } = double.MaxValue;
    public double LateStart => LateEnd - Length;

    public double Slack => LateStart - EarlyStart;

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

        // Console.WriteLine($"Node {Name}: length={Length}, early-start={EarlyStart}");
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

    public List<Node> CalculateCriticalRoute(List<Node> parentList)
    {
        var myList = new List<Node>(parentList);
        myList.Add(this);

        return (from node in ChildrenNodes
            where node.Slack == 0
            select node.CalculateCriticalRoute(myList)).MaxBy(l => l.Count) ?? myList;
    }

    public Node AddChild(Node node)
    {
        ChildrenNodes.Add(node);
        node.ParentNodes.Add(this);
        return node;
    }
}