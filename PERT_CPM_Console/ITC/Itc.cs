using System.Text;

namespace PERT_CPM_Console.ITC;

public class Itc
{
    // private int MCount => NodeSet.Count(node => node.M != 0);
    // private int KCount => NodeSet.Count;
    // private int ColumnCount => MCount + KCount + 3;
    public HashSet<ItcNode> NodeSet { get; set; } = new();

    private Dictionary<string, int> _columnIndex = default!;
    private List<object[]> _arrayList = default!;
    private object[] _currentLineArray = default!;

    public void Generate()
    {
        _arrayList = new List<object[]>();
        CreateIndex();
        HeaderRow(); // header row
        _arrayList.Add(new object[_columnIndex.Count]); // empty line (solver parameters)

        // TODO: k values
        _arrayList.Add(new object[_columnIndex.Count]); // empty line (objective function for solver)
        _arrayList.Add(new object[_columnIndex.Count]); // empty line (separator for variables and solutions)
        MRestrictions();
        XRestrictions();
        FinalRestrictions();
    }

    private void CreateIndex()
    {
        _columnIndex = new Dictionary<string, int>();
        int counter = 0;
        foreach (var node in NodeSet.OrderBy(n => n.Node.Name).ToList())
        {
            if (node.M != 0)
            {
                _columnIndex.Add($"Y{node.Node.Name}", counter++);
            }

            _columnIndex.Add($"X{node.Node.Name}", counter++);
        }

        _columnIndex.Add("XFIN", counter++);
        _columnIndex.Add("R", counter++);
        _columnIndex.Add("V", counter);
    }

    private void HeaderRow()
    {
        _currentLineArray = new object[_columnIndex.Count];
        foreach (var kvp in _columnIndex)
        {
            _currentLineArray[kvp.Value] = kvp.Key;
        }

        _arrayList.Add(_currentLineArray);
    }

    private void MRestrictions()
    {
        foreach (var node in NodeSet.Where(n => n.M != 0).OrderBy(n => n.Node.Name))
        {
            _currentLineArray = new object[_columnIndex.Count];

            var yP = _columnIndex[$"Y{node.Node.Name}"];
            var rP = _columnIndex["R"];
            var vP = _columnIndex["V"];
            _currentLineArray[yP] = 1;
            _currentLineArray[rP] = -1; // <=
            _currentLineArray[vP] = node.M;

            _arrayList.Add(_currentLineArray);
        }
    }

    private void XRestrictions()
    {
        foreach (var node in NodeSet.OrderBy(n => n.Node.Name))
        {
            int yi, xi, xip;
            int r = _columnIndex["R"];
            int v = _columnIndex["V"];

            if (node.Node.ParentNodes.Count == 0)
            {
                // No tiene predecesor (nodo inicial); restriccion = ; tiempo inicial 0
                _currentLineArray = new object[_columnIndex.Count];

                yi = _columnIndex[$"Y{node.Node.Name}"];
                xi = _columnIndex[$"X{node.Node.Name}"];

                _currentLineArray[yi] = 1;
                _currentLineArray[xi] = 1;
                _currentLineArray[v] = node.NormalTime; // node.Node.Length
                _currentLineArray[r] = 0; // =

                _arrayList.Add(_currentLineArray);
            }
            else if (node.Node.ParentNodes.Count == 1)
            {
                // Tiene un predecesor; restriccion =
                _currentLineArray = new object[_columnIndex.Count];

                var pNode = node.Node.ParentNodes.First();
                yi = _columnIndex[$"Y{node.Node.Name}"];
                xi = _columnIndex[$"X{node.Node.Name}"];
                xip = _columnIndex[$"X{pNode.Name}"];

                _currentLineArray[yi] = 1;
                _currentLineArray[xi] = 1;
                _currentLineArray[xip] = -1;
                _currentLineArray[v] = node.NormalTime; // node.Node.Length
                _currentLineArray[r] = 0; // =

                _arrayList.Add(_currentLineArray);
            }
            else
            {
                // else if (node.Node.ParentNodes.Count > 1)
                // tiene mas de un predecesor; restriccion >=
                yi = _columnIndex[$"Y{node.Node.Name}"];
                xi = _columnIndex[$"X{node.Node.Name}"];

                foreach (var pNode in node.Node.ParentNodes)
                {
                    _currentLineArray = new object[_columnIndex.Count];

                    xip = _columnIndex[$"X{pNode.Name}"];

                    _currentLineArray[yi] = 1;
                    _currentLineArray[xi] = 1;
                    _currentLineArray[xip] = -1;
                    _currentLineArray[v] = node.NormalTime; // node.Node.Length
                    _currentLineArray[r] = 1; // >=

                    _arrayList.Add(_currentLineArray);
                }
            }
        }
    }

    private void FinalRestrictions()
    {
        int fi = _columnIndex["XFIN"];
        int ri = _columnIndex["R"];
        int pi;
        foreach (var node in NodeSet.Where(n => n.Node.ChildrenNodes.Count == 0))
        {
            _currentLineArray = new object[_columnIndex.Count];

            pi = _columnIndex[$"X{node.Node.Name}"];

            _currentLineArray[pi] = -1;
            _currentLineArray[fi] = 1;
            _currentLineArray[ri] = 1; // >=

            _arrayList.Add(_currentLineArray);
        }
    }

    [Obsolete]
    public string ObjectiveFunction()
    {
        int _columnCount = 0;
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

        // AppendRestrictionM(sb);

        return sb.ToString();
    }
}