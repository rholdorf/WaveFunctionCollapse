using System;
using System.Collections.Generic;
using WaveFunctionCollapse.Tiles;

namespace WaveFunctionCollapse;

public class CellCollection
{
    public Cell[][] Cells { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Random Random;
    public List<Tile> UniqueTiles;
    
    public CellCollection(int width, int height, List<Tile> uniqueTiles, Random random)
    {
        Random = random;
        UniqueTiles = uniqueTiles;
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
                    Parent = this,
                    CanConnectTop = y > 0,
                    CanConnectRight = x < Width - 1,
                    CanConnectBottom = y < Height - 1,
                    CanConnectLeft = x > 0
                };
            }
        }
    }

    public void SetUniqueTiles(List<Tile> uniqueTiles)
    {
        var candidates = new HashSet<int>();
        for(var i = 0; i < uniqueTiles.Count; i++)
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
