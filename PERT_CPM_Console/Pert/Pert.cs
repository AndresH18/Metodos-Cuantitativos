namespace PERT_CPM_Console.Pert;


public class Pert
{
    public InitialNode InitialNode { get; init; }
    public FinalNode FinalNode { get; init; }
    public List<PertNode> CriticalRoute { get; private set; } = new();
    public double ProjectVariance => CriticalRoute.Sum(n => Math.Pow(n.Deviation, 2));
    public double ProjectDeviation => Math.Sqrt(ProjectVariance);

    public double ProjectLength { get; private set; } = default;

    public Pert()
    {
    }

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
}