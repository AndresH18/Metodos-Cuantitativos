namespace PERT_CPM_Console;

public sealed class Cpm
{
    public InitialNode InitialNode { get; init; }
    public FinalNode FinalNode { get; init; }

    public HashSet<Node> FinalNodes { get; init; } = new();

    public double? Length { get; private set; } = default;


    public double StartToEnd()
    {
        // if (Length == null)
        // {
        //     Length = _initial.StartNodes.Max(n => n.ToEnd());
        // }
        // return Length;

        return Length ??= InitialNode.StartNodes.Max(n => n.ToEnd());
    }

    public void EndToStart()
    {
        FinalNode.ToStart(Length ?? 0);
    }

    public void CriticalRoute()
    {
        
    }
}