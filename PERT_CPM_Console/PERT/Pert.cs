using System.Text;
using PERT_CPM_Console.CPM;

namespace PERT_CPM_Console.PERT;

public class Pert
{
    public InitialNode InitialNode { get; init; }
    public FinalNode FinalNode { get; init; }
    public List<PertNode> CriticalRoute { get; private set; } = new();
    public double ProjectVariance => CriticalRoute.Sum(n => Math.Pow(n.Deviation, 2));
    public double ProjectDeviation => Math.Sqrt(ProjectVariance);
    public double ProjectLength { get; private set; } = default;

    public HashSet<Node> Nodes { get; init; } = new();


    public Pert(InitialNode initialNode, FinalNode finalNode)
    {
        InitialNode = initialNode;
        FinalNode = finalNode;
    }

    public double StartToEnd()
    {
        return ProjectLength = InitialNode.StartNodes.Max(n => n.ToEnd());
    }

    public void EndToStart()
    {
        FinalNode.ToStart(ProjectLength);
    }

    public List<PertNode> CalculateCriticalRoute()
    {
        var r = InitialNode.CriticalRoute().ToList();
        CriticalRoute = new List<PertNode>();
        r.ForEach(n => CriticalRoute.Add((PertNode) n));
        // return CriticalRoute = InitialNode.CriticalRoute().ToList();
        return CriticalRoute;
    }

    private void SetNodesToCritical()
    {
        CriticalRoute.ForEach(n => n.IsCritical = true);
    }

    public void Calculate()
    {
        StartToEnd();
        EndToStart();
        CalculateCriticalRoute();
        SetNodesToCritical();
    }

    public string FormattedData()
    {
        const string format = "{0,5},{1,8},{2,10},{3,10},{4,10},{5,10:.0000},{6,13},{7,11},{8,11},{9,9},{10,7},{11,7}\n";
        var sb = new StringBuilder();

        sb.AppendLine("==> Resultados <==");
        sb.AppendLine($"Project Lenght: {ProjectLength}");

        sb.AppendFormat(format, "Act", "a", "b", "c", "Length", "std-dev",
            "Early-Start", "Early-End", "Late-Start", "Late-End",
            "Slack", "Crit");

        foreach (var n in Nodes.ToList())
        {
            var node = (PertNode) n;
            sb.AppendFormat(format, node.Name, node.InitialValue, node.LikelyValue, node.LastValue,
                node.Length, node.Deviation,
                node.EarlyStart, node.EarlyEnd, node.LateStart,
                node.LateEnd, node.Slack, node.IsCritical ? "*" : "");
        }

        return sb.ToString();
    }
}