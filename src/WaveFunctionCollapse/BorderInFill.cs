using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WaveFunctionCollapse.Tiles;

namespace WaveFunctionCollapse;

public class BorderInFill
{
    private readonly CellCollection _grid;
    private readonly Tile[] _uniqueTiles;
    private readonly Random _random;

    private readonly List<int> _topLeftCorner = new();
    private readonly List<int> _topRightCorner = new();
    private readonly List<int> _bottomLeftCorner = new();
    private readonly List<int> _bottomRightCorner = new();
    private readonly List<int> _top = new();
    private readonly List<int> _right = new();
    private readonly List<int> _bottom = new();
    private readonly List<int> _left = new();

    private readonly int _maxX;
    private readonly int _maxY;

    public BorderInFill(CellCollection grid, Tile[] uniqueTiles, Random random)
    {
        _grid = grid;
        _uniqueTiles = uniqueTiles;
        _random = random;
        _maxX = _grid.Width - 1;
        _maxY = _grid.Height - 1;
    }

    public void Fill(int x, int y)
    {
        LearnBorders();
        CollapseBorders();
        var cell = _grid.Cells[x][y];
        CollapseRecursively(cell);
    }

    private void LearnBorders()
    {
        for (var i = 0; i < _uniqueTiles.Length; i++)
        {
            var tile = _uniqueTiles[i];
            if (tile.IsMiddle)
            {
                continue;
            }

            if (tile.IsTopLeftCorner)
            {
                _topLeftCorner.Add(tile.Index);
                continue;
            }

            if (tile.IsTopRightCorner)
            {
                _topRightCorner.Add(tile.Index);
                continue;
            }

            if (tile.IsTopRow)
            {
                _top.Add(tile.Index);
                continue;
            }

            if (tile.IsBottomLeftCorner)
            {
                _bottomLeftCorner.Add(tile.Index);
                continue;
            }

            if (tile.IsBottomRightCorner)
            {
                _bottomRightCorner.Add(tile.Index);
                continue;
            }

            if (tile.IsBottomRow)
            {
                _bottom.Add(tile.Index); // any bottom, except corners
                continue;
            }

            if (tile.IsRightColumn)
            {
                _right.Add(tile.Index);
                continue;
            }

            // if it gets here, then the tile can only fit on the left border
            _left.Add(tile.Index);
        }
    }

    private void CollapseBorders()
    {
        if (_topLeftCorner.Count == 0)
        {
            _topLeftCorner.AddRange(_top);
        }

        if (_topRightCorner.Count == 0)
        {
            _topRightCorner.AddRange(_top);
        }

        if (_bottomRightCorner.Count == 0)
        {
            _bottomRightCorner.AddRange(_bottom);
        }

        if (_bottomLeftCorner.Count == 0)
        {
            _bottomLeftCorner.AddRange(_bottom);
        }

        SwitchCandidates(_grid.Cells[0][0], _topLeftCorner);
        SwitchCandidates(_grid.Cells[_maxX][0], _topRightCorner);
        SwitchCandidates(_grid.Cells[0][_maxY], _bottomLeftCorner);
        SwitchCandidates(_grid.Cells[_maxX][_maxY], _bottomRightCorner);

        for (var y = 1; y < _grid.Height - 1; y++)
        {
            _grid.Cells[0][y].Collapse();
            _grid.Cells[_maxX][y].Collapse();
        }

        for (var x = 1; x < _grid.Width - 1; x++)
        {
            _grid.Cells[x][0].Collapse();
            _grid.Cells[x][_maxY].Collapse();
        }
    }

    private static void SwitchCandidates(Cell cell, IReadOnlyList<int> values)
    {
        cell.Candidates.Clear();
        cell.Candidates.UnionWith(values);
    }


    private void CollapseRecursively(Cell cell)
    {
        cell.Collapse();

        cell.TopCell?.Collapse();
        cell.RightCell?.Collapse();
        cell.BottomCell?.Collapse();
        cell.LeftCell?.Collapse();

        if (cell.TopCell?.Collapsed == false)
        {
            CollapseRecursively(cell.TopCell); // Top
        }

        if (cell.RightCell?.Collapsed == false)
        {
            CollapseRecursively(cell.RightCell); // Right
        }

        if (cell.BottomCell?.Collapsed == false)
        {
            CollapseRecursively(cell.BottomCell); // Bottom
        }

        if (cell.RightCell?.Collapsed == false)
        {
            CollapseRecursively(cell.LeftCell); // Left
        }
    }
}