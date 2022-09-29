namespace PERT_CPM_Console;

public sealed class Cpm
{
    private InitialNode _initial;

    public double? Length { get; private set; } = default;

    public Cpm(InitialNode initial)
    {
        _initial = initial;
    }

    public double StartToEnd()
    {
        // if (Length == null)
        // {
        //     Length = _initial.StartNodes.Max(n => n.ToEnd());
        // }
        // return Length;

        return Length ??= _initial.StartNodes.Max(n => n.ToEnd());
    }

    public void EndToStart()
    {
        // TODO:
    }
}