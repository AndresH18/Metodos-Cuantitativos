using PERT_CPM_Console.CPM;

namespace PERT_CPM_Console;

public sealed class InitialNode
{
    public HashSet<Node> StartNodes { get; set; } = new();

    public List<Node> CriticalRoute()
    {
        var routes = (from n in StartNodes
            where n.Slack == 0
            select n.CalculateCriticalRoute(new List<Node>())).ToList();
        

        return routes.MaxBy(r => r.Count)!;
    }
}