using System.Collections.Generic;
using WaveFunctionCollapse.Tiles;

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
    
    public bool CanConnectTop { get; init; }
    public bool CanConnectRight { get; init; }
    public bool CanConnectBottom { get; init; }
    public bool CanConnectLeft { get; init; }
    
    public  HashSet<int> Candidates { get; } = new();
    
    public CellCollection CellCollection { get; init; }
    
    public Cell TopCell => CanConnectTop ? CellCollection.Cells[X][Y - 1] : null;
    public Cell RightCell => CanConnectRight ? CellCollection.Cells[X + 1][Y] : null;
    public Cell BottomCell => CanConnectBottom ? CellCollection.Cells[X][Y + 1] : null;
    public Cell LeftCell => CanConnectLeft ? CellCollection.Cells[X - 1][Y] : null;
}

public class CellCollection
{
    public Cell[][] Cells { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }     
    public CellCollection(int width, int height, Tile[] uniqueTiles)
    {
        Initialize(width, height);
        SetUniqueTiles(uniqueTiles);
    }
    
    public CellCollection(int width, int height)
    {
        Initialize(width, height);
    }
    
    private void Initialize(int width, int height)
    {
        Width = width;
        Height = height;
        Cells = new Cell[Width][];
        for (var i = 0; i < Width; i++)
        {
            Cells[i] = new Cell[Height];
        }

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                Cells[x][y] = new Cell(x, y, -1)
                {
                    CellCollection = this,
                    CanConnectTop = y > 0,
                    CanConnectRight = x < Width - 1,
                    CanConnectBottom = y < Height - 1,
                    CanConnectLeft = x > 0
                };
            }
        }
    }

    public void SetUniqueTiles(Tile[] uniqueTiles)
    {
        var candidates = new HashSet<int>();
        for(var i = 0; i < uniqueTiles.Length; i++)
            candidates.Add(i);
        
        for(var x = 0; x < Width; x++)
        {
            for(var y = 0; y < Height; y++)
            {
                Cells[x][y].Candidates.UnionWith(candidates);
            }
        }        
    }
}
