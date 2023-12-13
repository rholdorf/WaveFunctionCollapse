using System;
using System.Diagnostics;
using System.Linq;
using WaveFunctionCollapse.Tiles;

namespace WaveFunctionCollapse;

public class FloodFill
{
    private readonly CellCollection _grid;
    private readonly Tile[] _uniqueTiles;
    private readonly Random _random;

    public FloodFill(CellCollection grid, Tile[] uniqueTiles, Random random)
    {
        _grid = grid;
        _uniqueTiles = uniqueTiles;
        _random = random;
    }

    public void Fill(int x, int y)
    {
        var cell = _grid.Cells[x][y];
        CollapseRecursively(cell);
    }

    private void CollapseRecursively(Cell cell)
    {
        if(cell.Collapsed || cell.Candidates.Count == 0)
            return;
        
        if (!cell.Collapsed)
        {
            var index = _random.Next(cell.Candidates.Count);
            cell.Index = cell.Candidates.ElementAt(index);
            cell.Collapsed = true;
            cell.Candidates.Clear();
            Debug.WriteLine($"Collapsed {cell.X}, {cell.Y} to {cell.Index}");
        }

        var tile = _uniqueTiles[cell.Index];

        if (cell.CanConnectTop && tile.HasTopEdgeConnection)
        {
            var topCell = cell.TopCell;
            if (!topCell.Collapsed)
            {
                topCell.Candidates.RemoveWhere(o => !tile.TopEdgeConnections.Contains(o));
                if(topCell.Candidates.Count == 1)
                {
                    CollapseRecursively(topCell);
                }
            }
        }

        if (cell.CanConnectRight && tile.HasRightEdgeConnection)
        {
            var rightCell = cell.RightCell;
            if (!rightCell.Collapsed)
            {
                rightCell.Candidates.RemoveWhere(o => !tile.RightEdgeConnections.Contains(o));
                if(rightCell.Candidates.Count == 1)
                {
                    CollapseRecursively(rightCell);
                }
            }
        }

        if (cell.CanConnectBottom && tile.HasBottomEdgeConnection)
        {
            var bottomCell = cell.BottomCell;
            if (!bottomCell.Collapsed)
            {
                bottomCell.Candidates.RemoveWhere(o => !tile.BottomEdgeConnections.Contains(o));
                if(bottomCell.Candidates.Count == 1)
                {
                    CollapseRecursively(bottomCell);
                }
            }
        }

        if (cell.CanConnectLeft && tile.HasLeftEdgeConnection)
        {
            var leftCell = cell.LeftCell;
            if (!leftCell.Collapsed)
            {
                leftCell.Candidates.RemoveWhere(o => !tile.LeftEdgeConnections.Contains(o));
                if(leftCell.Candidates.Count == 1)
                {
                    CollapseRecursively(leftCell);
                }
            }
        }

        if (cell.CanConnectTop && !cell.TopCell.Collapsed) CollapseRecursively(cell.TopCell); // Top
        if (cell.CanConnectRight && !cell.RightCell.Collapsed) CollapseRecursively(cell.RightCell); // Right
        if (cell.CanConnectBottom && !cell.BottomCell.Collapsed) CollapseRecursively(cell.BottomCell); // Bottom
        if (cell.CanConnectLeft && !cell.LeftCell.Collapsed) CollapseRecursively(cell.LeftCell); // Left
    }
}

