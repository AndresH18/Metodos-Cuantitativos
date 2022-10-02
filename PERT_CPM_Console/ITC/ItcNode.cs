using PERT_CPM_Console.CPM;

namespace PERT_CPM_Console.ITC;

public class ItcNode
{
    public Node Node { get; init; }

    public ItcNode(Node node)
    {
        Node = node;
    }

    public double NormalTime => Node.Length;
    public double CompressedTime { get; init; } = 0;
    public double NormalPrice { get; init; } = 0;
    public double CompressedPrice { get; init; } = 0;

    public double M => NormalTime - CompressedTime;
    public double? K => M != 0 ? (CompressedPrice - NormalPrice) / M : null;
    
    
}