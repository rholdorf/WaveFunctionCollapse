using System.Collections.Generic;
using WaveFunctionCollapse.Tiles;

namespace WaveFunctionCollapse;

public class CellCollection
{
    public Cell[][] Cells { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public readonly List<Tile> TileSet;
    
    public CellCollection(int width, int height, List<Tile> tileSet)
    {
        TileSet = tileSet;
        SetBounderies(width, height);
        SetTilesInitialEntropy(tileSet);
    }

    private void SetBounderies(int width, int height)
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

    public void SetTilesInitialEntropy(List<Tile> tiles)
    {
        var initialEntropy = new HashSet<int>();
        for(var i = 0; i < tiles.Count; i++)
            initialEntropy.Add(tiles[i].Index);
        
        for(var x = 0; x < Width; x++)
        {
            for(var y = 0; y < Height; y++)
            {
                Cells[x][y].SetInitialEntropy(initialEntropy);
            }
        }        
    }
}
