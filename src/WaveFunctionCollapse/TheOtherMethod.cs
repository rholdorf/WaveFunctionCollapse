using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WaveFunctionCollapse.Tiles;

namespace WaveFunctionCollapse;

public class TheOtherMethod
{
    private readonly CellCollection _grid;

    public TheOtherMethod(CellCollection grid)
    {
        _grid = grid;
    }

    public void Fill(int x, int y)
    {
        var cell = _grid.Cells[x][y];
        CollapseCellAndDecreaseNeighboursEntropy(cell);
    }

    private void CollapseCellAndDecreaseNeighboursEntropy(Cell cell)
    {
        if (cell.CanCollapse())
            cell.Collapse();

        var stack = new Stack<Cell>();
        if (cell.TopCell.CanCollapse())
        {
            cell.TopCell.DecreaseEntropy(cell.TileIndex, Position.Top);
            stack.Push(cell.TopCell);
        }

        if (cell.RightCell.CanCollapse())
        {
            cell.RightCell?.DecreaseEntropy(cell.TileIndex, Position.Right);
            stack.Push(cell.RightCell);
        }

        if (cell.BottomCell.CanCollapse())
        {
            cell.BottomCell?.DecreaseEntropy(cell.TileIndex, Position.Bottom);
            stack.Push(cell.BottomCell);
        }

        if (cell.LeftCell.CanCollapse())
        {
            cell.LeftCell?.DecreaseEntropy(cell.TileIndex, Position.Left);
            stack.Push(cell.LeftCell);
        }

        while (stack.Count > 0)
        {
            var nextCell = stack.Pop();
            if (nextCell.CanCollapse())
            {
                CollapseCellAndDecreaseNeighboursEntropy(nextCell);
            }
        }        

    }


}