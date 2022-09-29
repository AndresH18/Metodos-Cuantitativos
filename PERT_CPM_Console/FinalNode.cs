namespace PERT_CPM_Console;

public class FinalNode
{
    public HashSet<Node> FinalNodes { get; set; } = new();

    public void ToStart(double length)
    {
        foreach (var parentNode in FinalNodes)
        {
            parentNode.ToStart(length);
        }
    }
}