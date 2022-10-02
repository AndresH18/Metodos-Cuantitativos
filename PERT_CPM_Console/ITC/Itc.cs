using System.Text;

namespace PERT_CPM_Console.ITC;

public class Itc
{
    public const string XPre = "X";
    public const string YPre = "Y";
    public const string V = "V";
    public const string R = "R";
    public const string Fin = XPre + "FIN";
    public const string Less = "<=";
    public const string Equal = "=";
    public const string Greater = ">=";

    // private int MCount => NodeSet.Count(node => node.M != 0);
    // private int KCount => NodeSet.Count;
    // private int ColumnCount => MCount + KCount + 3;
    public HashSet<ItcNode> NodeSet { get;} = new();

    private Dictionary<string, int> _columnIndex = default!;
    private List<object[]> _arrayList = default!;
    private object[] _currentLineArray = default!;

    public List<object[]> Generate()
    {
        _arrayList = new List<object[]>();
        CreateIndex();
        HeaderRow(); // header row
        _arrayList.Add(Array.Empty<object>()); // empty line (solver parameters)

        KValues(); // k values 
        _arrayList.Add(new object[_columnIndex.Count]); // empty line (objective function for solver)
        _arrayList.Add(new object[_columnIndex.Count]); // empty line (separator for variables and solutions)
        MRestrictions();
        XRestrictions();
        EndRestrictions();

        _currentLineArray = new object[_columnIndex.Count];
        int fRow, rRow;
        if (_columnIndex.TryGetValue(Fin, out fRow) && _columnIndex.TryGetValue(R, out rRow))
        {
            _currentLineArray[fRow] = 1;
            _currentLineArray[rRow] = Equal;
        }

        _arrayList.Add(_currentLineArray);

        return _arrayList;
    }

    private void CreateIndex()
    {
        _columnIndex = new Dictionary<string, int>();
        int counter = 0;
        foreach (var node in NodeSet.OrderBy(n => n.Node.Name).ToList())
        {
            if (node.M != 0)
            {
                _columnIndex.Add($"{YPre}{node.Node.Name}", counter++);
            }

            _columnIndex.Add($"{XPre}{node.Node.Name}", counter++);
        }

        _columnIndex.Add(Fin, counter++);
        _columnIndex.Add(R, counter++);
        _columnIndex.Add(V, counter);
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

    private void KValues()
    {
        _currentLineArray = new object[_columnIndex.Count];

        var r = NodeSet.Where(n => n.M != 0);
        int i;
        foreach (var node in r)
        {
            i = _columnIndex[$"{YPre}{node.Node.Name}"];

            _currentLineArray[i] = node.K!;
        }

        _arrayList.Add(_currentLineArray);
    }

    private void MRestrictions()
    {
        foreach (var node in NodeSet.Where(n => n.M != 0).OrderBy(n => n.Node.Name))
        {
            _currentLineArray = new object[_columnIndex.Count];

            var yP = _columnIndex[$"{YPre}{node.Node.Name}"];
            var rP = _columnIndex[R];
            var vP = _columnIndex[V];
            _currentLineArray[yP] = 1;
            _currentLineArray[rP] = Less; // <=
            _currentLineArray[vP] = node.M;

            _arrayList.Add(_currentLineArray);
        }
    }

    private void XRestrictions()
    {
        foreach (var node in NodeSet.OrderBy(n => n.Node.Name))
        {
            int yi, xi, xip;
            int r = _columnIndex[R];
            int v = _columnIndex[V];


            if (node.Node.ParentNodes.Count == 0)
            {
                // No tiene predecesor (nodo inicial); restriccion = ; tiempo inicial 0
                _currentLineArray = new object[_columnIndex.Count];

                // yi = _columnIndex[$"{YPre}{node.Node.Name}"];

                xi = _columnIndex[$"{XPre}{node.Node.Name}"];

                if (_columnIndex.TryGetValue($"{YPre}{node.Node.Name}", out yi))
                    _currentLineArray[yi] = 1;

                _currentLineArray[xi] = 1;
                _currentLineArray[v] = node.NormalTime; // node.Node.Length
                _currentLineArray[r] = Equal; // =

                _arrayList.Add(_currentLineArray);
            }
            else if (node.Node.ParentNodes.Count == 1)
            {
                // Tiene un predecesor; restriccion =
                _currentLineArray = new object[_columnIndex.Count];

                var pNode = node.Node.ParentNodes.First();

                // yi = _columnIndex[$"{YPre}{node.Node.Name}"];
                xi = _columnIndex[$"{XPre}{node.Node.Name}"];
                xip = _columnIndex[$"{XPre}{pNode.Name}"];

                if (_columnIndex.TryGetValue($"{YPre}{node.Node.Name}", out yi))
                    _currentLineArray[yi] = 1;

                _currentLineArray[xi] = 1;
                _currentLineArray[xip] = -1;
                _currentLineArray[v] = node.NormalTime; // node.Node.Length
                _currentLineArray[r] = Equal; // =

                _arrayList.Add(_currentLineArray);
            }
            else
            {
                // else if (node.Node.ParentNodes.Count > 1)
                // tiene mas de un predecesor; restriccion >=

                // yi = _columnIndex[$"{YPre}{node.Node.Name}"];
                xi = _columnIndex[$"{XPre}{node.Node.Name}"];

                foreach (var pNode in node.Node.ParentNodes)
                {
                    _currentLineArray = new object[_columnIndex.Count];

                    xip = _columnIndex[$"{XPre}{pNode.Name}"];

                    if (_columnIndex.TryGetValue($"{YPre}{node.Node.Name}", out yi))
                        _currentLineArray[yi] = 1;

                    _currentLineArray[xi] = 1;
                    _currentLineArray[xip] = -1;
                    _currentLineArray[v] = node.NormalTime; // node.Node.Length
                    _currentLineArray[r] = Greater; // >=

                    _arrayList.Add(_currentLineArray);
                }
            }
        }
    }

    private void EndRestrictions()
    {
        int fi = _columnIndex[Fin];
        int ri = _columnIndex[R];
        int pi;
        foreach (var node in NodeSet.Where(n => n.Node.ChildrenNodes.Count == 0))
        {
            _currentLineArray = new object[_columnIndex.Count];

            pi = _columnIndex[$"{XPre}{node.Node.Name}"];

            _currentLineArray[pi] = -1;
            _currentLineArray[fi] = 1;
            _currentLineArray[ri] = Greater; // >=

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
                sb.AppendFormat($"{YPre}{0},", name);
                _columnCount++;
            });
        // X values
        NodeSet.Select(itn => itn.Node)
            .OrderBy(n => n.Name)
            .Select(n => n.Name)
            .ToList().ForEach(name =>
            {
                sb.AppendFormat($"{YPre}{0},", name);
                _columnCount++;
            });
        sb.AppendLine("LHS,RST,RHS");

        // AppendRestrictionM(sb);

        return sb.ToString();
    }
}