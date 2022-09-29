using PERT_CPM_Console.CPM;

namespace PERT_CPM_Console.PERT;

public class PertNode : Node
{
    public double InitialValue { get; set; }
    public double LikelyValue { get; set; }
    public double LastValue { get; set; }

    public override double Length => (InitialValue + 4 * LikelyValue + LastValue) / 6;

    public double Deviation => (LastValue - InitialValue) / 6;

    public PertNode()
    {
    }

    public PertNode(string name, double initialValue, double likelyValue, double lastValue)
    {
        Name = name;
        InitialValue = initialValue;
        LikelyValue = likelyValue;
        LastValue = lastValue;
    }
}