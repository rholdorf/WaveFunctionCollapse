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
                if (tile.TopEdge.SequenceEqual(otherTile.BottomEdge)) tile.TopEdgeConnections.Add(otherTile);
                if (tile.RightEdge.SequenceEqual(otherTile.LeftEdge)) tile.RightEdgeConnections.Add(otherTile);
                if (tile.BottomEdge.SequenceEqual(otherTile.TopEdge)) tile.BottomEdgeConnections.Add(otherTile);
                if (tile.LeftEdge.SequenceEqual(otherTile.RightEdge)) tile.LeftEdgeConnections.Add(otherTile);
            }
        }
    }

    public static void Fill(this int[,] array, int value)
    {
        for (var i = 0; i < array.GetLength(0); i++)
        for (var j = 0; j < array.GetLength(1); j++)
            array[i, j] = value;
    }
}
