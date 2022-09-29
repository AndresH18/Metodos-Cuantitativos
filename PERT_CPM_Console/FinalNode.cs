namespace PERT_CPM_Console;

public class FinalNode
{
    public HashSet<Node> ParentNodes { get; set; } = new();

    public void ToStart(double length)
    {
        foreach (var parentNode in ParentNodes)
        {
            parentNode.ToStart(length);
        }
    }
}