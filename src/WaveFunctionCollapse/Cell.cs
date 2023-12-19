using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WaveFunctionCollapse;

public class Cell
{
    public int X { get; }
    public int Y { get; }
    public bool Collapsed { get; set; }
    public int Index { get; set; }
    public bool CanConnectTop { get; init; }
    public bool CanConnectRight { get; init; }
    public bool CanConnectBottom { get; init; }
    public bool CanConnectLeft { get; init; }
    public HashSet<int> Candidates { get; } = new();
    public CellCollection Parent { get; init; }
    public Cell TopCell => CanConnectTop ? Parent.Cells[X][Y - 1] : null;
    public Cell RightCell => CanConnectRight ? Parent.Cells[X + 1][Y] : null;
    public Cell BottomCell => CanConnectBottom ? Parent.Cells[X][Y + 1] : null;
    public Cell LeftCell => CanConnectLeft ? Parent.Cells[X - 1][Y] : null;

    public Cell(int x, int y, int index)
    {
        X = x;
        Y = y;
        Index = index;
    }

    public void ForceCollapse(int index)
    {
        Index = index;
        Collapsed = true;
    }

    public void Collapse()
    {
        if (Collapsed || Candidates.Count==0) return;

        var index = Parent.Random.Next(Candidates.Count);
        Index = Candidates.ElementAt(index);
        Collapsed = true;
        Candidates.Clear();
        Debug.WriteLine($"Collapsed {X}, {Y} to {Index}");

        TopCell?.DecreaseEntropy(this, Position.Top);
        RightCell?.DecreaseEntropy(this, Position.Right);
        BottomCell?.DecreaseEntropy(this, Position.Bottom);
        LeftCell?.DecreaseEntropy(this, Position.Left);
    }

    private void DecreaseEntropy(Cell basedOn, Position position)
    {
        if (Collapsed) return;

        var basedOnTile = Parent.UniqueTiles[basedOn.Index];
        Candidates.IntersectWith(basedOnTile.GetEdgeConnections(position));

        if (Candidates.Count == 1)
        {
            Collapse();
        }
    }
}