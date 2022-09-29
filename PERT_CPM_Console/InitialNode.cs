namespace PERT_CPM_Console;

public sealed class InitialNode
{
    public HashSet<Node> StartNodes { get; set; } = new();
}