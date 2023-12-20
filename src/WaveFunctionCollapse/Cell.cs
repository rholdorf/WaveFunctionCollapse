using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WaveFunctionCollapse;

public class Cell
{
    public int X { get; }
    public int Y { get; }
    public bool Collapsed { get; private set; }
    public int TileIndex { get; set; }
    public bool CanConnectTop { get; init; }
    public bool CanConnectRight { get; init; }
    public bool CanConnectBottom { get; init; }
    public bool CanConnectLeft { get; init; }
    public bool IsMiddle => CanConnectTop && CanConnectRight && CanConnectBottom && CanConnectLeft;
    public HashSet<int> Entropy { get; } = new();
    public CellCollection Parent { get; init; }
    public Cell TopCell => CanConnectTop ? Parent.Cells[X][Y - 1] : null;
    public Cell RightCell => CanConnectRight ? Parent.Cells[X + 1][Y] : null;
    public Cell BottomCell => CanConnectBottom ? Parent.Cells[X][Y + 1] : null;
    public Cell LeftCell => CanConnectLeft ? Parent.Cells[X - 1][Y] : null;

    public Cell(int x, int y, int tileIndex)
    {
        X = x;
        Y = y;
        TileIndex = tileIndex;
    }

    public void SetInitialEntropy(HashSet<int> initialEntropy)
    {
        Entropy.Clear();
        Entropy.UnionWith(initialEntropy);        
    }

    public void Collapse()
    {
        if (Collapsed || Entropy.Count == 0)
        {
            return;
        }

        var index = Random.Shared.Next(Entropy.Count);
        TileIndex = Entropy.ElementAt(index);
        Collapsed = true;
        Entropy.Clear();
        Debug.WriteLine($"Collapsed {X}, {Y} to {TileIndex}");
    }

    public void DecreaseEntropy(int tileIndex, Position position)
    {
        var basedOnTile = Parent.TileSet[tileIndex];
        Entropy.IntersectWith(basedOnTile.GetConnections(position));
        // if (Entropy.Count == 1)
        // {
        //     Collapse();
        // }
    }
}