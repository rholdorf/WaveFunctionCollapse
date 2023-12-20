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
        if(cell.Collapsed || cell.Entropy.Count == 0)
            return;
        
        if (!cell.Collapsed)
        {
            cell.Collapse();
        }

        var tile = _uniqueTiles[cell.TileIndex];

        if (cell.CanConnectTop && tile.HasTopCompatibleTileIndices)
        {
            var topCell = cell.TopCell;
            if (!topCell.Collapsed)
            {
                topCell.Entropy.RemoveWhere(o => !tile.TopCompatibleTileIndices.Contains(o));
                if(topCell.Entropy.Count == 1)
                {
                    CollapseRecursively(topCell);
                }
            }
        }

        if (cell.CanConnectRight && tile.HasRightCompatibleTileIndices)
        {
            var rightCell = cell.RightCell;
            if (!rightCell.Collapsed)
            {
                rightCell.Entropy.RemoveWhere(o => !tile.RightCompatibleTileIndices.Contains(o));
                if(rightCell.Entropy.Count == 1)
                {
                    CollapseRecursively(rightCell);
                }
            }
        }

        if (cell.CanConnectBottom && tile.HasBottomCompatibleTileIndices)
        {
            var bottomCell = cell.BottomCell;
            if (!bottomCell.Collapsed)
            {
                bottomCell.Entropy.RemoveWhere(o => !tile.BottomCompatibleTileIndices.Contains(o));
                if(bottomCell.Entropy.Count == 1)
                {
                    CollapseRecursively(bottomCell);
                }
            }
        }

        if (cell.CanConnectLeft && tile.HasLeftCompatibleTileIndices)
        {
            var leftCell = cell.LeftCell;
            if (!leftCell.Collapsed)
            {
                leftCell.Entropy.RemoveWhere(o => !tile.LeftCompatibleTileIndices.Contains(o));
                if(leftCell.Entropy.Count == 1)
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