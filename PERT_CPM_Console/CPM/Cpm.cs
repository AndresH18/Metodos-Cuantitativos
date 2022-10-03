using System.Text;

namespace PERT_CPM_Console.CPM;

public sealed class Cpm
{
    public HashSet<Node> Nodes { get; init; } = new();
    public InitialNode InitialNode { get; init; }
    public FinalNode FinalNode { get; init; }
    public List<Node> CriticalRoute { get; private set; } = new();

    public double ProjectLength { get; private set; } = default;

    public Cpm()
    {
    }

    public Cpm(InitialNode initialNode, FinalNode finalNode)
    {
        InitialNode = initialNode;
        FinalNode = finalNode;
    }


    public double StartToEnd()
    {
        // if (Length == null)
        // {
        //     Length = _initial.StartNodes.Max(n => n.ToEnd());
        // }
        // return Length;

        return ProjectLength = InitialNode.StartNodes.Max(n => n.ToEnd());
    }

    public void EndToStart()
    {
        FinalNode.ToStart(ProjectLength);
    }

    public List<Node> CalculateCriticalRoute()
    {
        return CriticalRoute = InitialNode.CriticalRoute().ToList();
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

    [Obsolete]
    public string FormattedCriticalRoute()
    {
        const string format = "{0,5},{1,11},{2,11},{3,11},{4,11},{5,11},{6,10}\n";
        var sb = new StringBuilder();

        sb.AppendFormat(format, "Act", "Length", "Early-Start", "Early-End", "Late-Start", "Late-End", "Crit");

        foreach (var node in CriticalRoute.ToList())
        {
            var q = node.ParentNodes.Select(p => p.Name);
            StringBuilder pred = new StringBuilder();
            foreach (var s in q)
            {
                pred.Append($"{s},");
            }

            sb.AppendFormat(format, node.Name, node.Length, node.EarlyStart, node.EarlyEnd, node.LateStart,
                node.LateEnd, node.IsCritical ? "*" : "");
        }

        return sb.ToString();
    }

    public string FormattedData()
    {
        const string format = "{0,5},{1,8},{2,13},{3,11},{4,11},{5,9},{6,7},{7,7}\n";
        var sb = new StringBuilder();

        sb.AppendLine("==> Resultados <==");
        sb.AppendLine($"Project Lenght: {ProjectLength}");

        sb.AppendFormat(format, "Act", "Length", "Early-Start", "Early-End", "Late-Start", "Late-End", "Slack", "Crit");

        foreach (var node in Nodes.ToList())
        {
            sb.AppendFormat(format, node.Name, node.Length, node.EarlyStart, node.EarlyEnd, node.LateStart,
                node.LateEnd, node.Slack, node.IsCritical ? "*" : "");
        }

        return sb.ToString();
    }
}