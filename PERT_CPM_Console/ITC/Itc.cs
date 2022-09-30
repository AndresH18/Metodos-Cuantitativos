using System.Text;

namespace PERT_CPM_Console.ITC;

public class Itc
{
    private int _columnCount;
    public HashSet<ItcNode> NodeSet { get; } = new();


    private static void H()
    {
    }

    public string ObjectiveFunction()
    {
        var sb = new StringBuilder();
        sb.Append(',');
        /* create first row (header rows) */
        // Y values
        NodeSet.Where(itn => itn.M != 0)
            .Select(itn => itn.Node)
            .OrderBy(n => n.Name)
            .Select(n => n.Name)
            .ToList().ForEach(name =>
            {
                sb.AppendFormat("Y{0},", name);
                _columnCount++;
            });
        // X values
        NodeSet.Select(itn => itn.Node)
            .OrderBy(n => n.Name)
            .Select(n => n.Name)
            .ToList().ForEach(name =>
            {
                sb.AppendFormat("Y{0},", name);
                _columnCount++;
            });
        sb.AppendLine("LHS,RST,RHS");

        AppendRestrictionM(sb);

        return sb.ToString();
    }

    public void AppendRestrictionM(StringBuilder sb)
    {
    }
}

internal class InitialNode
{
    public HashSet<ItcNode> StartNodes { get; set; } = new();
}

internal class FinalNode
{
    public HashSet<ItcNode> EndNodes { get; set; } = new();
}