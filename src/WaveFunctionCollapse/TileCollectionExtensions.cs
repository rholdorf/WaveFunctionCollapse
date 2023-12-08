using System.Linq;
using WaveFunctionCollapse.Tiles;

namespace WaveFunctionCollapse;

public static class TileCollectionExtensions
{
    public static void FindValidConnections(this Tile[] tiles)
    {
        for (var i = 0; i < tiles.Length; i++)
        {
            var tile = tiles[i];
            for (var j = 0; j < tiles.Length; j++)
            {
                if (i == j) continue; // skip self

                var otherTile = tiles[j];
                if (tile.TopEdge.SequenceEqual(otherTile.BottomEdge)) tile.TopEdgeConnections.Add(otherTile.Index);
                if (tile.RightEdge.SequenceEqual(otherTile.LeftEdge)) tile.RightEdgeConnections.Add(otherTile.Index);
                if (tile.BottomEdge.SequenceEqual(otherTile.TopEdge)) tile.BottomEdgeConnections.Add(otherTile.Index);
                if (tile.LeftEdge.SequenceEqual(otherTile.RightEdge)) tile.LeftEdgeConnections.Add(otherTile.Index);
            }
        }
    }
    
    public static void Fill(this int[][] array, int value)
    {
        for (var x = 0; x < array.Length; x++)
        for (var y = 0; y < array[x].Length; y++)
            array[x][y] = value;
    }
}
