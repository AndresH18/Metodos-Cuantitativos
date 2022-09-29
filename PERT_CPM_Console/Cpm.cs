using System.Collections.Immutable;

namespace PERT_CPM_Console;

public sealed class Cpm
{
    public InitialNode InitialNode { get; init; }
    public FinalNode FinalNode { get; init; }

    public HashSet<Node> FinalNodes { get; } = new();

    public List<Node> CriticalRoute { get; private set; } = new();

    public double Length { get; private set; } = default;

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

        return Length = InitialNode.StartNodes.Max(n => n.ToEnd());
    }

    public void EndToStart()
    {
        FinalNode.ToStart(Length);
    }

    public List<Node> CalculateCriticalRoute()
    {
        return CriticalRoute = InitialNode.CriticalRoute().ToList();
    }

    public void Calculate()
    {
        StartToEnd();
        EndToStart();
        CalculateCriticalRoute();
    }
}