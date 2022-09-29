using PERT_CPM_Console.CPM;

namespace PERT_CPM_Console;

public abstract class AbstractSolution<T> where T : Node
{
    public InitialNode InitialNode { get; init; }
    public FinalNode FinalNode { get; init; }

    public List<T> CriticalRoute { get; protected set; } = new();

    public virtual double ProjectLength { get; protected set; } = default;

    protected AbstractSolution(InitialNode initialNode, FinalNode finalNode)
    {
        InitialNode = initialNode;
        FinalNode = finalNode;
    }

    public abstract double StartToEnd();

    public abstract void EndToStart();

    public abstract List<T> CalculateCriticalRoute();

    protected abstract void SetNodesToCritical();

    public abstract void Calculate();
}