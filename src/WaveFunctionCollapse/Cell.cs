using System;

namespace WaveFunctionCollapse;

public class Cell
{
    public int X { get; }
    public int Y { get; }

    public bool Collapsed { get; set; }
    public int Index { get; set; }
    
    public Cell(int x, int y, int index)
    {
        X = x;
        Y = y;
        Index = index;
    }
    
    public bool CanConnectTop { get; set; }
    public bool CanConnectRight { get; set; }
    public bool CanConnectBottom { get; set; }
    public bool CanConnectLeft { get; set; }
    
    public CellCollection CellCollection { get; init; }
    
    public Cell TopCell => CanConnectTop ? CellCollection.Cells[X][Y - 1] : null;
    public Cell RightCell => CanConnectRight ? CellCollection.Cells[X + 1][Y] : null;
    public Cell BottomCell => CanConnectBottom ? CellCollection.Cells[X][Y + 1] : null;
    public Cell LeftCell => CanConnectLeft ? CellCollection.Cells[X - 1][Y] : null;
}

public class CellCollection
{
    public Cell[][] Cells { get; }
    public int Width { get; }
    public int Height { get; }     
    public CellCollection(int width, int height)
    {
        Width = width;
        Height = height;
        Cells = new Cell[width][];
        for (var i = 0; i < width; i++)
        {
            Cells[i] = new Cell[height];
        }
        
        for(var x = 0; x < width; x++)
        {
            for(var y = 0; y < height; y++)
            {
                Cells[x][y] = new Cell(x, y, -1)
                {
                    CellCollection = this,
                    CanConnectTop = y > 0,
                    CanConnectRight = x < width - 1,
                    CanConnectBottom = y < height - 1,
                    CanConnectLeft = x > 0
                };
            }
        }
    }
}
