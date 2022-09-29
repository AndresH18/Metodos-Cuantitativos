namespace PERT_CPM_Console.Cpm;

public sealed class Cpm
{
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
}